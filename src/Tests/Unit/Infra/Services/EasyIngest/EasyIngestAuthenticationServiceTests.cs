using System.Net;
using Domain.Services;
using Infra.Clients;
using Infra.Constants;
using Infra.Models.EasyIngest.Login;
using Infra.Services.EasyIngest;
using Infra.Utils;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Refit;
using Unit.Faker;

namespace Unit.Infra.Services.EasyIngest;

public class EasyIngestAuthenticationServiceTests
{
    private readonly ActimizeCommonLogs _actimizeCommonLogs = new(Substitute.For<ILogger<ActimizeCommonLogs>>());
    private readonly ActimizeConfigurationFaker _actimizeConfigurationFaker = new();
    private readonly IEasyIngestClient _easyIngestClient = Substitute.For<IEasyIngestClient>();
    private readonly ILogger<EasyIngestAuthenticationService> _logger = Substitute.For<ILogger<EasyIngestAuthenticationService>>();
    private readonly IMetricService _metricService = Substitute.For<IMetricService>();

    [Fact]
    public async Task ActimizeAuthenticationAsync_ShouldReturnDisable_WhenTheConfigurationIsNotEnabled()
    {
        // Arrange
        var actimizeConfiguration = _actimizeConfigurationFaker.GenerateValidConfiguration();
        actimizeConfiguration.Enable = false;

        var easyIngestAuthentication = new EasyIngestAuthenticationService
        (
            actimizeConfiguration,
            _actimizeCommonLogs,
            _easyIngestClient,
            _logger,
            _metricService
        );

        //Act
        var responseData = await easyIngestAuthentication.ActimizeAuthenticationAsync();

        //Assert
        Assert.Equal(EasyIngestAuthenticationStatus.Disabled, responseData.AuthenticationStatus);
    }

    [Fact]
    public async Task ActimizeAuthenticationAsync_ShouldReturnToken_WhenTokenIsNull()
    {
        // Arrange
        var actimizeConfiguration = _actimizeConfigurationFaker.GenerateValidConfiguration();

        var easyIngestAuthentication = new EasyIngestAuthenticationService
        (
            actimizeConfiguration,
            _actimizeCommonLogs,
            _easyIngestClient,
            _logger,
            _metricService
        );

        var actmizeLoginResultFaker = new ActimizeLoginResultFaker().GetHttpResponseMessageOk();

        _easyIngestClient.LoginAsync(Arg.Any<EasyIngestLoginRequestModel>())
            .Returns(new ApiResponse<IApiResponse>(
                actmizeLoginResultFaker,
                null,
                new(),
                null
            ));

        //Act
        var responseData = await easyIngestAuthentication.ActimizeAuthenticationAsync();

        //Assert
        Assert.Equal(responseData.CsrfToken, actmizeLoginResultFaker.Headers.GetValues("CSRFTOKEN").FirstOrDefault());
    }

    [Fact]
    public async Task ActimizeAuthenticationAsync_ShouldReturnSameToken_WhenLastRequisitionFail()
    {
        // Arrange
        var actimizeConfiguration = _actimizeConfigurationFaker.GenerateValidConfiguration();

        var easyIngestAuthentication = new EasyIngestAuthenticationService
        (
            actimizeConfiguration,
            _actimizeCommonLogs,
            _easyIngestClient,
            _logger,
            _metricService
        );

        var actmizeLoginResultFaker = new ActimizeLoginResultFaker().GetHttpResponseMessageOk();

        _easyIngestClient.LoginAsync(Arg.Any<EasyIngestLoginRequestModel>())
            .Returns(new ApiResponse<IApiResponse>(
                new HttpResponseMessage(HttpStatusCode.Unauthorized),
                null,
                new(),
                null
            ));

        //Act
        var responseData = await easyIngestAuthentication.ActimizeAuthenticationAsync();

        _easyIngestClient.LoginAsync(Arg.Any<EasyIngestLoginRequestModel>())
           .Returns(new ApiResponse<IApiResponse>(
               actmizeLoginResultFaker,
               null,
               new(),
               null
           ));

        var refreshResponseData = await easyIngestAuthentication.ActimizeAuthenticationAsync();

        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, responseData.ResultHttpStatusCode);
        Assert.Equal(refreshResponseData.CsrfToken, actmizeLoginResultFaker.Headers.GetValues("CSRFTOKEN").FirstOrDefault());
    }

    [Fact]
    public async Task ActimizeAuthenticationAsync_ShouldReturnSameToken_WhenTokenIsNotExpired()
    {
        // Arrange
        var actimizeConfiguration = _actimizeConfigurationFaker.GenerateValidConfiguration();

        var easyIngestAuthentication = new EasyIngestAuthenticationService
        (
            actimizeConfiguration,
            _actimizeCommonLogs,
            _easyIngestClient,
            _logger,
            _metricService
        );

        _easyIngestClient.LoginAsync(Arg.Any<EasyIngestLoginRequestModel>())
            .Returns(new ApiResponse<IApiResponse>(
                new ActimizeLoginResultFaker().GetHttpResponseMessageOk(),
                null,
                new(),
                null
            ));

        //Act
        var responseData = await easyIngestAuthentication.ActimizeAuthenticationAsync();
        var refreshResponseData = await easyIngestAuthentication.ActimizeAuthenticationAsync();

        //Assert
        Assert.Equal(responseData.CsrfToken, refreshResponseData.CsrfToken);
    }

    [Fact]
    public async Task ActimizeAuthenticationAsync_ShouldReturnError_WhenExceptionOccurs()
    {
        // Arrange
        var actimizeConfiguration = _actimizeConfigurationFaker.GenerateValidConfiguration();

        var easyIngestAuthentication = new EasyIngestAuthenticationService
        (
            actimizeConfiguration,
            _actimizeCommonLogs,
            _easyIngestClient,
            _logger,
            _metricService
        );

        _easyIngestClient.LoginAsync(Arg.Any<EasyIngestLoginRequestModel>())
           .ThrowsAsync(new Exception("Test Exception"));

        //Assert
        await Assert.ThrowsAsync<Exception>(async () => await easyIngestAuthentication.ActimizeAuthenticationAsync());
    }
}

