namespace PoundPupLegacy.DomainModel;

public interface CountryToUpdate : Country, PoliticalEntityToUpdate
{
}

public interface CountryToCreate : Country, PoliticalEntityToCreate
{
}

public interface Country : PoliticalEntity
{
    CountryDetails CountryDetails { get; }
}


public sealed record CountryDetails
{
    public required string Name { get; init; }
    public required int HagueStatusId { get; init; }
    public required string? ResidencyRequirements { get; init; }
    public required string? AgeRequirements { get; init; }
    public required string? MarriageRequirements { get; init; }
    public required string? IncomeRequirements { get; init; }
    public required string? HealthRequirements { get; init; }
    public required string? OtherRequirements { get; init; }
}
