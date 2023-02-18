namespace PoundPupLegacy.ViewModel;

public record SubdivisionType
{
    public string Name { get; set; }

    public SubdivisionListItem[] Subdivisions { get; set; }
}
