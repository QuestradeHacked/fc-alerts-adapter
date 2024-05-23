namespace Infra.Models.EasyIngest.WorkItems;

public record AccountModel(
    string? AccountKey,
    string? AccountName,
    int AccountNumber,
    DateTime AccountOpenDate,
    string? AccountType,
    string? BusinessUnitIdentifier,
    string? HoldingBranch,
    string? OpeningChannel,
    string? PopulationGroup,
    string? PrimaryPartyKey
);
