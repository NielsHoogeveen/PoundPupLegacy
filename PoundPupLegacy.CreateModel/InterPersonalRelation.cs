namespace PoundPupLegacy.CreateModel;

public abstract record InterPersonalRelation : Node
{
    private InterPersonalRelation() { }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public required InterPersonalRelationDetails InterPersonalRelationDetails { get; init; }
    public sealed record ToCreateForExistingParticipants : InterPersonalRelation, NodeToCreate
    {
        public required int PersonIdFrom { get; init; }
        public required int PersonIdTo { get; init; }
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
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
                NodeDetailsForCreate = NodeDetailsForCreate,
                IdentificationForCreate = IdentificationForCreate,
            };
        }
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
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
                NodeDetailsForCreate = NodeDetailsForCreate,
                IdentificationForCreate = IdentificationForCreate,
            };
        }
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
    }
    public sealed record InterPersonalRelationToUpdate : InterPersonalRelation, NodeToUpdate
    {
        public required int PersonIdFrom { get; init; }
        public required int PersonIdTo { get; init; }
        public override Identification Identification => IdentificationCertain;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required Identification.Certain IdentificationCertain { get; init; }
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
    }
}

public sealed record InterPersonalRelationDetails
{
    public required int InterPersonalRelationTypeId { get; init; }
    public required DateTimeRange? DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required string? Description { get; init; }
}
