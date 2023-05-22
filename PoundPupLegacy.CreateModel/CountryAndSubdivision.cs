namespace PoundPupLegacy.CreateModel;
public interface ImmediatelyIdentifiableCountryAndSubdivision : CountryAndSubdivision, ImmediatelyIdentifiableTopLevelCountry, ImmediatelyIdentifiableISOCodedSubdivision
{
}
public interface EventuallyIdentifiableCountryAndSubdivision : CountryAndSubdivision, EventuallyIdentifiableTopLevelCountry, EventuallyIdentifiableISOCodedSubdivision
{
}
public interface CountryAndSubdivision: TopLevelCountry, ISOCodedSubdivision
{
}

public abstract record NewCountryAndSubdivisionBase: NewTopLevelCountryBase, EventuallyIdentifiableCountryAndSubdivision
{
    public required string ISO3166_2_Code { get; init; }
    public required int CountryId { get; init; }
    public required int SubdivisionTypeId { get; init; }

}

public abstract record ExistingCountryAndSubdivisionBase : ExistingTopLevelCountryBase, ImmediatelyIdentifiableCountryAndSubdivision
{
    public required string ISO3166_2_Code { get; init; }
    public required int CountryId { get; init; }
    public required int SubdivisionTypeId { get; init; }

}
