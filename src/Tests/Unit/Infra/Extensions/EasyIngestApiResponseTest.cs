using Infra.Constants;
using Infra.Extensions;
using Refit;
using Unit.Faker;

namespace Unit.Infra.Extensions;

public class EasyIngestApiResponseTest
{
    private readonly ActimizeLoginResultFaker _actimizeLoginResultFaker = new();

    [Fact]
    public void GetAuthenticationData_ShouldReturnSuccess_WhenHttpStatusCodeIsOK()
    {
        // Arrange
        var actmizeLoginResultFaker = _actimizeLoginResultFaker.GetHttpResponseMessageOk();
        var apiResponse = new ApiResponse<IApiResponse>
        (
            actmizeLoginResultFaker,
            null,
            new(),
            null
        );

        //Act
        var result = EasyIngestApiResponse.GetAuthenticationData(apiResponse);

        // Assert
        Assert.Equal(EasyIngestAuthenticationStatus.Success,result.AuthenticationStatus);
    }

    [Fact]
    public void GetAuthenticationData_ShouldReturnFail_WhenHttpStatusCodeIsNotOK()
    {
        // Arrange
        var actmizeLoginResultFaker = _actimizeLoginResultFaker.GetHttpResponseMessageNoOk();
        var apiResponse = new ApiResponse<IApiResponse>
        (
            actmizeLoginResultFaker,
            null,
            new(),
            null
        );

        //Act
        var result = EasyIngestApiResponse.GetAuthenticationData(apiResponse);

        // Assert
        Assert.Equal(EasyIngestAuthenticationStatus.Fail,result.AuthenticationStatus);
    }
}
