namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(CountrySubdivisionType))]
public partial class CountrySubdivisionTypeJsonContext : JsonSerializerContext { }

public sealed record CountrySubdivisionType
{
    public required string Name { get; init; }

    private CountrySubdivisionListEntry[] subdivisions = Array.Empty<CountrySubdivisionListEntry>();
    public required CountrySubdivisionListEntry[] Subdivisions {
        get => subdivisions;
        init {
            if (value is not null) {
                subdivisions = value;
            }
        }
    }
}
