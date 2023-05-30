namespace PoundPupLegacy.CreateModel;

public abstract record CaseParties
{
    public required string? Organizations { get; init; }
    public required string? Persons { get; init; }
    public sealed record ToCreate : CaseParties, PossiblyIdentifiable
    {
        public required List<int> OrganizationIds { get; init; }
        public required List<int> PersonIds { get; init; }
        public required Identification.Possible Identification { get; init; }
    }
    public sealed record ToUpdate : CaseParties, CertainlyIdentifiable
    {
        public required List<int> OrganizationIdsToAdd { get; init; }
        public required List<int> PersonIdsToAdd { get; init; }
        public required List<int> OrganizationIdsToRemove { get; init; }
        public required List<int> PersonIdsToRemove { get; init; }
        public required Identification.Certain Identification { get; init; }
    }
}
