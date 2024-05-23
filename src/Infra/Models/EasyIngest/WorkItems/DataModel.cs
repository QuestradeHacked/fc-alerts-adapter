using System.Text.Json.Serialization;

namespace Infra.Models.EasyIngest.WorkItems;

public record DataModel
(
    string AlertNum,
    int? AlertScore,
    string AlertSubtype,
    decimal Amount,
    string? BusinessUnitIdentifier,
    [property: JsonPropertyName("ConfirmedFraud_Attempted")]
    decimal? ConfirmedFraudAttempted,
    [property: JsonPropertyName("ConfirmedFraud_Category")]
    string? ConfirmedFraudCategory,
    [property: JsonPropertyName("ConfirmedFraud_Exposure")]
    decimal? ConfirmedFraudExposure,
    [property: JsonPropertyName("ConfirmedFraud_GrossLoss")]
    decimal? ConfirmedFraudGrossLoss,
    [property: JsonPropertyName("ConfirmedFraud_NetLoss")]
    decimal? ConfirmedFraudNetLoss,
    [property: JsonPropertyName("ConfirmedFraud_Recovery")]
    decimal? ConfirmedFraudRecovery,
    string? DestinationAccount,
    DateTime IssueDetectionDate,
    string? Method,
    string? Notes,
    string PartyAccountNo,
    string? PartyEmail,
    string PartyFirstName,
    string? PartyKey,
    string? PartyLastName,
    [property: JsonPropertyName("UserID")] string UserId
);
