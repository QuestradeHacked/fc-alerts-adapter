using Domain.Services;
using Google.Apis.Bigquery.v2.Data;
using Google.Cloud.BigQuery.V2;
using Infra.Clients.BigQuery;
using Infra.Repository.BigQuery;
using Infra.Utils;
using Microsoft.Extensions.Logging;
using Moq;
using NSubstitute;
using Xunit;

namespace Integration;

public class BigQueryRepositoryIntegrationTest
{
    private readonly BigQueryRepository _repository;
    private readonly IAlertsAdapterBigQueryClient _client;

    public BigQueryRepositoryIntegrationTest()
    {
        var bigQueryCommonLogs = Substitute.For<BigQueryCommonLogs>();
        var metricService = Substitute.For<IMetricService>();

        var mockLogger = new Mock<ILogger<BigQueryRepository>>();

        _client = Substitute.For<IAlertsAdapterBigQueryClient>();
        _repository = new BigQueryRepository(mockLogger.Object, metricService, _client, bigQueryCommonLogs);
    }

    [Fact]
    public async Task GetRecordsAsync_ShouldReturnDataWithSuccess_WhenBigQueryTableHaveNewRecords()
    {
        // Arrange
        var bigQueryClient = Substitute.For<BigQueryClient>();
        var queryResults = new GetQueryResultsResponse { Rows = new[] { new TableRow() }, Schema = new TableSchema() };

        var expectedResults = new BigQueryResults(bigQueryClient, queryResults, null, null);

        _client.GetRowsAsync().ReturnsForAnyArgs(expectedResults);

        // Act
        var result = await _repository.GetRecordsAsync();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateRowAsync_ShouldBeExecutedSuccessfully_WhenIncomingEasyIngestDataIsReceivedCorrectly()
    {
        // Arrange
        var faker = new Bogus.Faker();

        var customerId = faker.Random.Uuid().ToString();
        var columnName = faker.Random.Word();
        var updatedValue = faker.Address.Country();

        // Act & Assert
        try
        {
            await _repository.UpdateRowAsync(customerId, columnName, updatedValue);
        }
        catch (Exception ex)
        {
            Assert.Fail($"Unexpected exception: {ex.Message}");
        }
    }
}
