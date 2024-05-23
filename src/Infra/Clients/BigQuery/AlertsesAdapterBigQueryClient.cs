using Google.Cloud.BigQuery.V2;
using Infra.Config;

namespace Infra.Clients.BigQuery;

public class AlertsesAdapterBigQueryClient : IAlertsAdapterBigQueryClient
{
    private readonly BigQueryClient _client;
    private readonly BigQueryConfig _config;

    public AlertsesAdapterBigQueryClient(BigQueryConfig config)
    {
        _config = config;
        _client = BigQueryClient.Create(_config.ProjectId);
    }

    public async Task<BigQueryResults> GetRowsAsync(CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT" + $" * FROM {_config.DatasetName}.{_config.TableName}";

        return await _client.ExecuteQueryAsync(sql, null, cancellationToken: cancellationToken);
    }

    public async Task UpdateRowsAsync(string customerId, string columnName, string updatedValue,
        CancellationToken cancellationToken = default)
    {
        var updateSql = $@"MERGE {_config.DatasetName}.{_config.TableName} AS target
                        USING ( SELECT * FROM {_config.DatasetName}.{_config.TableName} WHERE CustomerId = @customerId ) AS source
                        ON target.CustomerId = source.CustomerId WHEN MATCHED THEN UPDATE SET target.{columnName} = @updatedValue;";

        var parameters = new[]
        {
            new BigQueryParameter("customerId", BigQueryDbType.String, customerId),
            new BigQueryParameter("updatedValue", BigQueryDbType.String, updatedValue)
        };

        await _client.ExecuteQueryAsync(updateSql, parameters, cancellationToken: cancellationToken);

    }
}
