using System.Net;
using Domain.Constants;
using Domain.Models.WorkItems;
using Domain.Services;
using Infra.Clients;
using Infra.Constants;
using Infra.Extensions;
using Infra.Models.EasyIngest.Login;
using Infra.Models.EasyIngest.WorkItems;
using Microsoft.Extensions.Logging;
using Refit;
using SerilogTimings;

namespace Infra.Services.EasyIngest;

public class WorkItemsService : IWorkItemsService
{
    public WorkItemsService(IEasyIngestClient easyIngestClient, ILogger<WorkItemsService> logger,
        IMetricService metricService, IEasyIngestAuthenticationService authenticationService)
    {
        _easyIngestClient = easyIngestClient;
        _logger = logger;
        _metricService = metricService;
        _authenticationService = authenticationService;
    }

    public async Task<bool> IngestWorkItemAsync(AlertEntity alertEntity, CancellationToken cancellationToken)
    {
        _logEasyIngestWorkItemDebug(_logger, nameof(IngestWorkItemAsync), alertEntity, null);

        var authenticationModel = await _authenticationService.ActimizeAuthenticationAsync();

        if (authenticationModel is not { AuthenticationStatus: EasyIngestAuthenticationStatus.Success })
        {
            _logEasyIngestWorkItemAuthenticationWarning(_logger, nameof(IEasyIngestAuthenticationService),
                authenticationModel, null);
            return false;
        }

        var timing = Operation.Begin("Timing for Easy Ingest API request - {class}.{method}", nameof(WorkItemsService),
            nameof(IngestWorkItemAsync));

        var response = await _easyIngestClient.WorkItemsAsync(
            authenticationModel.CsrfToken!,
            alertEntity.AlertType,
            alertEntity.AlertMappingName,
            alertEntity.MapAlertEntityToWorkItemRequest()
        );

        if (!response.IsSuccessStatusCode || response.Content == null)
        {
            _logEasyIngestWorkItemError(_logger, nameof(IngestWorkItemAsync), response.Content, response.StatusCode,
                response.Error);

            _metricService.Distribution(
                MetricNames.EasyIngestApiRequest,
                timing.Elapsed.TotalMilliseconds,
                new List<string>
                {
                    (int)response.StatusCode > 500 ? MetricTags.StatusPermanentError : MetricTags.StatusNoOk,
                    EasyIngestMetricTags.WorkItemEndpoint
                }
            );

            return false;
        }

        _logEasyIngestWorkItemInfo(_logger, nameof(IngestWorkItemAsync), response, null);
        _metricService.Distribution(
            MetricNames.EasyIngestApiRequest,
            timing.Elapsed.TotalMilliseconds,
            new List<string>
            {
                MetricTags.StatusOK,
                EasyIngestMetricTags.WorkItemEndpoint
            }
        );

        return response.Content.Status;
    }

    private readonly IEasyIngestAuthenticationService _authenticationService;
    private readonly IEasyIngestClient _easyIngestClient;

    private readonly Action<ILogger, string?, EasyIngestAuthenticationResponseModel?, Exception?>
        _logEasyIngestWorkItemAuthenticationWarning =
            LoggerMessage.Define<string?, EasyIngestAuthenticationResponseModel?>(
                eventId: new EventId(3, nameof(WorkItemsService)),
                formatString: "{ActimizeAuthenticationAsync} responded with response {@response}",
                logLevel: LogLevel.Warning
            );

    private readonly Action<ILogger, string?, AlertEntity?, Exception?> _logEasyIngestWorkItemDebug =
        LoggerMessage.Define<string?, AlertEntity?>(
            eventId: new EventId(1, nameof(WorkItemsService)),
            formatString: "{MethodName} start method for {@request}",
            logLevel: LogLevel.Debug
        );

    private readonly Action<ILogger, string?, WorkItemsResponseModel?, HttpStatusCode?, Exception?>
        _logEasyIngestWorkItemError =
            LoggerMessage.Define<string?, WorkItemsResponseModel?, HttpStatusCode?>(
                eventId: new EventId(2, nameof(WorkItemsService)),
                formatString: "{MethodName} responded with response {@response} and StatusCode {StatusCode}",
                logLevel: LogLevel.Error
            );

    private readonly Action<ILogger, string?, IApiResponse<WorkItemsResponseModel>?, Exception?>
        _logEasyIngestWorkItemInfo =
            LoggerMessage.Define<string?, IApiResponse<WorkItemsResponseModel>?>(
                eventId: new EventId(4, nameof(WorkItemsService)),
                formatString: "{MethodName} sent for Easy Ingest API with {@result}",
                logLevel: LogLevel.Information
            );

    private readonly ILogger<WorkItemsService> _logger;
    private readonly IMetricService _metricService;
}
