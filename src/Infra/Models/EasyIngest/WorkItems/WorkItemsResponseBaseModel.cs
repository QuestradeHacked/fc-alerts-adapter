using System.Diagnostics.CodeAnalysis;

namespace Infra.Models.EasyIngest.WorkItems;

[ExcludeFromCodeCoverage]
public abstract class WorkItemsResponseBaseModel
{
    public virtual IEnumerable<string>? ErrorMessages { get; set; }
}
