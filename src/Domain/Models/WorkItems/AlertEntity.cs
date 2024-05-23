namespace Domain.Models.WorkItems;

public class AlertEntity
{
    public string AlertMappingName { get; set; } = default!;
    public string AlertType { get; set; } = default!;
    public string? BusinessUnitIdentifier { get; set; }
    public string? PartyKey { get; set; }

    #region Alert-level fields

    public string AlertNum { get; set; } = default!;
    public int? AlertScore { get; set; }
    public string AlertSubtype { get; set; } = default!;
    public decimal Amount { get; set; }
    public decimal? ConfirmedFraudAttempted { get; set; }
    public string? ConfirmedFraudCategory { get; set; }
    public decimal? ConfirmedFraudExposure { get; set; }
    public decimal? ConfirmedFraudGrossLoss { get; set; }
    public decimal? ConfirmedFraudNetLoss { get; set; }
    public decimal? ConfirmedFraudRecovery { get; set; }
    public string? DestinationAccount { get; set; }
    public DateTime IssueDetectionDate { get; set; }
    public string? Method { get; set; }
    public string? Notes { get; set; }
    public string PartyAccountNo { get; set; } = default!;
    public string? PartyEmail { get; set; }
    public string PartyFirstName { get; set; } = default!;
    public string? PartyLastName { get; set; }
    public string UserId { get; set; } = default!;

    #endregion

    #region Main Party Fields

    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AddressRelationType { get; set; }
    public string? ApartmentSuiteNumber { get; set; }
    public DateTime BirthIncorporationDate { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public int DocumentNumber { get; set; }
    public string? DocumentType { get; set; }
    public string? OpenDate { get; set; }
    public string? PartyName { get; set; }
    public string? PartyType { get; set; }
    public string? StateProvince { get; set; }

    #endregion

    #region Main Account Fields

    public string? AccountKey { get; set; }
    public string? AccountName { get; set; }
    public int AccountNumber { get; set; }
    public DateTime AccountOpenDate { get; set; }
    public string? AccountType { get; set; }
    public string? HoldingBranch { get; set; }
    public string? OpeningChannel { get; set; }
    public string? PopulationGroup { get; set; }
    public string? PrimaryPartyKey { get; set; }

    #endregion
}
