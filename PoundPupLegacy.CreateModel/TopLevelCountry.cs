namespace PoundPupLegacy.CreateModel;

public interface ImmediatelyIdentifiableTopLevelCountry : TopLevelCountry, ImmediatelyIdentifiableCountry
{
}
public interface EventuallyIdentifiableTopLevelCountry: TopLevelCountry, EventuallyIdentifiableCountry 
{ 
}
public interface TopLevelCountry : Country
{
    string ISO3166_1_Code { get; }

    int SecondLevelRegionId { get; }
}

public record NewTopLevelCountryBase: NewCountryBase, EventuallyIdentifiableTopLevelCountry
{
    public required string ISO3166_1_Code { get; init; }

    public required int SecondLevelRegionId { get; init; }

}
public record ExistingTopLevelCountryBase : ExistingCountryBase, ImmediatelyIdentifiableTopLevelCountry
{
    public required string ISO3166_1_Code { get; init; }

    public required int SecondLevelRegionId { get; init; }

}