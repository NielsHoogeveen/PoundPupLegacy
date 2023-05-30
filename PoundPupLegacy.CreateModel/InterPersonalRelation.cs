namespace PoundPupLegacy.CreateModel;

public abstract record InterPersonalRelation : Node
{
    private InterPersonalRelation() { }
    public required InterPersonalRelationDetails InterPersonalRelationDetails { get; init; }
    public sealed record ToCreateForExistingParticipants : InterPersonalRelation, NodeToCreate
    {
        public required int PersonIdFrom { get; init; }
        public required int PersonIdTo { get; init; }
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
    }
    public sealed record ToCreateForNewPersonFrom : InterPersonalRelation, NodeToCreate
    {
        public required int PersonIdTo { get; init; }
        public ToCreateForExistingParticipants ResolvePersonFrom(int PersonIdFrom)
        {
            return new ToCreateForExistingParticipants {
                PersonIdFrom = PersonIdFrom,
                PersonIdTo = PersonIdTo,
                InterPersonalRelationDetails = InterPersonalRelationDetails,
                NodeDetails = NodeDetails,
                Identification = Identification,
            };
        }
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
    }

    public sealed record ToCreateForNewPersonTo : InterPersonalRelation, NodeToCreate
    {
        public required int PersonIdFrom { get; init; }
        public required int InterPersonalRelationTypeId { get; init; }
        public ToCreateForExistingParticipants ResolvePersonTo(int PersonIdTo)
        {
            return new ToCreateForExistingParticipants {
                PersonIdFrom = PersonIdFrom,
                PersonIdTo = PersonIdTo,
                InterPersonalRelationDetails = InterPersonalRelationDetails,
                NodeDetails = NodeDetails,
                Identification = Identification,
            };
        }
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
    }
    public sealed record InterPersonalRelationToUpdate : InterPersonalRelation, NodeToUpdate
    {
        public required int PersonIdFrom { get; init; }
        public required int PersonIdTo { get; init; }
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
    }
}

public sealed record InterPersonalRelationDetails
{
    public required int InterPersonalRelationTypeId { get; init; }
    public required DateTimeRange? DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required string? Description { get; init; }
}
