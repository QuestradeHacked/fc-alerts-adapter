using Google.Cloud.BigQuery.V2;

namespace Infra.Clients.BigQuery;

public interface IAlertsAdapterBigQueryClient
{
    Task<BigQueryResults> GetRowsAsync(CancellationToken cancellationToken = default);

    public Task UpdateRowsAsync(string customerId, string columnName, string updatedValue,
        CancellationToken cancellationToken = default);
}
