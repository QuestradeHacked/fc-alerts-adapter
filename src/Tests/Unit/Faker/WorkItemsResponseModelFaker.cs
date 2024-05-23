using Infra.Models.EasyIngest.WorkItems;

namespace Unit.Faker;

public static class WorkItemsResponseModelFaker
{
    public static WorkItemsResponseModel GenerateValidWithStatusTrueWorkItemsResponseModel()
    {
        return new WorkItemsResponseModel
        {
            Status = true
        };
    }

    public static WorkItemsResponseModel GenerateValidWithStatusFalseWorkItemsResponseModel()
    {
        return new WorkItemsResponseModel
        {
            Status = false
        };
    }
}
