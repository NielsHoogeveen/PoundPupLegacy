namespace PoundPupLegacy.ViewModel;

public record CountrySubdivisionType
{
    public string Name { get; set; }

    public SubdivisionListItem[] Subdivisions { get; set; }
}
