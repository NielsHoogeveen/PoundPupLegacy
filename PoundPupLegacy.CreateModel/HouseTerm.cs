namespace PoundPupLegacy.CreateModel;

public abstract record HouseTerm : CongressionalTerm
{
    private HouseTerm() { }
    public required HouseTermDetails HouseTermDetails { get; init; }
    public required CongressionalTermDetails CongressionalTermDetails { get; init; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(Func<HouseTermToCreate, T> create, Func<HouseTermToUpdate, T> update);
    public abstract void Match(Action<HouseTermToCreate> create, Action<HouseTermToUpdate> update);

    public sealed record HouseTermToCreate : HouseTerm, CongressionalTermToCreate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override T Match<T>(Func<HouseTermToCreate, T> create, Func<HouseTermToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<HouseTermToCreate> create, Action<HouseTermToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record HouseTermToUpdate : HouseTerm, CongressionalTermToUpdate
    {
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public override T Match<T>(Func<HouseTermToCreate, T> create, Func<HouseTermToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<HouseTermToCreate> create, Action<HouseTermToUpdate> update)
        {
            update(this);
        }
    }
}

public sealed record HouseTermDetails
{
    public required int? RepresentativeId { get; set; }
    public required int SubdivisionId { get; init; }
    public required int? District { get; init; }
    public required DateTimeRange DateTimeRange { get; init; }

}
