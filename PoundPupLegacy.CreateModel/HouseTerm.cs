namespace PoundPupLegacy.CreateModel;

public abstract record HouseTerm : CongressionalTerm
{
    private HouseTerm() { }
    public required HouseTermDetails HouseTermDetails { get; init; }
    public sealed record ToCreate : HouseTerm, CongressionalTermToCreate
    {
        public required CongressionalTermDetails.CongressionalTermDetailsForCreate CongressionalTermDetails { get; init; }
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
    }
    public sealed record HouseTermToUpdate : HouseTerm, CongressionalTermToUpdate
    {
        public required CongressionalTermDetails.CongressionalTermDetailsForUpdate CongressionalTermDetails { get; init; }
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
    }
}

public sealed record HouseTermDetails
{
    public required int? RepresentativeId { get; set; }
    public required int SubdivisionId { get; init; }
    public required int? District { get; init; }
    public required DateTimeRange DateTimeRange { get; init; }

}
