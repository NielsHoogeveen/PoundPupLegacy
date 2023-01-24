namespace PoundPupLegacy.ViewModel;

public interface Country: PoliticalEntity
{
    public AdoptionImports AdoptionImports { get; set; }

    public OrganizationTypeWithOrganizations[] OrganizationTypes { get; set; }

    public SubdivisionListItem[] Subdivisions { get; set; }
}
