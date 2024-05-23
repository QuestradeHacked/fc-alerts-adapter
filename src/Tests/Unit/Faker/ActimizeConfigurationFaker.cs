using Bogus;
using Infra.Config;

namespace Unit.Faker;

public class ActimizeConfigurationFaker
{
    private readonly Faker<ActimizeConfiguration> _fakerActimizeConfiguration = new();

    public ActimizeConfiguration GenerateValidConfiguration()
    {
        return _fakerActimizeConfiguration
            .RuleFor(configuration => configuration.EasyIngestApiUserName, faker => faker.Internet.UserName())
            .RuleFor(configuration => configuration.EasyIngestApiPassword, faker => faker.Internet.Password())
            .RuleFor(configuration => configuration.EasyIngestBaseUrl, faker => faker.Internet.Url())
            .RuleFor(configuration => configuration.Enable, true);
    }
}
