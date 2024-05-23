using Infra.Constants;
using Infra.Models.EasyIngest.Login;

namespace Unit.Faker;

public static class EasyIngestAuthenticationResponseModelFaker
{
    public static EasyIngestAuthenticationResponseModel GenerateValidAuthenticationResponse()
    {
        var faker = new Bogus.Faker();

        return new EasyIngestAuthenticationResponseModel
        {
            AuthenticationStatus = EasyIngestAuthenticationStatus.Success,
            CsrfToken = faker.Random.Guid().ToString()
        };
    }

    public static EasyIngestAuthenticationResponseModel GenerateInvalidAuthenticationResponse()
    {
        var faker = new Bogus.Faker();

        return new EasyIngestAuthenticationResponseModel
        {
            AuthenticationStatus = EasyIngestAuthenticationStatus.Error,
            CsrfToken = null
        };
    }
}
