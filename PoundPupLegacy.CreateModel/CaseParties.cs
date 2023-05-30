namespace PoundPupLegacy.CreateModel;

public abstract record CaseParties
{
    public required string? Organizations { get; init; }
    public required string? Persons { get; init; }
    public sealed record ToCreate : CaseParties, PossiblyIdentifiable
    {
        public required List<int> OrganizationIds { get; init; }
        public required List<int> PersonIds { get; init; }
        public required Identification.Possible IdentificationForCreate { get; init; }
        public Identification Identification => IdentificationForCreate;
    }
    public sealed record ToUpdate : CaseParties, CertainlyIdentifiable
    {
        public required List<int> OrganizationIdsToAdd { get; init; }
        public required List<int> PersonIdsToAdd { get; init; }
        public required List<int> OrganizationIdsToRemove { get; init; }
        public required List<int> PersonIdsToRemove { get; init; }
        public required Identification.Certain IdentificationCertain { get; init; }
        public Identification Identification => IdentificationCertain;
    }
}
