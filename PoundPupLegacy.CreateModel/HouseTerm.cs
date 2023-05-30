namespace PoundPupLegacy.CreateModel;

public abstract record HouseTerm : CongressionalTerm
{
    private HouseTerm() { }
    public required HouseTermDetails HouseTermDetails { get; init; }
    public abstract CongressionalTermDetails CongressionalTermDetails { get; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public sealed record ToCreate : HouseTerm, CongressionalTermToCreate
    {
        public override CongressionalTermDetails CongressionalTermDetails => CongressionalTermDetailsForCreate;
        public required CongressionalTermDetails.CongressionalTermDetailsForCreate CongressionalTermDetailsForCreate { get; init; }
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
    }
    public sealed record HouseTermToUpdate : HouseTerm, CongressionalTermToUpdate
    {
        public override CongressionalTermDetails CongressionalTermDetails => CongressionalTermDetailsForUpdate;
        public required CongressionalTermDetails.CongressionalTermDetailsForUpdate CongressionalTermDetailsForUpdate { get; init; }
        public required Identification.Certain IdentificationCertain { get; init; }
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public override Identification Identification => IdentificationCertain;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
    }
}

public sealed record HouseTermDetails
{
    public required int? RepresentativeId { get; set; }
    public required int SubdivisionId { get; init; }
    public required int? District { get; init; }
    public required DateTimeRange DateTimeRange { get; init; }

}
