namespace PoundPupLegacy.ViewModel.Models;

public abstract record SubdivisionBase: NameableBase, Subdivision{
    public required OrganizationTypeWithOrganizations[] OrganizationTypes { get; init; }
    public required SubdivisionType[] SubdivisionTypes { get; init; }
    public required BasicLink Country { get; init; }
}

public interface Subdivision : PoliticalEntity
{

    OrganizationTypeWithOrganizations[] OrganizationTypes { get; }

    SubdivisionType[] SubdivisionTypes { get; }

    BasicLink Country {get;}
}
