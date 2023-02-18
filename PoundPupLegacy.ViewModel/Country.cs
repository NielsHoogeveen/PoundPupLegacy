namespace PoundPupLegacy.ViewModel;

public interface Country : PoliticalEntity
{
    public AdoptionImports AdoptionImports { get;  }

    public OrganizationTypeWithOrganizations[] OrganizationTypes { get; }

    public SubdivisionType[] SubdivisionTypes { get; }

    public string FlagPath { get; }
}
