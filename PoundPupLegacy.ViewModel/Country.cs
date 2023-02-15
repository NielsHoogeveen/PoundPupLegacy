namespace PoundPupLegacy.ViewModel;

public interface Country : PoliticalEntity
{
    public AdoptionImports AdoptionImports { get;  }

    public OrganizationTypeWithOrganizations[] OrganizationTypes { get; }

    public CountrySubdivisionType[] SubdivisionTypes { get; }
}
