using Infra.Config;
using Infra.Config.PubSub;
using Infra.Models.Messages;
using Questrade.Library.PubSubClientHelper.Primitives;

namespace Adapter.Config;

public class AlertsAdapterConfiguration : BaseSubscriberConfiguration<AlertsAdapterMessage>
{
    public ActimizeConfiguration ActimizeConfiguration { get; } = new();

    public AlertsAdapterSubscriberConfiguration AlertsAdapterSubscriberConfiguration { get; } = new();

    public override Task HandleMessageLogAsync(ILogger logger, LogLevel logLevel, AlertsAdapterMessage message, string logMessage, Exception? error = null, CancellationToken cancellationToken = new())
    {
        return Task.CompletedTask;
    }

    internal void Validate()
    {
        ActimizeConfiguration.Validate();

        if (AlertsAdapterSubscriberConfiguration == null)
        {
            throw new InvalidOperationException(message: "AlertsAdapterSubscriberConfiguration is not valid.");
        }

        AlertsAdapterSubscriberConfiguration.Validate();
    }
}
