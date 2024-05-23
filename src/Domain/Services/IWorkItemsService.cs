using Domain.Models.WorkItems;

namespace Domain.Services;

public interface IWorkItemsService
{
    Task<bool> IngestWorkItemAsync(AlertEntity alertEntity, CancellationToken cancellationToken);
}
