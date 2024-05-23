namespace Infra.Config;

public class BigQueryConfig
{
    public string ProjectId { get; set; } = default!;
    public string DatasetName { get; set; } = default!;
    public string TableName { get; set; } = default!;
}
