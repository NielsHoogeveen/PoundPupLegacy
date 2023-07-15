namespace PoundPupLegacy.DomainModel;

public abstract record PartyPoliticalEntityRelationType : Nameable
{
    private PartyPoliticalEntityRelationType() { }
    public required PartyPoliticalEntityRelationTypeDetails PartyPoliticalEntityRelationTypeDetails { get; init; }
    public sealed record ToCreate : PartyPoliticalEntityRelationType, NameableToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record ToUpdate : PartyPoliticalEntityRelationType, NameableToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}

public sealed record PartyPoliticalEntityRelationTypeDetails
{
    public required bool HasConcreteSubtype { get; init; }
}
