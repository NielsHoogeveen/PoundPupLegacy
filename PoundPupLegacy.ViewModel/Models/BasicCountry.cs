namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(BasicCountry))]
public partial class BasicCountryJsonContext : JsonSerializerContext { }

public sealed record BasicCountry : TopLevelCountryBase
{
}
