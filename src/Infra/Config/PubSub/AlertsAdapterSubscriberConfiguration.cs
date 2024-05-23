using Infra.Models.Messages;
using Microsoft.Extensions.Logging;
using Questrade.Library.PubSubClientHelper.Primitives;

namespace Infra.Config.PubSub;

public class AlertsAdapterSubscriberConfiguration : BaseSubscriberConfiguration<AlertsAdapterMessage>
{
    public override Task HandleMessageLogAsync(ILogger logger, LogLevel logLevel, AlertsAdapterMessage message, string logMessage, Exception? error = null, CancellationToken cancellationToken = new())
    {
        return Task.CompletedTask;
    }

    public void Validate()
    {
        if (!Enable) return;

        if (string.IsNullOrWhiteSpace(ProjectId) || string.IsNullOrWhiteSpace(SubscriptionId))
        {
            throw new InvalidOperationException("The configuration options for the AlertsAdapterSubscriber is not valid");
        }

        if (UseEmulator && string.IsNullOrWhiteSpace(Endpoint))
        {
            throw new InvalidOperationException("The emulator configuration options for AlertsAdapterSubscriber is not valid");
        }
    }
}
