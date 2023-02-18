namespace PoundPupLegacy.ViewModel;

public interface Subdivision : PoliticalEntity
{

    public OrganizationTypeWithOrganizations[] OrganizationTypes { get; }

    public SubdivisionType[] SubdivisionTypes { get; }
}
