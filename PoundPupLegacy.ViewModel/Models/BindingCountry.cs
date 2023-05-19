namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(BindingCountry))]
public partial class BindingCountryJsonContext : JsonSerializerContext { }

public sealed record BindingCountry : TopLevelCountryBase
{

    private BasicLink[] boundCountries = Array.Empty<BasicLink>();
    public required BasicLink[] BoundCountries {
        get => boundCountries;
        init {
            if (value is not null) {
                boundCountries = value;
            }
        }
    }
}
