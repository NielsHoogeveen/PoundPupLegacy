namespace PoundPupLegacy.CreateModel;

public abstract record SenateTerm : CongressionalTerm
{
    private SenateTerm() { }
    public required SenateTermDetails SenateTermDetails { get; init; }
    public required CongressionalTermDetails CongressionalTermDetails { get; init; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(Func<SenateTermToCreate, T> create, Func<SenateTermToUpdate, T> update);
    public abstract void Match(Action<SenateTermToCreate> create, Action<SenateTermToUpdate> update);

    public sealed record SenateTermToCreate : SenateTerm, CongressionalTermToCreate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override T Match<T>(Func<SenateTermToCreate, T> create, Func<SenateTermToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<SenateTermToCreate> create, Action<SenateTermToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record SenateTermToUpdate : SenateTerm, CongressionalTermToUpdate
    {
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public override T Match<T>(Func<SenateTermToCreate, T> create, Func<SenateTermToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<SenateTermToCreate> create, Action<SenateTermToUpdate> update)
        {
            update(this);
        }
    }
}

public sealed record SenateTermDetails
{
    public required int? SenatorId { get; set; }
    public required int SubdivisionId { get; init; }
    public required DateTimeRange DateTimeRange { get; init; }
}
