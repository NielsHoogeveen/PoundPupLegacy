namespace PoundPupLegacy.ViewModel.Models;

public interface Subdivision : PoliticalEntity
{

    public OrganizationTypeWithOrganizations[] OrganizationTypes { get; }

    public SubdivisionType[] SubdivisionTypes { get; }
}
