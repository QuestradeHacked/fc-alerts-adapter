using Infra.Constants;
using Infra.Models.EasyIngest.Login;
using Refit;

namespace Infra.Extensions;

public static class EasyIngestApiResponse
{
    private const string AuthTokenName = "CSRFTOKEN";

    public static EasyIngestAuthenticationResponseModel GetAuthenticationData(this IApiResponse response)
    {
        var result = new EasyIngestAuthenticationResponseModel
        {
            ResultHttpStatusCode = response.StatusCode
        };

        if (response.IsSuccessStatusCode)
        {
            result.CsrfToken = response.Headers.GetValues(AuthTokenName).FirstOrDefault();
            result.AuthenticationStatus = EasyIngestAuthenticationStatus.Success;
            return result;
        }

        result.AuthenticationStatus = EasyIngestAuthenticationStatus.Fail;

        return result;
    }
}
