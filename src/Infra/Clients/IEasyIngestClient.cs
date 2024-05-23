using Infra.Models.EasyIngest.Login;
using Infra.Models.EasyIngest.WorkItems;
using Refit;

namespace Infra.Clients;

public interface IEasyIngestClient
{
    [Post("/public/v1/auth/login")]
    Task<IApiResponse> LoginAsync([Body] EasyIngestLoginRequestModel request);

    [Post("/v1/work-items/{alertTypeIdentifier}/mappings/{mappingName}")]
    Task<IApiResponse<WorkItemsResponseModel>> WorkItemsAsync(
        [Header("CSRFTOKEN")] string csrfToken,
        [AliasAs("alertTypeIdentifier")] string alertTypeIdentifier,
        [AliasAs("mappingName")] string mappingName,
        [Body] WorkItemRequestModel alertItem);
}
