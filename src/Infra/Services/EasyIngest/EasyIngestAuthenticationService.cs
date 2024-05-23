using System.Net;
using Domain.Constants;
using Domain.Services;
using Infra.Clients;
using Infra.Config;
using Infra.Constants;
using Infra.Extensions;
using Infra.Models.EasyIngest.Login;
using Infra.Utils;
using Microsoft.Extensions.Logging;
using SerilogTimings;

namespace Infra.Services.EasyIngest;

public class EasyIngestAuthenticationService : IEasyIngestAuthenticationService
{
    private readonly ActimizeCommonLogs _actimizeCommonLogs;

    private readonly ActimizeConfiguration _actimizeConfiguration;

    private readonly IEasyIngestClient _easyIngestClient;

    private readonly EasyIngestLoginRequestModel _easyIngestLoginRequest;

    private readonly Action<ILogger, string?, Exception?> _logEasyIngestLoginFailed =
        LoggerMessage.Define<string?>(
            eventId: new EventId(1, nameof(EasyIngestAuthenticationService)),
            formatString: "Fail logging into the Easy Ingest: {StatusReturned}",
            logLevel: LogLevel.Warning
        );

    private readonly ILogger<EasyIngestAuthenticationService> _logger;

    private readonly IMetricService _metricService;

    private EasyIngestAuthenticationResponseModel _easyIngestAuthenticationResponse = default!;

    private DateTime _tokenExpiration;

    public EasyIngestAuthenticationService(
        ActimizeConfiguration actimizeConfiguration,
        ActimizeCommonLogs actimizeCommonLogs,
        IEasyIngestClient easyIngestClient,
        ILogger<EasyIngestAuthenticationService> logger,
        IMetricService metricService
    )
    {
        _actimizeConfiguration = actimizeConfiguration;
        _actimizeCommonLogs = actimizeCommonLogs;
        _easyIngestClient = easyIngestClient;
        _easyIngestLoginRequest = new EasyIngestLoginRequestModel
        {
            Password = _actimizeConfiguration.EasyIngestApiPassword,
            UserName = _actimizeConfiguration.EasyIngestApiUserName
        };
        _logger = logger;
        _metricService = metricService;
    }

    public async Task<EasyIngestAuthenticationResponseModel> ActimizeAuthenticationAsync()
    {
        if (!_actimizeConfiguration.Enable)
            return new EasyIngestAuthenticationResponseModel
            {
                AuthenticationStatus = EasyIngestAuthenticationStatus.Disabled
            };

        if (ShouldRefreshToken()) await RefreshTokenAsync();

        return _easyIngestAuthenticationResponse;
    }

    private async Task RefreshTokenAsync()
    {
        try
        {
            var timing = Operation.Begin("Timing for Easy Ingest API request - {class}.{method}",
                nameof(EasyIngestAuthenticationService), nameof(RefreshTokenAsync));

            var loginRequest = await _easyIngestClient.LoginAsync(_easyIngestLoginRequest);

            timing.Complete();

            _easyIngestAuthenticationResponse = loginRequest.GetAuthenticationData();

            if (_easyIngestAuthenticationResponse.ResultHttpStatusCode != HttpStatusCode.OK)
            {
                _logEasyIngestLoginFailed(_logger, _easyIngestAuthenticationResponse.ResultHttpStatusCode.ToString(),
                    null);

                _metricService.Distribution(
                    MetricNames.EasyIngestApiRequest,
                    timing.Elapsed.TotalMilliseconds,
                    new List<string>
                    {
                        MetricTags.StatusNoOk,
                        EasyIngestMetricTags.LoginEndpoint
                    }
                );

                return;
            }

            _tokenExpiration = DateTime.UtcNow.AddMinutes(15);

            _metricService.Distribution(
                MetricNames.EasyIngestApiRequest,
                timing.Elapsed.TotalSeconds,
                new List<string>
                {
                    MetricTags.StatusOK,
                    EasyIngestMetricTags.LoginEndpoint
                }
            );
        }
        catch (Exception error)
        {
            _actimizeCommonLogs.ActimizeLogCriticalError(
                _logger,
                nameof(EasyIngestAuthenticationService),
                "Error logging into the Easy Ingest",
                error
            );

            _metricService.Increment(
                MetricNames.EasyIngestApiRequest,
                new List<string>
                {
                    MetricTags.StatusPermanentError,
                    EasyIngestMetricTags.LoginEndpoint
                }
            );

            _easyIngestAuthenticationResponse = new EasyIngestAuthenticationResponseModel
            {
                AuthenticationStatus = EasyIngestAuthenticationStatus.Error
            };

            throw;
        }
    }

    private bool ShouldRefreshToken()
    {
        return _easyIngestAuthenticationResponse is not
                   { AuthenticationStatus: EasyIngestAuthenticationStatus.Success } ||
               (_tokenExpiration - DateTime.UtcNow).TotalMinutes < 1;
    }
}
