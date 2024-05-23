using Domain.Services;
using Infra.Models.Messages;
using Infra.Services.Subscriber;
using Integration.Fixture;
using Integration.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NSubstitute;
using Questrade.Library.PubSubClientHelper.Primitives;
using Xunit;

namespace Integration.Infra.Subscriber;

public class AlertsAdapterSubscriberTest : IAssemblyFixture<PubSubEmulatorProcessFixture>
{
    private readonly AlertsAdapterSubscriber _alertsAdapterSubscriber;

    private readonly IMetricService _metricService;

    private readonly MockLogger<AlertsAdapterSubscriberTest> _logger;

    private readonly PubSubEmulatorProcessFixture _pubSubFixture;

    private readonly int _subscriberTimeout;

    private readonly string _topicId;

    public AlertsAdapterSubscriberTest(PubSubEmulatorProcessFixture pubSubFixture)
    {
        Mock<ILoggerFactory> loggerFactory = new();

        _logger = new MockLogger<AlertsAdapterSubscriberTest>();
        loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(_logger);

        _metricService = Substitute.For<IMetricService>();
        _pubSubFixture = pubSubFixture;
        _subscriberTimeout = _pubSubFixture.SubscriberTimeout;
        _topicId = $"T_{Guid.NewGuid()}";

        var services = new ServiceCollection();

        services.AddSingleton<IDefaultJsonSerializerOptionsProvider, MyDefaultJsonSerializerOptionsProvider>();
        var serviceProvider = services.AddMemoryCache().BuildServiceProvider();
        var subscriptionId = $"{_topicId}.{Guid.NewGuid()}";
        var subscriberConfig = _pubSubFixture.CreateDefaultSubscriberConfig(subscriptionId);

        _alertsAdapterSubscriber = new AlertsAdapterSubscriber(
            subscriberConfig,
            loggerFactory.Object,
            _metricService,
            serviceProvider
        );

        _pubSubFixture.CreateTopic(_topicId);
        _pubSubFixture.CreateSubscription(_topicId, subscriptionId);
    }

    [Theory]
    [MemberData(nameof(GetPossibleInvalidMessages))]
    public async Task HandleReceivedMessageAsync_ShouldLogWarning_WhenRowCountOrTimeCreatedIsNull(AlertsAdapterMessage alertsAdapterMessage)
    {
        // Arrange
        var publisher = await _pubSubFixture.CreatePublisherAsync(_topicId);

        // Act
        await publisher.PublishAsync(JsonConvert.SerializeObject(alertsAdapterMessage));
        await _alertsAdapterSubscriber.StartAsync(CancellationToken.None);
        await Task.Delay(_subscriberTimeout);
        await _alertsAdapterSubscriber.StopAsync(CancellationToken.None);

        var loggedMessages = _logger.GetAllMessages();

        // Assert
        Assert.Contains("A message with RowCount or TimeCreated null was received", loggedMessages);
        _metricService.Received(1).Increment(
            metricName: Arg.Any<string>(),
            metricTags: Arg.Any<List<string>>()
        );
    }

    //TODO: Replace this temporary test with the real and successful one later
    [Fact]
    public async Task HandleReceivedMessageAsync_ShouldLogInformation_WhenTemporaryWorkflowIsDone()
    {
        // Arrange
        var faker = new Bogus.Faker();
        var publisher = await _pubSubFixture.CreatePublisherAsync(_topicId);
        var alertsAdapterMessage = new AlertsAdapterMessage
        {
            RowCount = faker.Random.Int(),
            TimeCreated = faker.Date.Recent()
        };

        // Act
        await publisher.PublishAsync(JsonConvert.SerializeObject(alertsAdapterMessage));
        await _alertsAdapterSubscriber.StartAsync(CancellationToken.None);
        await Task.Delay(_subscriberTimeout);
        await _alertsAdapterSubscriber.StopAsync(CancellationToken.None);

        var loggedMessages = _logger.GetAllMessages();

        // Assert
        Assert.Contains("[temp] Message processed", loggedMessages);
        _metricService.Received(2).Increment(
            metricName: Arg.Any<string>(),
            metricTags: Arg.Any<List<string>>()
        );
    }

    public static IEnumerable<object[]> GetPossibleInvalidMessages()
    {
        var faker = new Bogus.Faker();

        yield return new object[]
        {
            new AlertsAdapterMessage
            {
                RowCount = faker.Random.Int(),
                TimeCreated = null
            }
        };

        yield return new object[]
        {
            new AlertsAdapterMessage
            {
                RowCount = null,
                TimeCreated = faker.Date.Recent()
            }
        };

        yield return new object[]
        {
            new AlertsAdapterMessage
            {
                RowCount = null,
                TimeCreated = null
            }
        };
    }
}
