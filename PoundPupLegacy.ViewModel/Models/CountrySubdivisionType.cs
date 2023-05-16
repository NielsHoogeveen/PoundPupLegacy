namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(SubdivisionType))]
public partial class SubdivisionTypeJsonContext : JsonSerializerContext { }

public sealed record SubdivisionType
{
    public required string Name { get; init; }

    private SubdivisionListEntry[] subdivisions = Array.Empty<SubdivisionListEntry>();
    public required SubdivisionListEntry[] Subdivisions {
        get => subdivisions;
        init {
            if (value is not null) {
                subdivisions = value;
            }
        }
    }
}
