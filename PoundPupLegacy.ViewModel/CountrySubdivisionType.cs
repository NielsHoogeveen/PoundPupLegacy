namespace PoundPupLegacy.ViewModel;

public record SubdivisionType
{
    public required string Name { get; init; }

    private SubdivisionListItem[] subdivisions = Array.Empty<SubdivisionListItem>();
    public required SubdivisionListItem[] Subdivisions {
        get => subdivisions;
        init {
            if (value is not null) {
                subdivisions = value;
            }
        }
    }
}
