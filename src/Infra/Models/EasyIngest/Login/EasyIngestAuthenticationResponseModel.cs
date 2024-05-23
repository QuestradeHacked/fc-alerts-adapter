using System.Net;
using Infra.Constants;

namespace Infra.Models.EasyIngest.Login;

public class EasyIngestAuthenticationResponseModel
{
    public string? CsrfToken { get; set; }

    public EasyIngestAuthenticationStatus AuthenticationStatus { get; set; }

    public HttpStatusCode? ResultHttpStatusCode { get; set; }
}
