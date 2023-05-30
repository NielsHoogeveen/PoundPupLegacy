namespace PoundPupLegacy.CreateModel;

public abstract record SenateTerm : CongressionalTerm
{
    private SenateTerm() { }
    public required SenateTermDetails SenateTermDetails { get; init; }
    public abstract CongressionalTermDetails CongressionalTermDetails { get; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public sealed record ToCreate : SenateTerm, CongressionalTermToCreate
    {
        public override CongressionalTermDetails CongressionalTermDetails => CongressionalTermDetailsForCreate;
        public required CongressionalTermDetails.CongressionalTermDetailsForCreate CongressionalTermDetailsForCreate { get; init; }
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
    }
    public sealed record ToUpdate : SenateTerm, CongressionalTermToUpdate
    {
        public override CongressionalTermDetails CongressionalTermDetails => CongressionalTermDetailsForUpdate;
        public required CongressionalTermDetails.CongressionalTermDetailsForUpdate CongressionalTermDetailsForUpdate { get; init; }
        public required Identification.Certain IdentificationCertain { get; init; }
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public override Identification Identification => IdentificationCertain;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
    }
}

public sealed record SenateTermDetails
{
    public required int? SenatorId { get; set; }
    public required int SubdivisionId { get; init; }
    public required DateTimeRange DateTimeRange { get; init; }
}
