namespace PoundPupLegacy.CreateModel;

public abstract record InterCountryRelation : Node
{
    private InterCountryRelation() { }
    public required InterCountryRelationDetails InterCountryRelationDetails { get; init; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(Func<InterCountryRelationToCreate, T> create, Func<InterCountryRelationToUpdate, T> update);
    public abstract void Match(Action<InterCountryRelationToCreate> create, Action<InterCountryRelationToUpdate> update);

    public sealed record InterCountryRelationToCreate : InterCountryRelation, NodeToCreate
    {
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;

        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override T Match<T>(Func<InterCountryRelationToCreate, T> create, Func<InterCountryRelationToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<InterCountryRelationToCreate> create, Action<InterCountryRelationToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record InterCountryRelationToUpdate : InterCountryRelation, NodeToUpdate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;

        public override NodeDetails NodeDetails => NodeDetailsForUpdate;

        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override T Match<T>(Func<InterCountryRelationToCreate, T> create, Func<InterCountryRelationToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<InterCountryRelationToCreate> create, Action<InterCountryRelationToUpdate> update)
        {
            update(this);
        }
    }
}

public sealed record InterCountryRelationDetails
{
    public required int InterCountryRelationTypeId { get; init; }
    public required int CountryIdFrom { get; init; }
    public required int CountryIdTo { get; init; }
    public required DateTimeRange? DateTimeRange { get; init; }
    public required int? NumberOfChildrenInvolved { get; init; }
    public required decimal? MoneyInvolved { get; init; }
    public required int? DocumentIdProof { get; init; }

}