namespace Infra.Models.EasyIngest.WorkItems;

public record ItemModel
(
    DataModel Data,
    PartyModel Party,
    AccountModel MainAccount
);
