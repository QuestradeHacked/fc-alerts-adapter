using System.Text.Json.Serialization;

namespace Infra.Models.EasyIngest.Login;

public class EasyIngestLoginRequestModel
{
    [JsonPropertyName("password")]
    public string Password { get; set; } = default!;

    [JsonPropertyName("username")]
    public string UserName { get; set; } = default!;
}
