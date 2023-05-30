namespace PoundPupLegacy.CreateModel;

public abstract record SenateTerm : CongressionalTerm
{
    private SenateTerm() { }
    public required SenateTermDetails SenateTermDetails { get; init; }
    public abstract CongressionalTermDetails CongressionalTermDetails { get; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(Func<SenateTermToCreate, T> create, Func<SenateTermToUpdate, T> update);
    public abstract void Match(Action<SenateTermToCreate> create, Action<SenateTermToUpdate> update);

    public sealed record SenateTermToCreate : SenateTerm, CongressionalTermToCreate
    {
        public override CongressionalTermDetails CongressionalTermDetails => CongressionalTermDetailsForCreate;
        public required CongressionalTermDetails.CongressionalTermDetailsForCreate CongressionalTermDetailsForCreate { get; init; }
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
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
        public override CongressionalTermDetails CongressionalTermDetails => CongressionalTermDetailsForUpdate;
        public required CongressionalTermDetails.CongressionalTermDetailsForUpdate CongressionalTermDetailsForUpdate { get; init; }
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override Identification Identification => IdentificationForUpdate;
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
