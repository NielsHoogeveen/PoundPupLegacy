namespace PoundPupLegacy.CreateModel;

public abstract record InterOrganizationalRelationType : EndoRelationType
{
    private InterOrganizationalRelationType() { }
    public required EndoRelationTypeDetails EndoRelationTypeDetails { get; init; }
    public sealed record ToCreate : InterOrganizationalRelationType, EndoRelationTypeToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record ToUpdate : InterOrganizationalRelationType, EndoRelationTypeToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}
