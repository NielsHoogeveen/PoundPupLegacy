namespace PoundPupLegacy.DomainModel;

public abstract record SenateTerm : CongressionalTerm
{
    private SenateTerm() { }

    public sealed record ToCreateForExistingSenator : SenateTerm, CongressionalTermToCreate
    {
        public required SenateTermDetails.ForExistingSenator SenateTermDetails { get; init; }
        public required CongressionalTermDetails.CongressionalTermDetailsForCreate CongressionalTermDetails { get; init; }
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
    }
    public sealed record ToCreateForNewSenator : SenateTerm, CongressionalTermToCreate
    {
        public required SenateTermDetails.ForNewSenator SenateTermDetails { get; init; }
        public required CongressionalTermDetails.CongressionalTermDetailsForCreate CongressionalTermDetails { get; init; }
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }

        public ToCreateForExistingSenator ResolveSenator(int senatorId)
        {
            return new ToCreateForExistingSenator {
                Identification = Identification,
                NodeDetails = NodeDetails,
                CongressionalTermDetails = CongressionalTermDetails,
                SenateTermDetails = new SenateTermDetails.ForExistingSenator {
                    DateTimeRange = SenateTermDetails.DateTimeRange,
                    SubdivisionId = SenateTermDetails.SubdivisionId,
                    SenatorId = senatorId
                }
            };
        }
    }
    public sealed record ToUpdate : SenateTerm, CongressionalTermToUpdate
    {
        public required SenateTermDetails.ForExistingSenator SenateTermDetails { get; init; }
        public required CongressionalTermDetails.CongressionalTermDetailsForUpdate CongressionalTermDetails { get; init; }
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
    }
}

public abstract record SenateTermDetails
{
    public required int SubdivisionId { get; init; }
    public required DateTimeRange DateTimeRange { get; init; }

    public sealed record ForNewSenator : SenateTermDetails
    {
    }
    public sealed record ForExistingSenator : SenateTermDetails
    {
        public required int SenatorId { get; init; }
    }
}
