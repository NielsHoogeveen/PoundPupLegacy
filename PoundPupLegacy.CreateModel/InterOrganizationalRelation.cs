namespace PoundPupLegacy.CreateModel;

public abstract record InterOrganizationalRelation: Node
{
    private InterOrganizationalRelation() { }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public required InterOrganizationalRelationDetails InterOrganizationalRelationDetails { get; init; }
    public abstract T Match<T>(
        Func<InterOrganizationalRelationToCreateForExistingParticipants, T> create,
        Func<InterOrganizationalRelationToCreateForNewOrganizationFrom, T> createNewFrom,
        Func<InterOrganizationalRelationToCreateForNewOrganizationTo, T> createNewTo,
        Func<InterOrganizationalRelationToUpdate, T> update
     );
    public abstract void Match(
        Action<InterOrganizationalRelationToCreateForExistingParticipants> create,
        Action<InterOrganizationalRelationToCreateForNewOrganizationFrom> createNewFrom,
        Action<InterOrganizationalRelationToCreateForNewOrganizationTo> createNewTo,
        Action<InterOrganizationalRelationToUpdate> update
    );

    public sealed record InterOrganizationalRelationToCreateForExistingParticipants : InterOrganizationalRelation, NodeToCreate
    {
        public required int OrganizationIdFrom { get; init; }
        public required int OrganizationIdTo { get; init; }
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override T Match<T>(
            Func<InterOrganizationalRelationToCreateForExistingParticipants, T> create,
            Func<InterOrganizationalRelationToCreateForNewOrganizationFrom, T> createNewFrom,
            Func<InterOrganizationalRelationToCreateForNewOrganizationTo, T> createNewTo,
            Func<InterOrganizationalRelationToUpdate, T> update
         ){
            return create(this);
        }
        public override void Match(
            Action<InterOrganizationalRelationToCreateForExistingParticipants> create,
            Action<InterOrganizationalRelationToCreateForNewOrganizationFrom> createNewFrom,
            Action<InterOrganizationalRelationToCreateForNewOrganizationTo> createNewTo,
            Action<InterOrganizationalRelationToUpdate> update
        ){
            create(this);
        }
    }
    public sealed record InterOrganizationalRelationToCreateForNewOrganizationFrom : InterOrganizationalRelation, NodeToCreate
    {
        public required int OrganizationIdTo { get; init; }
        public InterOrganizationalRelationToCreateForExistingParticipants ResolveOrganizationFrom(int organizationIdFrom)
        {
            return new InterOrganizationalRelationToCreateForExistingParticipants {
                OrganizationIdFrom = organizationIdFrom,
                OrganizationIdTo = OrganizationIdTo,
                InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                NodeDetailsForCreate = NodeDetailsForCreate,
                NodeIdentificationForCreate = NodeIdentificationForCreate,
            };
        }
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override T Match<T>(
            Func<InterOrganizationalRelationToCreateForExistingParticipants, T> create,
            Func<InterOrganizationalRelationToCreateForNewOrganizationFrom, T> createNewFrom,
            Func<InterOrganizationalRelationToCreateForNewOrganizationTo, T> createNewTo,
            Func<InterOrganizationalRelationToUpdate, T> update
         ){
            return createNewFrom(this);
        }
        public override void Match(
            Action<InterOrganizationalRelationToCreateForExistingParticipants> create,
            Action<InterOrganizationalRelationToCreateForNewOrganizationFrom> createNewFrom,
            Action<InterOrganizationalRelationToCreateForNewOrganizationTo> createNewTo,
            Action<InterOrganizationalRelationToUpdate> update
        ){
            createNewFrom(this);
        }
    }

    public sealed record InterOrganizationalRelationToCreateForNewOrganizationTo : InterOrganizationalRelation, NodeToCreate
    {
        public required int OrganizationIdFrom { get; init; }
        public required int InterOrganizationalRelationTypeId { get; init; }
        public InterOrganizationalRelationToCreateForExistingParticipants ResolveOrganizationTo(int organizationIdTo)
        {
            return new InterOrganizationalRelationToCreateForExistingParticipants {
                OrganizationIdFrom = OrganizationIdFrom,
                OrganizationIdTo = organizationIdTo,
                InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                NodeDetailsForCreate = NodeDetailsForCreate,
                NodeIdentificationForCreate = NodeIdentificationForCreate,
            };
        }
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override T Match<T>(
            Func<InterOrganizationalRelationToCreateForExistingParticipants, T> create,
            Func<InterOrganizationalRelationToCreateForNewOrganizationFrom, T> createNewFrom,
            Func<InterOrganizationalRelationToCreateForNewOrganizationTo, T> createNewTo,
            Func<InterOrganizationalRelationToUpdate, T> update
         ){
            return createNewTo(this);
        }
        public override void Match(
            Action<InterOrganizationalRelationToCreateForExistingParticipants> create,
            Action<InterOrganizationalRelationToCreateForNewOrganizationFrom> createNewFrom,
            Action<InterOrganizationalRelationToCreateForNewOrganizationTo> createNewTo,
            Action<InterOrganizationalRelationToUpdate> update
        ){
            createNewTo(this);
        }
    }
    public sealed record InterOrganizationalRelationToUpdate : InterOrganizationalRelation, NodeToUpdate
    {
        public required int OrganizationIdFrom { get; init; }
        public required int OrganizationIdTo { get; init; }
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override T Match<T>(
            Func<InterOrganizationalRelationToCreateForExistingParticipants, T> create,
            Func<InterOrganizationalRelationToCreateForNewOrganizationFrom, T> createNewFrom,
            Func<InterOrganizationalRelationToCreateForNewOrganizationTo, T> createNewTo,
            Func<InterOrganizationalRelationToUpdate, T> update
         ){
            return update(this);
        }
        public override void Match(
            Action<InterOrganizationalRelationToCreateForExistingParticipants> create,
            Action<InterOrganizationalRelationToCreateForNewOrganizationFrom> createNewFrom,
            Action<InterOrganizationalRelationToCreateForNewOrganizationTo> createNewTo,
            Action<InterOrganizationalRelationToUpdate> update
        ){
            update(this);
        }
    }
}

public sealed record InterOrganizationalRelationDetails
{
    public required int InterOrganizationalRelationTypeId { get; init; }
    public required DateTimeRange DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required int? GeographicalEntityId { get; init; }
    public required string? Description { get; init; }
    public required decimal? MoneyInvolved { get; init; }
    public required int? NumberOfChildrenInvolved { get; init; }
}
