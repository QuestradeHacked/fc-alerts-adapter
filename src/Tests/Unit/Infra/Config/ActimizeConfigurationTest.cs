using Unit.Faker;

namespace Unit.Infra.Config;

public class ActimizeConfigurationValidateTest
{
    private readonly ActimizeConfigurationFaker _actimizeConfigurationFaker = new();

    [Fact]
    public void Validate_ShouldNotThrowException_WhenConfigurationIsValid()
    {
        // Arrange
        var actimizeConfiguration = _actimizeConfigurationFaker.GenerateValidConfiguration();

        //Act
        var exception = Record.Exception(() => actimizeConfiguration.Validate());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Validate_ShouldThrowException_WhenUserNameIsNotValid()
    {
        // Arrange
        var actimizeConfiguration = _actimizeConfigurationFaker.GenerateValidConfiguration();
        actimizeConfiguration.EasyIngestApiUserName = string.Empty;

        // Assert
        Assert.Throws<InvalidOperationException>(() =>
        actimizeConfiguration.Validate());
    }

    [Fact]
    public void Validate_ShouldThrowException_WhenUserPasswordIsNotValid()
    {
        // Arrange
        var actimizeConfiguration = _actimizeConfigurationFaker.GenerateValidConfiguration();
        actimizeConfiguration.EasyIngestApiPassword = string.Empty;

        // Assert
        Assert.Throws<InvalidOperationException>(() =>
        actimizeConfiguration.Validate());
    }

    [Fact]
    public void Validate_ShouldThrowException_WhenBaseUrlIsNotValid()
    {
        // Arrange
        var actimizeConfiguration = _actimizeConfigurationFaker.GenerateValidConfiguration();
        actimizeConfiguration.EasyIngestBaseUrl = string.Empty;

        // Assert
        Assert.Throws<InvalidOperationException>(() =>
            actimizeConfiguration.Validate());
    }
}
