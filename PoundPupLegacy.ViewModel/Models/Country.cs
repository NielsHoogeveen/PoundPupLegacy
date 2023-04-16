namespace PoundPupLegacy.ViewModel.Models;

public interface Country : PoliticalEntity
{
    AdoptionImports AdoptionImports { get; }

    OrganizationTypeWithOrganizations[] OrganizationTypes { get; }

    SubdivisionType[] SubdivisionTypes { get; }

    Image FlagImage { get; }

}
