namespace PoundPupLegacy.CreateModel;

public abstract record InterPersonalRelation : Node
{
    private InterPersonalRelation() { }
    public required InterPersonalRelationDetails InterPersonalRelationDetails { get; init; }

    public abstract record ToCreate : InterPersonalRelation, NodeToCreate
    {
        private ToCreate() { }
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }

        public sealed record ForExistingParticipants : ToCreate
        {
            public required int PersonIdFrom { get; init; }
            public required int PersonIdTo { get; init; }
        }
        public sealed record ForNewPersonFrom : ToCreate
        {
            public required int PersonIdTo { get; init; }
            public ForExistingParticipants ResolvePersonFrom(int PersonIdFrom)
            {
                return new ForExistingParticipants {
                    PersonIdFrom = PersonIdFrom,
                    PersonIdTo = PersonIdTo,
                    InterPersonalRelationDetails = InterPersonalRelationDetails,
                    NodeDetails = NodeDetails,
                    Identification = Identification,
                };
            }
        }

        public sealed record ForNewPersonTo : ToCreate
        {
            public required int PersonIdFrom { get; init; }
            public ForExistingParticipants ResolvePersonTo(int PersonIdTo)
            {
                return new ForExistingParticipants {
                    PersonIdFrom = PersonIdFrom,
                    PersonIdTo = PersonIdTo,
                    InterPersonalRelationDetails = InterPersonalRelationDetails,
                    NodeDetails = NodeDetails,
                    Identification = Identification,
                };
            }
        }
    }
    public sealed record ToUpdate : InterPersonalRelation, NodeToUpdate
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
