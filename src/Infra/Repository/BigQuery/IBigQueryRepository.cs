using Google.Cloud.BigQuery.V2;

namespace Infra.Repository.BigQuery;

public interface IBigQueryRepository
{
    Task<BigQueryResults> GetRecordsAsync();
    Task UpdateRowAsync(string customerId, string columnName, string updateValue);
}
