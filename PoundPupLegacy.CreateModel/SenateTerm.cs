namespace PoundPupLegacy.CreateModel;

public abstract record SenateTerm : CongressionalTerm
{
    private SenateTerm() { }
    public required SenateTermDetails SenateTermDetails { get; init; }
    public sealed record ToCreate : SenateTerm, CongressionalTermToCreate
    {
        public required CongressionalTermDetails.CongressionalTermDetailsForCreate CongressionalTermDetails { get; init; }
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
    }
    public sealed record ToUpdate : SenateTerm, CongressionalTermToUpdate
    {
        public required CongressionalTermDetails.CongressionalTermDetailsForUpdate CongressionalTermDetails { get; init; }
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
    }
}

public sealed record SenateTermDetails
{
    public required int? SenatorId { get; set; }
    public required int SubdivisionId { get; init; }
    public required DateTimeRange DateTimeRange { get; init; }
}
