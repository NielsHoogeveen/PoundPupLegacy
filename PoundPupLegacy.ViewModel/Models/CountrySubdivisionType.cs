namespace PoundPupLegacy.ViewModel.Models;

public record SubdivisionType
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
