namespace PoundPupLegacy.CreateModel;

public interface ImmediatelyIdentifiableCountry : Country, ImmediatelyIdentifiablePoliticalEntity
{
}

public interface EventuallyIdentifiableCountry: Country, EventuallyIdentifiablePoliticalEntity
{
}

public interface Country : PoliticalEntity
{
    int HagueStatusId { get; }
    string? ResidencyRequirements { get; }
    string? AgeRequirements { get; }
    string? MarriageRequirements { get; }
    string? IncomeRequirements { get; }
    string? HealthRequirements { get; }
    string? OtherRequirements { get; }
}

public abstract record NewCountryBase: NewPoliticalEntityBase, EventuallyIdentifiableCountry
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
public abstract record ExistingCountryBase : ExistingPoliticalEntityBase, ImmediatelyIdentifiableCountry
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
