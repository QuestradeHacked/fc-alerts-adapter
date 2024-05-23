using Domain.Models.WorkItems;
using Infra.Models.EasyIngest.WorkItems;

namespace Infra.Extensions;

public static class WorkItemMapper
{
    public static WorkItemRequestModel MapAlertEntityToWorkItemRequest(this AlertEntity entity)
    {
        return new WorkItemRequestModel
        (
            new ItemModel(
                entity.MapToDataModel(),
                entity.MapToPartyModel(),
                entity.MapToMainAccountModel()
            )
        );
    }

    private static DataModel MapToDataModel(this AlertEntity entity)
    {
        return new DataModel(
            AlertNum: entity.AlertNum,
            AlertScore: entity.AlertScore,
            AlertSubtype: entity.AlertSubtype,
            Amount: entity.Amount,
            BusinessUnitIdentifier: entity.BusinessUnitIdentifier,
            ConfirmedFraudAttempted: entity.ConfirmedFraudAttempted,
            ConfirmedFraudCategory: entity.ConfirmedFraudCategory,
            ConfirmedFraudExposure: entity.ConfirmedFraudExposure,
            ConfirmedFraudGrossLoss: entity.ConfirmedFraudGrossLoss,
            ConfirmedFraudNetLoss: entity.ConfirmedFraudNetLoss,
            ConfirmedFraudRecovery: entity.ConfirmedFraudRecovery,
            DestinationAccount: entity.DestinationAccount,
            IssueDetectionDate: entity.IssueDetectionDate,
            Method: entity.Method,
            Notes: entity.Notes,
            PartyAccountNo: entity.PartyAccountNo,
            PartyEmail: entity.PartyEmail,
            PartyFirstName: entity.PartyFirstName,
            PartyKey: entity.PartyKey,
            PartyLastName: entity.PartyLastName,
            UserId: entity.UserId
        );
    }

    private static PartyModel MapToPartyModel(this AlertEntity entity)
    {
        return new PartyModel(
            AddressLine1: entity.AddressLine1,
            AddressLine2: entity.AddressLine2,
            AddressRelationType: entity.AddressRelationType,
            ApartmentSuiteNumber: entity.ApartmentSuiteNumber,
            BirthIncorporationDate: entity.BirthIncorporationDate,
            BusinessUnitIdentifier: entity.BusinessUnitIdentifier,
            City: entity.City,
            Country: entity.Country,
            DocumentNumber: entity.DocumentNumber,
            DocumentType: entity.DocumentType,
            OpenDate: entity.OpenDate,
            PartyKey: entity.PartyKey,
            PartyName: entity.PartyName,
            PartyType: entity.PartyType,
            StateProvince: entity.StateProvince
        );
    }

    private static AccountModel MapToMainAccountModel(this AlertEntity entity)
    {
        return new AccountModel(
            AccountKey: entity.AccountKey,
            AccountName: entity.AccountName,
            AccountNumber: entity.AccountNumber,
            AccountOpenDate: entity.AccountOpenDate,
            AccountType: entity.AccountType,
            BusinessUnitIdentifier: entity.BusinessUnitIdentifier,
            HoldingBranch: entity.HoldingBranch,
            OpeningChannel: entity.OpeningChannel,
            PopulationGroup: entity.PopulationGroup,
            PrimaryPartyKey: entity.PrimaryPartyKey
        );
    }
}
