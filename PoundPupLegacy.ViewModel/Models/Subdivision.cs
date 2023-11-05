namespace PoundPupLegacy.ViewModel.Models;

public abstract record SubdivisionBase: NameableBase, Subdivision{
    public required OrganizationTypeWithOrganizations[] OrganizationTypes { get; init; }
    public required CountrySubdivisionType[] SubdivisionTypes { get; init; }
    public required BasicLink Country { get; init; }
}

public interface Subdivision : PoliticalEntity
{

    OrganizationTypeWithOrganizations[] OrganizationTypes { get; }

    CountrySubdivisionType[] SubdivisionTypes { get; }

    BasicLink Country {get;}
}
