using System.Globalization;
using Domain.Models.WorkItems;

namespace Unit.Faker;

public static class AlertEntityFaker
{
    public static AlertEntity GenerateCompleteValidAlertEntity()
    {
        var faker = new Bogus.Faker();
        var alertEntity = new AlertEntity
        {
            Amount = faker.Finance.Amount(1, 100),
            City = faker.Address.City(),
            Country = faker.Address.Country(),
            Method = faker.Finance.TransactionType(),
            AddressLine1 = faker.Address.StreetAddress(),
            AddressLine2 = faker.Address.SecondaryAddress(),
            AlertNum = faker.Random.Word(),
            AlertScore = faker.Random.Int(0, 1000),
            AlertType = faker.Random.String(15, 15),
            DestinationAccount = faker.Random.Word(),
            OpenDate = faker.Date.Recent().ToString(CultureInfo.InvariantCulture),
            DocumentNumber = faker.Random.Int(1),
            DocumentType = faker.Random.String(2, 5),
            PartyEmail = faker.Person.Email,
            PartyKey = faker.Person.UserName,
            PartyName = faker.Person.FullName,
            PartyType = faker.Random.String(5, 10),
            StateProvince = faker.Address.State(),
            AddressRelationType = faker.Random.String(5, 10),
            AlertMappingName = faker.Random.String(15, 15),
            ApartmentSuiteNumber = faker.Address.SecondaryAddress(),
            BusinessUnitIdentifier = faker.Random.String(5, 15),
            ConfirmedFraudAttempted = faker.Finance.Amount(),
            ConfirmedFraudCategory = faker.Random.String(5, 15),
            ConfirmedFraudExposure = faker.Finance.Amount(),
            ConfirmedFraudRecovery = faker.Finance.Amount(),
            AccountKey = faker.Random.String(5, 15),
            AccountName = faker.Finance.AccountName(),
            AccountNumber = faker.Random.Int(1000, 99999),
            AccountType = faker.Random.String(10, 10),
            HoldingBranch = faker.Random.String(5, 10),
            OpeningChannel = faker.Random.String(5, 10),
            PopulationGroup = faker.Random.String(5, 10),
            PartyFirstName = faker.Person.FirstName,
            PartyLastName = faker.Person.LastName,
            ConfirmedFraudGrossLoss = faker.Finance.Amount(),
            ConfirmedFraudNetLoss = faker.Finance.Amount(),
            AccountOpenDate = faker.Date.Past(),
            PrimaryPartyKey = faker.Random.String(5, 10),
            BirthIncorporationDate = faker.Date.Past(10),
            Notes = faker.Lorem.Lines(3),
            AlertSubtype = faker.Random.Word(),
            IssueDetectionDate = faker.Date.Recent(),
            PartyAccountNo = faker.Random.String(1, 10),
            UserId = faker.Person.UserName
        };

        return alertEntity;
    }
}
