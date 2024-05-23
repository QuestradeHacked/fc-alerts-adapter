namespace Infra.Config;

public class ActimizeConfiguration
{
    public string EasyIngestApiPassword { get; set; } = default!;

    public string EasyIngestApiUserName { get; set; } = default!;

    public string EasyIngestBaseUrl { get; set; } = default!;

    public bool Enable { get; set; }

    public int Retry { get; set; } = 3;

    public double Timeout { get; set; } = 10000;

    public void Validate()
    {
        if (
            string.IsNullOrWhiteSpace(EasyIngestApiUserName) ||
            string.IsNullOrWhiteSpace(EasyIngestApiPassword) ||
            string.IsNullOrWhiteSpace(EasyIngestBaseUrl)
        )
            throw new InvalidOperationException(
                "The configuration options for the ActimizeConfiguration is not valid. Please check Google Secrets Manager values");
    }
}
