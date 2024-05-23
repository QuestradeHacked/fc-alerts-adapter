using Infra.Config.PubSub;

namespace Unit.Infra.Config.PubSub;

public class AlertsAdapterSubscriberConfigurationTest
{
    private readonly Bogus.Faker _faker = new();

    [Fact]
    public void Validate_ShouldRaiseException_WhenProjectIdIsNull()
    {
        // Arrange
        AlertsAdapterSubscriberConfiguration alertsAdapterSubscriberConfiguration = new()
        {
            ProjectId = null
        };

        // Act
        var exception = Record.Exception(() => alertsAdapterSubscriberConfiguration.Validate());

        // Assert
        Assert.IsType<InvalidOperationException>(exception);
        Assert.Equal("The configuration options for the AlertsAdapterSubscriber is not valid", exception.Message);
    }

    [Fact]
    public void Validate_ShouldRaiseException_WhenProjectIdIsEmpty()
    {
        // Arrange
        AlertsAdapterSubscriberConfiguration alertsAdapterSubscriberConfiguration = new()
        {
            ProjectId = string.Empty
        };

        // Act
        var exception = Record.Exception(() => alertsAdapterSubscriberConfiguration.Validate());

        // Assert
        Assert.IsType<InvalidOperationException>(exception);
        Assert.Equal("The configuration options for the AlertsAdapterSubscriber is not valid", exception.Message);
    }

    [Fact]
    public void Validate_ShouldRaiseException_WhenProjectIdIsAWhiteSpace()
    {
        // Arrange
        AlertsAdapterSubscriberConfiguration alertsAdapterSubscriberConfiguration = new()
        {
            ProjectId = " "
        };

        // Act
        var exception = Record.Exception(() => alertsAdapterSubscriberConfiguration.Validate());

        // Assert
        Assert.IsType<InvalidOperationException>(exception);
        Assert.Equal("The configuration options for the AlertsAdapterSubscriber is not valid", exception.Message);
    }

    [Fact]
    public void Validate_ShouldRaiseException_WhenSubscriptionIdIsNull()
    {
        // Arrange
        AlertsAdapterSubscriberConfiguration alertsAdapterSubscriberConfiguration = new()
        {
            ProjectId = _faker.Internet.Random.ToString(),
            SubscriptionId = null
        };

        // Act
        var exception = Record.Exception(() => alertsAdapterSubscriberConfiguration.Validate());

        // Assert
        Assert.IsType<InvalidOperationException>(exception);
        Assert.Equal("The configuration options for the AlertsAdapterSubscriber is not valid", exception.Message);
    }

    [Fact]
    public void Validate_ShouldRaiseException_WhenSubscriptionIdIsEmpty()
    {
        // Arrange
        AlertsAdapterSubscriberConfiguration alertsAdapterSubscriberConfiguration = new()
        {
            ProjectId = _faker.Internet.Random.ToString(),
            SubscriptionId = string.Empty
        };

        // Act
        var exception = Record.Exception(() => alertsAdapterSubscriberConfiguration.Validate());

        // Assert
        Assert.IsType<InvalidOperationException>(exception);
        Assert.Equal("The configuration options for the AlertsAdapterSubscriber is not valid", exception.Message);
    }

    [Fact]
    public void Validate_ShouldRaiseException_WhenSubscriptionIdIsAWhiteSpace()
    {
        // Arrange
        AlertsAdapterSubscriberConfiguration alertsAdapterSubscriberConfiguration = new()
        {
            ProjectId = _faker.Internet.Random.ToString(),
            SubscriptionId = " "
        };

        // Act
        var exception = Record.Exception(() => alertsAdapterSubscriberConfiguration.Validate());

        // Assert
        Assert.IsType<InvalidOperationException>(exception);
        Assert.Equal("The configuration options for the AlertsAdapterSubscriber is not valid", exception.Message);
    }

    [Fact]
    public void Validate_ShouldRaiseException_WhenEmulatorIsTrue_AndEndpointIsEmpty()
    {
        // Arrange
        AlertsAdapterSubscriberConfiguration alertsAdapterSubscriberConfiguration = new()
        {
            ProjectId = _faker.Internet.Random.ToString(),
            SubscriptionId = _faker.Internet.Random.ToString(),
            UseEmulator = true,
            Endpoint = string.Empty
        };

        // Act
        var exception = Record.Exception(() => alertsAdapterSubscriberConfiguration.Validate());

        // Assert
        Assert.IsType<InvalidOperationException>(exception);
        Assert.Equal("The emulator configuration options for AlertsAdapterSubscriber is not valid",
            exception.Message);
    }

    [Fact]
    public void Validate_ShouldRaiseException_WhenEmulatorIsTrue_AndEndpointIsNull()
    {
        // Arrange
        AlertsAdapterSubscriberConfiguration alertsAdapterSubscriberConfiguration = new()
        {
            ProjectId = _faker.Internet.Random.ToString(),
            SubscriptionId = _faker.Internet.Random.ToString(),
            UseEmulator = true,
            Endpoint = null
        };

        // Act
        var exception = Record.Exception(() => alertsAdapterSubscriberConfiguration.Validate());

        // Assert
        Assert.IsType<InvalidOperationException>(exception);
        Assert.Equal("The emulator configuration options for AlertsAdapterSubscriber is not valid",
            exception.Message);
    }

    [Fact]
    public void Validate_ShouldRaiseException_WhenEmulatorIsTrue_AndEndpointASpace()
    {
        // Arrange
        AlertsAdapterSubscriberConfiguration alertsAdapterSubscriberConfiguration = new()
        {
            ProjectId = _faker.Internet.Random.ToString(),
            SubscriptionId = _faker.Internet.Random.ToString(),
            UseEmulator = true,
            Endpoint = " "
        };

        // Act
        var exception = Record.Exception(() => alertsAdapterSubscriberConfiguration.Validate());

        // Assert
        Assert.IsType<InvalidOperationException>(exception);
        Assert.Equal("The emulator configuration options for AlertsAdapterSubscriber is not valid",
            exception.Message);
    }

    [Fact]
    public void Validate_ShouldNotThrow_WhenAllConfigurationsAreCorrect()
    {
        // Arrange
        AlertsAdapterSubscriberConfiguration alertsAdapterSubscriberConfiguration = new()
        {
            Enable = false
        };

        // Act
        var exception = Record.Exception(() => alertsAdapterSubscriberConfiguration.Validate());

        // Assert
        Assert.Null(exception);
    }
}
