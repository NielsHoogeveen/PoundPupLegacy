namespace PoundPupLegacy.CreateModel;

public abstract record DeportationCase : Case
{
    private DeportationCase() { }
    public required DeportationCaseDetails DeportationCaseDetails { get; init; }
    public sealed record ToCreate : DeportationCase, CaseToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
        public required CaseDetails.CaseDetailsForCreate CaseDetails { get; init; }
        public required LocatableDetails.LocatableDetailsForCreate LocatableDetails { get; init; }
    }
    public sealed record ToUpdate : DeportationCase, CaseToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
        public required CaseDetails.CaseDetailsForUpdate CaseDetails { get; init; }
        public required LocatableDetails.LocatableDetailsForUpdate LocatableDetails { get; init; }
    }
}
public sealed record DeportationCaseDetails
{
    public required int? SubdivisionIdFrom { get; init; }
    public required int? CountryIdTo { get; init; }
}
