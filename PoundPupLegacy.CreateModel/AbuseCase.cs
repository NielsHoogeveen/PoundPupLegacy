namespace PoundPupLegacy.CreateModel;

public abstract record AbuseCase : Case
{
    private AbuseCase() { }
    public required AbuseCaseDetails AbuseCaseDetails { get; init; }

    public sealed record ToCreate : AbuseCase, CaseToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
        public required CaseDetails.CaseDetailsForCreate CaseDetails { get; init; }
        public required LocatableDetails.LocatableDetailsForCreate LocatableDetails { get; init; }
    }
    public sealed record ToUpdate : AbuseCase, CaseToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
        public required CaseDetails.CaseDetailsForUpdate CaseDetails { get; init; }
        public required LocatableDetails.LocatableDetailsForUpdate LocatableDetails { get; init; }
    }
}

public sealed record AbuseCaseDetails
{
    public required int ChildPlacementTypeId { get; init; }
    public required int? FamilySizeId { get; init; }
    public required bool? HomeschoolingInvolved { get; init; }
    public required bool? FundamentalFaithInvolved { get; init; }
    public required bool? DisabilitiesInvolved { get; init; }
    public required List<int> TypeOfAbuseIds { get; init; }
    public required List<int> TypeOfAbuserIds { get; init; }
}
