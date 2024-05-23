using Microsoft.Extensions.Logging;

namespace Infra.Utils;

public class ActimizeCommonLogs
{
    private readonly ILogger<ActimizeCommonLogs> _logger;

    public readonly Action<ILogger, string?, string?, Exception?> ActimizeLogCriticalError =
        LoggerMessage.Define<string?, string?>(
            eventId: new EventId(2, nameof(ActimizeCommonLogs)),
            formatString: "{ActimizeService}:{LogMessage}",
            logLevel: LogLevel.Error
        );

    public ActimizeCommonLogs(ILogger<ActimizeCommonLogs> logger)
    {
        _logger = logger;
    }
}
