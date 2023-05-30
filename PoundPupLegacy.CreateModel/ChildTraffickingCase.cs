namespace PoundPupLegacy.CreateModel;

public abstract record ChildTraffickingCase : Case
{
    private ChildTraffickingCase() { }
    public required ChildTraffickingCaseDetails ChildTraffickingCaseDetails { get; init; }
    public abstract CaseDetails CaseDetails { get; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public sealed record ToCreate : ChildTraffickingCase, CaseToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public override CaseDetails CaseDetails => CaseDetailsForCreate;
        public override LocatableDetails LocatableDetails => LocatableDetailsForCreate;
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public required CaseDetails.CaseDetailsForCreate CaseDetailsForCreate { get; init; }
        public required LocatableDetails.LocatableDetailsForCreate LocatableDetailsForCreate { get; init; }
    }
    public sealed record ToUpdate : ChildTraffickingCase, CaseToUpdate
    {
        public required Identification.Certain IdentificationCertain { get; init; }
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationCertain;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public override CaseDetails CaseDetails => CaseDetailsForUpdate;
        public override LocatableDetails LocatableDetails => LocatableDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public required CaseDetails.CaseDetailsForUpdate CaseDetailsForUpdate { get; init; }
        public required LocatableDetails.LocatableDetailsForUpdate LocatableDetailsForUpdate { get; init; }
    }
}

public sealed record ChildTraffickingCaseDetails
{
    public required int? NumberOfChildrenInvolved { get; init; }
    public required int CountryIdFrom { get; init; }
}
