namespace PoundPupLegacy.CreateModel;

public abstract record UnitedStatesPoliticalPartyAffiliation: Nameable
{
    private UnitedStatesPoliticalPartyAffiliation() { }
    public required UnitedStatesPoliticalPartyAffliationDetails UnitedStatesPoliticalPartyAffliationDetails { get; init; }
    public sealed record ToCreate : UnitedStatesPoliticalPartyAffiliation, NameableToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record UnitedStatesPoliticalPartyAffiliationToUpdate : UnitedStatesPoliticalPartyAffiliation, NameableToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}

public sealed record UnitedStatesPoliticalPartyAffliationDetails
{
    public required int? UnitedStatesPoliticalPartyId { get; init; }
}