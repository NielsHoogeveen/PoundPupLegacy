namespace PoundPupLegacy.CreateModel;

public abstract record ChildTraffickingCase : Case
{
    private ChildTraffickingCase() { }
    public required ChildTraffickingCaseDetails ChildTraffickingCaseDetails { get; init; }
    public sealed record ToCreate : ChildTraffickingCase, CaseToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
        public required CaseDetails.CaseDetailsForCreate CaseDetails { get; init; }
        public required LocatableDetails.LocatableDetailsForCreate LocatableDetails { get; init; }
    }
    public sealed record ToUpdate : ChildTraffickingCase, CaseToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
        public required CaseDetails.CaseDetailsForUpdate CaseDetails { get; init; }
        public required LocatableDetails.LocatableDetailsForUpdate LocatableDetails { get; init; }
    }
}

public sealed record ChildTraffickingCaseDetails
{
    public required int? NumberOfChildrenInvolved { get; init; }
    public required int CountryIdFrom { get; init; }
}
