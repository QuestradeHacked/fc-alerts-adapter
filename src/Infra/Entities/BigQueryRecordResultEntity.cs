namespace Infra.Entities;

public class BigQueryRecordResultEntity
{
    public string AccountKey { get; set; } = default!;

    public string AccountName { get; set; } = default!;

    public long AccountNumber { get; set; }

    public DateTime AccountOpenDate { get; set; }

    public string AccountType { get; set; } = default!;

    public string AddressLine1 { get; set; } = default!;

    public string AddressLine2 { get; set; } = default!;

    public string AddressRelationType { get; set; } = default!;

    public string ApartmentSuiteNumber { get; set; } = default!;

    public string AlertMappingName { get; set; } = default!;

    public string AlertType { get; set; } = default!;

    public string AlertNum { get; set; } = default!;

    public long? AlertScore { get; set; }

    public string AlertSubtype { get; set; } = default!;

    public double Amount { get; set; }

    public DateTime BirthIncorporationDate { get; set; }

    public string BusinessUnitIdentifier { get; set; } = default!;

    public string City { get; set; } = default!;

    public double? ConfirmedFraudAttempted { get; set; }

    public string ConfirmedFraudCategory { get; set; } = default!;

    public double? ConfirmedFraudExposure { get; set; }

    public double? ConfirmedFraudGrossLoss { get; set; }

    public double? ConfirmedFraudNetLoss { get; set; }

    public double? ConfirmedFraudRecovery { get; set; }

    public string Country { get; set; } = default!;

    public string DestinationAccount { get; set; } = default!;

    public long DocumentNumber { get; set; }

    public string DocumentType { get; set; } = default!;

    public string HoldingBranch { get; set; } = default!;

    public DateTime IssueDetectionDate { get; set; }

    public string Method { get; set; } = default!;

    public string Notes { get; set; } = default!;

    public string OpenDate { get; set; } = default!;

    public string OpeningChannel { get; set; } = default!;

    public string PartyAccountNo { get; set; } = default!;

    public string PartyEmail { get; set; } = default!;

    public string PartyFirstName { get; set; } = default!;

    public string PartyLastName { get; set; } = default!;

    public string PartyKey { get; set; } = default!;

    public string PartyName { get; set; } = default!;

    public string PartyType { get; set; } = default!;

    public string PopulationGroup { get; set; } = default!;

    public string PrimaryPartyKey { get; set; } = default!;

    public DateTime? ProcessingDate { get; set; }

    public string StateProvince { get; set; } = default!;

    public string Status { get; set; } = default!;

    public string UserId { get; set; } = default!;
}
