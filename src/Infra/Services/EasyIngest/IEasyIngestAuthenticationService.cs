using Infra.Models.EasyIngest.Login;

namespace Infra.Services.EasyIngest;

public interface IEasyIngestAuthenticationService
{
    Task<EasyIngestAuthenticationResponseModel> ActimizeAuthenticationAsync();
}
