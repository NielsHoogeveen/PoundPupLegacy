namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(BoundCountry))]
public partial class BoundCountryJsonContext : JsonSerializerContext { }

public sealed record BoundCountry : CountryBase, IsoCodedSubdivision
{
    public required string ISO3166_2_Code { get; init; }
    public required BasicLink BindingCountry { get; init; }

}
