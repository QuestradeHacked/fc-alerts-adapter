using Domain.Constants;
using Domain.Services;
using Google;
using Google.Apis.Bigquery.v2.Data;
using Google.Cloud.BigQuery.V2;
using Infra.Clients.BigQuery;
using Infra.Constants;
using Infra.Repository.BigQuery;
using Infra.Utils;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Xunit.Sdk;
using Assert = Xunit.Assert;

namespace Unit.Infra.Repository.BigQuery
{
    [TestFixture]
    public class BigQueryRepositoryTests
    {
        private Mock<IAlertsAdapterBigQueryClient> _clientMock;
        private Mock<IMetricService> _metricServiceMock;
        private Mock<ILogger<BigQueryRepository>> _loggerMock;
        private Mock<BigQueryCommonLogs> _bigQueryCommonLogsMock;
        private BigQueryRepository _bigQueryRepository;

        public BigQueryRepositoryTests(Mock<IAlertsAdapterBigQueryClient> clientMock, Mock<IMetricService> metricServiceMock, Mock<ILogger<BigQueryRepository>> loggerMock, Mock<BigQueryCommonLogs> bigQueryCommonLogsMock, BigQueryRepository bigQueryRepository)
        {
            _clientMock = clientMock;
            _metricServiceMock = metricServiceMock;
            _loggerMock = loggerMock;
            _bigQueryCommonLogsMock = bigQueryCommonLogsMock;
            _bigQueryRepository = bigQueryRepository;
        }

        [SetUp]
        public void Setup()
        {
            _clientMock = new Mock<IAlertsAdapterBigQueryClient>();
            _metricServiceMock = new Mock<IMetricService>();
            _loggerMock = new Mock<ILogger<BigQueryRepository>>();
            _bigQueryCommonLogsMock = new Mock<BigQueryCommonLogs>();

            _bigQueryRepository = new BigQueryRepository(
                _loggerMock.Object,
                _metricServiceMock.Object,
                _clientMock.Object,
                _bigQueryCommonLogsMock.Object
            );
        }

        [Test]
        public void GetRecordsAsync_WhenThrowingNullData_ShouldHandleException()
        {
            // Arrange
            _clientMock.Setup(x => x.GetRowsAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentNullException());

            // Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _bigQueryRepository.GetRecordsAsync());
        }


        [Test] public async Task GetRecordsAsync_WhenNoDataFound()
        {
            // Arrange
            var expectedDataSet = GenerateExpectedDataSet();

            _clientMock.Setup(x => x.GetRowsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((BigQueryResults)expectedDataSet);

            // Act
            var result = await _bigQueryRepository.GetRecordsAsync();

            // Assert
            Assert.Equivalent(expectedDataSet, result);

            _metricServiceMock.Verify(
                x => x.Increment(BigQueryMetricTags.GetDataFromBigQueryRequest, It.IsAny<List<string>>()),
                Times.Once
            );

        }

        private BigQueryResults GenerateExpectedDataSet()
        {
            throw new NullException(null!);
        }


        [Test]
        public void GetRecordsAsync_Exception()
        {
            // Arrange
            _clientMock.Setup(x => x.GetRowsAsync(It.IsAny<CancellationToken>()))
                       .ThrowsAsync(new Exception("Simulated exception"));

            // Act & Assert
            Xunit.Assert.ThrowsAsync<Exception>(async () =>
                await _bigQueryRepository.GetRecordsAsync()
            );

            _metricServiceMock.Verify(
                x => x.Increment(BigQueryMetricTags.GetDataFromBigQueryRequest, It.IsAny<List<string>>()),
                Times.Once
            );

            _bigQueryCommonLogsMock.Verify(
                x => x.BigQueryLogCriticalError(
                    _loggerMock.Object,
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Exception>()
                ),
                Times.Once
            );
        }

        [Test]
        public async Task UpdateRowAsync_Successful()
        {
            // Arrange
            var customerId = "123";
            var columnName = "Firstname";
            var updateValue = "John Doe";

            _clientMock.Setup(x => x.UpdateRowsAsync(customerId, columnName, updateValue, CancellationToken.None))
                .Returns(Task.FromResult(0));

            // Act
            await _bigQueryRepository.UpdateRowAsync(customerId, columnName, updateValue);

            // Assert
            _metricServiceMock.Verify(
                x => x.Increment(BigQueryMetricTags.UpdateBigQueryDataRequest, It.IsAny<List<string>>()),
                Times.Once);

            _metricServiceMock.Verify(
                x => x.Distribution(
                    BigQueryMetricTags.UpdateBigQueryDataRequest,
                    It.IsAny<double>(),
                    It.IsAny<List<string>>()
                ),
                Times.Once);
        }

        [Test]
        public void UpdateRowAsync_ExceptionHandling()
        {
            // Arrange
            var customerId = "123";
            var columnName = "Lastname";
            var updateValue = "Doe";

            _clientMock.Setup(x => x.UpdateRowsAsync(customerId, columnName, updateValue,CancellationToken.None))
                .ThrowsAsync(new GoogleApiException("Simulated exception"));

            // Act & Assert
            Assert.ThrowsAsync<GoogleApiException>(
                async () => await _bigQueryRepository.UpdateRowAsync(customerId, columnName, updateValue));

            _metricServiceMock.Verify(
                x => x.Increment(BigQueryMetricTags.UpdateBigQueryDataRequest, It.IsAny<List<string>>()),
                Times.Once);

            _metricServiceMock.Verify(
                x => x.Increment(
                    BigQueryMetricTags.UpdateBigQueryDataRequest,
                    new List<string> { MetricTags.StatusPermanentError, BigQueryMetricTags.UpdateBigQueryDataRequest }
                ),
                Times.Once);
        }
    }
}
