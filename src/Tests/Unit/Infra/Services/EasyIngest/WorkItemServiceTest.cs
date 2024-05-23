using System.Net;
using Domain.Services;
using Infra.Clients;
using Infra.Extensions;
using Infra.Models.EasyIngest.WorkItems;
using Infra.Services.EasyIngest;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Refit;
using Unit.Faker;

namespace Unit.Infra.Services.EasyIngest;

public class WorkItemServiceTest
{
    private readonly IEasyIngestClient _easyIngestClientMock;
    private readonly ILogger<WorkItemsService> _loggerMock;
    private readonly IMetricService _metricServiceMock;
    private readonly IEasyIngestAuthenticationService _authenticationServiceMock;

    public WorkItemServiceTest()
    {
        _easyIngestClientMock = Substitute.For<IEasyIngestClient>();
        _loggerMock = Substitute.For<ILogger<WorkItemsService>>();
        _metricServiceMock = Substitute.For<IMetricService>();
        _authenticationServiceMock = Substitute.For<IEasyIngestAuthenticationService>();
    }

    [Fact]
    public async Task IngestWorkItemAsync_ShouldReturnStatusTrue_WhenPassAllRequiredInformation()
    {
        //Arrange
        var alertEntity = AlertEntityFaker.GenerateCompleteValidAlertEntity();

        var authenticationModel = EasyIngestAuthenticationResponseModelFaker.GenerateValidAuthenticationResponse();

        _authenticationServiceMock.ActimizeAuthenticationAsync().Returns(authenticationModel);

        var workItemService = new WorkItemsService(
            _easyIngestClientMock,
            _loggerMock,
            _metricServiceMock,
            _authenticationServiceMock
        );

        var response = WorkItemsResponseModelFaker.GenerateValidWithStatusTrueWorkItemsResponseModel();

        _easyIngestClientMock.WorkItemsAsync(
                authenticationModel.CsrfToken!,
                alertEntity.AlertType,
                alertEntity.AlertMappingName,
                alertEntity.MapAlertEntityToWorkItemRequest()
            )
            .Returns(
                new ApiResponse<WorkItemsResponseModel>(new HttpResponseMessage(HttpStatusCode.OK), response, null!)
            );

        //Act
        var result = await workItemService.IngestWorkItemAsync(alertEntity, CancellationToken.None);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IngestWorkItemAsync_ShouldReturnStatusFalse_WhenPassAllRequiredInformation()
    {
        //Arrange
        var alertEntity = AlertEntityFaker.GenerateCompleteValidAlertEntity();

        var authenticationModel = EasyIngestAuthenticationResponseModelFaker.GenerateValidAuthenticationResponse();

        _authenticationServiceMock.ActimizeAuthenticationAsync().Returns(authenticationModel);

        var workItemService = new WorkItemsService(
            _easyIngestClientMock,
            _loggerMock,
            _metricServiceMock,
            _authenticationServiceMock
        );

        var response = WorkItemsResponseModelFaker.GenerateValidWithStatusFalseWorkItemsResponseModel();

        _easyIngestClientMock.WorkItemsAsync(
                authenticationModel.CsrfToken!,
                alertEntity.AlertType,
                alertEntity.AlertMappingName,
                alertEntity.MapAlertEntityToWorkItemRequest()
            )
            .Returns(
                new ApiResponse<WorkItemsResponseModel>(new HttpResponseMessage(HttpStatusCode.OK), response, null!)
            );

        //Act
        var result = await workItemService.IngestWorkItemAsync(alertEntity, CancellationToken.None);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public async Task IngestWorkItemAsync_ShouldReturnStatusFalse_WhenEasyIngestAuthenticationStatusIsNotSuccess()
    {
        //Arrange
        var alertEntity = AlertEntityFaker.GenerateCompleteValidAlertEntity();

        var authenticationModel = EasyIngestAuthenticationResponseModelFaker.GenerateInvalidAuthenticationResponse();

        _authenticationServiceMock.ActimizeAuthenticationAsync().Returns(authenticationModel);

        var workItemService = new WorkItemsService(
            _easyIngestClientMock,
            _loggerMock,
            _metricServiceMock,
            _authenticationServiceMock
        );

        var response = WorkItemsResponseModelFaker.GenerateValidWithStatusFalseWorkItemsResponseModel();

        _easyIngestClientMock.WorkItemsAsync(
                authenticationModel.CsrfToken ?? string.Empty,
                alertEntity.AlertType,
                alertEntity.AlertMappingName,
                alertEntity.MapAlertEntityToWorkItemRequest()
            )
            .Returns(
                new ApiResponse<WorkItemsResponseModel>(new HttpResponseMessage(HttpStatusCode.OK), response, null!)
            );

        //Act
        var result = await workItemService.IngestWorkItemAsync(alertEntity, CancellationToken.None);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public async Task IngestWorkItemAsync_ShouldReturnStatusFalse_WhenEasyIngestReturnStatusCodeNotOk()
    {
        //Arrange
        var alertEntity = AlertEntityFaker.GenerateCompleteValidAlertEntity();

        var authenticationModel = EasyIngestAuthenticationResponseModelFaker.GenerateValidAuthenticationResponse();

        _authenticationServiceMock.ActimizeAuthenticationAsync().Returns(authenticationModel);

        var workItemService = new WorkItemsService(
            _easyIngestClientMock,
            _loggerMock,
            _metricServiceMock,
            _authenticationServiceMock
        );

        var response = WorkItemsResponseModelFaker.GenerateValidWithStatusFalseWorkItemsResponseModel();

        _easyIngestClientMock.WorkItemsAsync(
                authenticationModel.CsrfToken!,
                alertEntity.AlertType,
                alertEntity.AlertMappingName,
                alertEntity.MapAlertEntityToWorkItemRequest()
            )
            .Returns(
                new ApiResponse<WorkItemsResponseModel>(new HttpResponseMessage(HttpStatusCode.BadRequest), response,
                    null!)
            );

        //Act
        var result = await workItemService.IngestWorkItemAsync(alertEntity, CancellationToken.None);

        //Assert
        Assert.False(result);
    }
}
