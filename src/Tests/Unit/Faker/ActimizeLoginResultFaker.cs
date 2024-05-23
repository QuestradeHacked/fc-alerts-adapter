using System.Net;

namespace Unit.Faker;

public class ActimizeLoginResultFaker
{
    public HttpResponseMessage GetHttpResponseMessageOk()
    {
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
        httpResponseMessage.Headers.Add("CSRFTOKEN", Guid.NewGuid().ToString());

        return httpResponseMessage;
    }

    public HttpResponseMessage GetHttpResponseMessageNoOk()
    {
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
        return httpResponseMessage;
    }
}
