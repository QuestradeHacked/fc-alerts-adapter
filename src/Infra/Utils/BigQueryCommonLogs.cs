using Microsoft.Extensions.Logging;

namespace Infra.Utils;

 public class BigQueryCommonLogs
 {
     public readonly Action<ILogger, string?, string?, Exception?> BigQueryLogCriticalError =
         LoggerMessage.Define<string?, string?>(
             eventId: new EventId(2, nameof(BigQueryCommonLogs)),
             formatString: "{ActimizeService}:{LogMessage}",
             logLevel: LogLevel.Error
        );
 }
