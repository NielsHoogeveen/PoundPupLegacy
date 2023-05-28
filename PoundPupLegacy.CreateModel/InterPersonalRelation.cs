namespace PoundPupLegacy.CreateModel;

public abstract record InterPersonalRelation : Node
{
    private InterPersonalRelation() { }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public required InterPersonalRelationDetails InterPersonalRelationDetails { get; init; }
    public abstract T Match<T>(
        Func<InterPersonalRelationToCreateForExistingParticipants, T> create,
        Func<InterPersonalRelationToCreateForNewPersonFrom, T> createNewFrom,
        Func<InterPersonalRelationToCreateForNewPersonTo, T> createNewTo,
        Func<InterPersonalRelationToUpdate, T> update
     );
    public abstract void Match(
        Action<InterPersonalRelationToCreateForExistingParticipants> create,
        Action<InterPersonalRelationToCreateForNewPersonFrom> createNewFrom,
        Action<InterPersonalRelationToCreateForNewPersonTo> createNewTo,
        Action<InterPersonalRelationToUpdate> update
    );

    public sealed record InterPersonalRelationToCreateForExistingParticipants : InterPersonalRelation, NodeToCreate
    {
        public required int PersonIdFrom { get; init; }
        public required int PersonIdTo { get; init; }
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override T Match<T>(
            Func<InterPersonalRelationToCreateForExistingParticipants, T> create,
            Func<InterPersonalRelationToCreateForNewPersonFrom, T> createNewFrom,
            Func<InterPersonalRelationToCreateForNewPersonTo, T> createNewTo,
            Func<InterPersonalRelationToUpdate, T> update
         )
        {
            return create(this);
        }
        public override void Match(
            Action<InterPersonalRelationToCreateForExistingParticipants> create,
            Action<InterPersonalRelationToCreateForNewPersonFrom> createNewFrom,
            Action<InterPersonalRelationToCreateForNewPersonTo> createNewTo,
            Action<InterPersonalRelationToUpdate> update
        )
        {
            create(this);
        }
    }
    public sealed record InterPersonalRelationToCreateForNewPersonFrom : InterPersonalRelation, NodeToCreate
    {
        public required int PersonIdTo { get; init; }
        public InterPersonalRelationToCreateForExistingParticipants ResolvePersonFrom(int PersonIdFrom)
        {
            return new InterPersonalRelationToCreateForExistingParticipants {
                PersonIdFrom = PersonIdFrom,
                PersonIdTo = PersonIdTo,
                InterPersonalRelationDetails = InterPersonalRelationDetails,
                NodeDetailsForCreate = NodeDetailsForCreate,
                NodeIdentificationForCreate = NodeIdentificationForCreate,
            };
        }
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override T Match<T>(
            Func<InterPersonalRelationToCreateForExistingParticipants, T> create,
            Func<InterPersonalRelationToCreateForNewPersonFrom, T> createNewFrom,
            Func<InterPersonalRelationToCreateForNewPersonTo, T> createNewTo,
            Func<InterPersonalRelationToUpdate, T> update
         )
        {
            return createNewFrom(this);
        }
        public override void Match(
            Action<InterPersonalRelationToCreateForExistingParticipants> create,
            Action<InterPersonalRelationToCreateForNewPersonFrom> createNewFrom,
            Action<InterPersonalRelationToCreateForNewPersonTo> createNewTo,
            Action<InterPersonalRelationToUpdate> update
        )
        {
            createNewFrom(this);
        }
    }

    public sealed record InterPersonalRelationToCreateForNewPersonTo : InterPersonalRelation, NodeToCreate
    {
        public required int PersonIdFrom { get; init; }
        public required int InterPersonalRelationTypeId { get; init; }
        public InterPersonalRelationToCreateForExistingParticipants ResolvePersonTo(int PersonIdTo)
        {
            return new InterPersonalRelationToCreateForExistingParticipants {
                PersonIdFrom = PersonIdFrom,
                PersonIdTo = PersonIdTo,
                InterPersonalRelationDetails = InterPersonalRelationDetails,
                NodeDetailsForCreate = NodeDetailsForCreate,
                NodeIdentificationForCreate = NodeIdentificationForCreate,
            };
        }
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override T Match<T>(
            Func<InterPersonalRelationToCreateForExistingParticipants, T> create,
            Func<InterPersonalRelationToCreateForNewPersonFrom, T> createNewFrom,
            Func<InterPersonalRelationToCreateForNewPersonTo, T> createNewTo,
            Func<InterPersonalRelationToUpdate, T> update
         )
        {
            return createNewTo(this);
        }
        public override void Match(
            Action<InterPersonalRelationToCreateForExistingParticipants> create,
            Action<InterPersonalRelationToCreateForNewPersonFrom> createNewFrom,
            Action<InterPersonalRelationToCreateForNewPersonTo> createNewTo,
            Action<InterPersonalRelationToUpdate> update
        )
        {
            createNewTo(this);
        }
    }
    public sealed record InterPersonalRelationToUpdate : InterPersonalRelation, NodeToUpdate
    {
        public required int PersonIdFrom { get; init; }
        public required int PersonIdTo { get; init; }
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override T Match<T>(
            Func<InterPersonalRelationToCreateForExistingParticipants, T> create,
            Func<InterPersonalRelationToCreateForNewPersonFrom, T> createNewFrom,
            Func<InterPersonalRelationToCreateForNewPersonTo, T> createNewTo,
            Func<InterPersonalRelationToUpdate, T> update
         )
        {
            return update(this);
        }
        public override void Match(
            Action<InterPersonalRelationToCreateForExistingParticipants> create,
            Action<InterPersonalRelationToCreateForNewPersonFrom> createNewFrom,
            Action<InterPersonalRelationToCreateForNewPersonTo> createNewTo,
            Action<InterPersonalRelationToUpdate> update
        )
        {
            update(this);
        }
    }
}

public sealed record InterPersonalRelationDetails
{
    public required int InterPersonalRelationTypeId { get; init; }
    public required DateTimeRange? DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required string? Description { get; init; }
}
