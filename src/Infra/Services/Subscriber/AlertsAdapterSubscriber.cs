using Domain.Constants;
using Domain.Services;
using Infra.Config.PubSub;
using Infra.Models.Messages;
using Microsoft.Extensions.Logging;
using Questrade.Library.PubSubClientHelper.Subscriber;

namespace Infra.Services.Subscriber;

public class AlertsAdapterSubscriber : PubsubSubscriberBackgroundService<AlertsAdapterSubscriberConfiguration, AlertsAdapterMessage>
{
    private readonly IMetricService _metricService;

    public AlertsAdapterSubscriber(
        AlertsAdapterSubscriberConfiguration subscriberConfiguration,
        ILoggerFactory loggerFactory,
        IMetricService metricService,
        IServiceProvider serviceProvider)
        : base(loggerFactory, subscriberConfiguration, serviceProvider)
    {
        _metricService = metricService;
    }

    protected override Task<bool> HandleReceivedMessageAsync(AlertsAdapterMessage message, CancellationToken cancellationToken)
    {
        _logDefineScope(Logger, nameof(AlertsAdapterSubscriber), nameof(HandleReceivedMessageAsync));
        _logMessageReceivedInformation(Logger, message, null);

        _metricService.Increment(MetricNames.AlertsAdapterMessageCount, new List<string> { MetricTags.StatusSuccess });

        if (message.RowCount is null || message.TimeCreated is null)
        {
            _logMissingRequiredInformationWarning(Logger, null);

            return Task.FromResult(true);
        }

        // FC-660 - BigQuery call implementation here

        // TODO: Replace this log with the real one
        _temporarySuccessLogForTests(Logger, null);

        _metricService.Increment(MetricNames.AlertsAdapterHandleMessageCount,
            new List<string> { MetricTags.StatusSuccess });

        return Task.FromResult(true);
    }

    private readonly Func<ILogger, string?, string?, IDisposable> _logDefineScope =
        LoggerMessage.DefineScope<string?, string?>(
            formatString: "{AlertsAdapterSubscriberName}:{HandleReceivedMessageAsyncName}"
        );

    private readonly Action<ILogger, AlertsAdapterMessage, Exception?> _logMessageReceivedInformation =
        LoggerMessage.Define<AlertsAdapterMessage>(
            eventId: new EventId(1, nameof(AlertsAdapterSubscriber)),
            formatString: "Message processed: {AlertsAdapterMessage}",
            logLevel: LogLevel.Information
        );

    private readonly Action<ILogger, Exception?> _logMissingRequiredInformationWarning =
        LoggerMessage.Define(
            eventId: new EventId(2, nameof(AlertsAdapterSubscriber)),
            formatString: "A message with RowCount or TimeCreated null was received",
            logLevel: LogLevel.Warning
        );

    private readonly Action<ILogger, Exception?> _temporarySuccessLogForTests =
        LoggerMessage.Define(
            eventId: new EventId(3, nameof(AlertsAdapterSubscriber)),
            formatString: "[temp] Message processed",
            logLevel: LogLevel.Information
        );
}
