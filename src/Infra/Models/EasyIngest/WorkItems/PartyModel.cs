namespace Infra.Models.EasyIngest.WorkItems;

public record PartyModel(
    string? AddressLine1,
    string? AddressLine2,
    string? AddressRelationType,
    string? ApartmentSuiteNumber,
    DateTime BirthIncorporationDate,
    string? BusinessUnitIdentifier,
    string? City,
    string? Country,
    int DocumentNumber,
    string? DocumentType,
    string? OpenDate,
    string? PartyKey,
    string? PartyName,
    string? PartyType,
    string? StateProvince
);
