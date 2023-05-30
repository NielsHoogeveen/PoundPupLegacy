namespace PoundPupLegacy.CreateModel;

public interface TopLevelCountryToUpdate : TopLevelCountry, CountryToUpdate
{
}
public interface TopLevelCountryToCreate: TopLevelCountry, CountryToCreate 
{ 
}
public interface TopLevelCountry : Country
{
    TopLevelCountryDetails TopLevelCountryDetails { get; }
}

public record TopLevelCountryDetails
{
    public required string ISO3166_1_Code { get; init; }
    public required int SecondLevelRegionId { get; init; }
}