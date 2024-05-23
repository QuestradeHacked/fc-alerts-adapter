using Domain.Constants;
using Infra.Services.DataDog;
using NSubstitute;
using StatsdClient;

namespace Unit.Infra.Services.DataDog;

public class DataDogMetricServiceTest
{
    private readonly IDogStatsd _dogStatsd = Substitute.For<IDogStatsd>();

    private readonly Bogus.Faker _faker = new();

    private readonly DataDogMetricService _dataDogMetricService;

    public DataDogMetricServiceTest()
    {
        _dataDogMetricService = Substitute.For<DataDogMetricService>(_dogStatsd);
    }

    [Fact]
    public void Distribution_ShouldCallDogStatsdClient()
    {
        // Arrange
        var latency = _faker.Random.Double(500,1500);

        // Act
        _dataDogMetricService.Distribution(
            MetricNames.EasyIngestApiRequest,
            latency,
            new List<string>()
        );

        var exception = Record.Exception
        (
            () => _dogStatsd.Received(1).Distribution(
            Arg.Is(MetricNames.EasyIngestApiRequest),
            Arg.Is(latency),
            tags: Arg.Any<string[]>())
        );

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Increment_ShouldCallDogStatsdClient()
    {
        // Arrange
        var tags = new List<string>();

        // Act
        _dataDogMetricService.Increment(
            MetricNames.EasyIngestApiRequest,
            tags
        );

        var exception = Record.Exception
        (
            () => _dogStatsd.Received(1).Increment(
                    Arg.Is(MetricNames.EasyIngestApiRequest),
                    tags: Arg.Any<string[]>())
        );

        // Assert
        Assert.Null(exception);
    }
}
