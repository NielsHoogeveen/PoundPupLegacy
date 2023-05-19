namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(CountryAndSubdivision))]
public partial class CountryAndSubdivisionJsonContext : JsonSerializerContext { }

public sealed record CountryAndSubdivision : TopLevelCountryBase, IsoCodedSubdivision
{
    public required string ISO3166_2_Code { get; init; }

}
