using Infra.Utils;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Unit.Infra.Utils;

public class BigQueryCommonLogsTest
{
    private Mock<ILogger<BigQueryCommonLogs>> _loggerMock;
    private BigQueryCommonLogs _bigQueryCommonLogs;

    public BigQueryCommonLogsTest(Mock<ILogger<BigQueryCommonLogs>> loggerMock, BigQueryCommonLogs bigQueryCommonLogs)
    {
        _loggerMock = loggerMock;
        _bigQueryCommonLogs = bigQueryCommonLogs;
    }

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<BigQueryCommonLogs>>();
        _bigQueryCommonLogs = new BigQueryCommonLogs();
    }

    [Test]
    public void BigQueryLogCriticalError_LogsError()
    {
        // Arrange
        const string expectedActimizeService = "YourActimizeService";
        const string expectedLogMessage = "YourLogMessage";
        var expectedException = new Exception("Simulated exception");

        // Act
        _bigQueryCommonLogs.BigQueryLogCriticalError(
            _loggerMock.Object,
            expectedActimizeService,
            expectedLogMessage,
            expectedException);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) =>
                    string.Equals(expectedActimizeService + ":" + expectedLogMessage, o.ToString())),
                expectedException,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ),
            Times.Once
        );
    }
}
