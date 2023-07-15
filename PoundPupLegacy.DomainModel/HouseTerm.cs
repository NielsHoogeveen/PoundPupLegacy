namespace PoundPupLegacy.DomainModel;

public abstract record HouseTerm : CongressionalTerm
{
    private HouseTerm() { }

    public sealed record ToCreateForNewRepresenatative : HouseTerm, CongressionalTermToCreate
    {
        public required HouseTermDetails.ForNewRepresentative HouseTermDetails { get; init; }
        public required CongressionalTermDetails.CongressionalTermDetailsForCreate CongressionalTermDetails { get; init; }
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public ToCreateForExistingRepresentative Resolve(int representativeId)
        {
            return new ToCreateForExistingRepresentative {
                Identification = Identification,
                NodeDetails = NodeDetails,
                CongressionalTermDetails = CongressionalTermDetails,
                HouseTermDetails = new HouseTermDetails.ForExistingRepresentative {
                    DateTimeRange = HouseTermDetails.DateTimeRange,
                    SubdivisionId = HouseTermDetails.SubdivisionId,
                    District = HouseTermDetails.District,
                    RepresentativeId = representativeId
                }
            };
        }
    }

    public sealed record ToCreateForExistingRepresentative : HouseTerm, CongressionalTermToCreate
    {
        public required HouseTermDetails.ForExistingRepresentative HouseTermDetails { get; init; }
        public required CongressionalTermDetails.CongressionalTermDetailsForCreate CongressionalTermDetails { get; init; }
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
    }
    public sealed record HouseTermToUpdate : HouseTerm, CongressionalTermToUpdate
    {
        public required HouseTermDetails.ForExistingRepresentative HouseTermDetails { get; init; }
        public required CongressionalTermDetails.CongressionalTermDetailsForUpdate CongressionalTermDetails { get; init; }
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
    }
}

public abstract record HouseTermDetails
{
    public required int SubdivisionId { get; init; }
    public required int? District { get; init; }
    public required DateTimeRange DateTimeRange { get; init; }

    public sealed record ForExistingRepresentative : HouseTermDetails
    {
        public required int RepresentativeId { get; init; }
    }
    public sealed record ForNewRepresentative : HouseTermDetails
    {
    }
}
