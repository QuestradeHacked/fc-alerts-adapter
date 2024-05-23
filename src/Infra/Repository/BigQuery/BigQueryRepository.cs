using Domain.Constants;
using Domain.Services;
using Google;
using Google.Cloud.BigQuery.V2;
using Infra.Clients.BigQuery;
using Infra.Constants;
using Infra.Utils;
using Microsoft.Extensions.Logging;
using SerilogTimings;

namespace Infra.Repository.BigQuery;

public class BigQueryRepository : IBigQueryRepository
{
    private readonly IAlertsAdapterBigQueryClient _client;
    private readonly ILogger<BigQueryRepository> _logger;
    private readonly IMetricService _metricService;
    private readonly BigQueryCommonLogs _bigQueryCommonLogs;

    public BigQueryRepository(ILogger<BigQueryRepository> logger, IMetricService metricService,
        IAlertsAdapterBigQueryClient client, BigQueryCommonLogs bigQueryCommonLogs)
    {
        _metricService = metricService;
        _client = client;
        _logger = logger;
        _bigQueryCommonLogs = bigQueryCommonLogs;
    }

    public async Task<BigQueryResults> GetRecordsAsync()
    {
        try
        {
            _metricService.Increment(
                    BigQueryMetricTags.GetDataFromBigQueryRequest,
                    new List<string> {MetricTags.StatusSuccess}
            );
            var timing = Operation.Begin(
                "Fetching All data from BigQuery"
            );
            using (Operation.Time("Retrieving BigQuery Data"))
            {
                var dataSet = await _client.GetRowsAsync();

                timing.Complete();
                _metricService.Distribution(
                    BigQueryMetricTags.GetDataFromBigQueryRequest, timing.Elapsed.TotalMilliseconds,
                    new List<string>
                        {
                            MetricTags.StatusSuccess, BigQueryMetricTags.GetDataFromBigQueryRequestReceived
                        }
                    );

                return dataSet;
            }
        }
        catch (Exception exception)
        {
            _metricService.Increment(
                BigQueryMetricTags.GetDataFromBigQueryRequest,
                new List<string>
                    {
                        MetricTags.StatusPermanentError,
                        BigQueryMetricTags.GetDataFromBigQueryRequest
                    }
                );

            _bigQueryCommonLogs.BigQueryLogCriticalError(
                _logger,
                nameof(GetRecordsAsync),
                "Error retrieving records from BigQuery",
                exception);

            throw;
        }
    }

    public async Task UpdateRowAsync(string customerId, string columnName, string updateValue)
    {
        try
        {
            _metricService.Increment(
                BigQueryMetricTags.UpdateBigQueryDataRequest,
                new List<string>{MetricTags.StatusSuccess}
            );
            var timing = Operation.Begin(
                "Modify BigQuery Data set {columnName} with value of {updateValue}",columnName,updateValue
            );
            using (Operation.Time(
                       $"Update BigQueryData Record CustomerID : {customerId}, ColumnName : {columnName} with update value of : {updateValue} "))
            {
                await _client.UpdateRowsAsync(customerId, columnName, updateValue);
                timing.Complete();
                _metricService.Distribution(
                    BigQueryMetricTags.UpdateBigQueryDataRequest,timing.Elapsed.TotalMilliseconds,
                    new List<string>
                    {
                        MetricTags.StatusSuccess, BigQueryMetricTags.UpdateBigQueryDataRequestReceived
                    }
                );
            }
        }
        catch (GoogleApiException exception)
        {
            _metricService.Increment(
                    BigQueryMetricTags.UpdateBigQueryDataRequest,
                    new List<string>
                    {
                        MetricTags.StatusPermanentError,
                        BigQueryMetricTags.UpdateBigQueryDataRequest
                    }
            );
            _bigQueryCommonLogs.BigQueryLogCriticalError(
                _logger,
                nameof(GetRecordsAsync),
                "Data cannot be Updated to the Database",
                exception);
            throw;
        }
    }
}
