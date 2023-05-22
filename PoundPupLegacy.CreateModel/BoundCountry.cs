namespace PoundPupLegacy.CreateModel;

public sealed record NewBoundCountry : NewCountryBase, EventuallyIdentifiableBoundCountry
{
    public required int BindingCountryId { get; init; }
    public required string ISO3166_2_Code { get; init; }
    public required int CountryId { get; init; }
    public required int SubdivisionTypeId { get; init; }
    
}
public sealed record ExistingBoundCountry : ExistingCountryBase, ImmediatelyIdentifiableBoundCountry
{
    public required int BindingCountryId { get; init; }
    public required string ISO3166_2_Code { get; init; }
    public required int CountryId { get; init; }
    public required int SubdivisionTypeId { get; init; }

}

public interface ImmediatelyIdentifiableBoundCountry : BoundCountry, ImmediatelyIdentifiableCountry, ImmediatelyIdentifiableISOCodedSubdivision
{

}
public interface EventuallyIdentifiableBoundCountry : BoundCountry, EventuallyIdentifiableCountry, EventuallyIdentifiableISOCodedSubdivision
{

}
public interface BoundCountry:  Country, ISOCodedSubdivision
{
    int BindingCountryId {get;}
}