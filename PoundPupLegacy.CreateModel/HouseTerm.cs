namespace PoundPupLegacy.CreateModel;

public abstract record HouseTerm : CongressionalTerm
{
    private HouseTerm() { }
    public required HouseTermDetails HouseTermDetails { get; init; }
    public abstract CongressionalTermDetails CongressionalTermDetails { get; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(Func<HouseTermToCreate, T> create, Func<HouseTermToUpdate, T> update);
    public abstract void Match(Action<HouseTermToCreate> create, Action<HouseTermToUpdate> update);

    public sealed record HouseTermToCreate : HouseTerm, CongressionalTermToCreate
    {
        public override CongressionalTermDetails CongressionalTermDetails => CongressionalTermDetailsForCreate;
        public required CongressionalTermDetails.CongressionalTermDetailsForCreate CongressionalTermDetailsForCreate { get; init; }
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
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
        public override CongressionalTermDetails CongressionalTermDetails => CongressionalTermDetailsForUpdate;
        public required CongressionalTermDetails.CongressionalTermDetailsForUpdate CongressionalTermDetailsForUpdate { get; init; }
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override Identification Identification => IdentificationForUpdate;
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
