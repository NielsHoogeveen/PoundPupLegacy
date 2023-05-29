namespace PoundPupLegacy.CreateModel;

public abstract record CaseParties
{
    public required string? Organizations { get; init; }
    public required string? Persons { get; init; }
    public sealed record CasePartiesToCreate : CaseParties, EventuallyIdentifiable
    {
        public required List<int> OrganizationIds { get; init; }
        public required List<int> PersonIds { get; init; }
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public Identification Identification => IdentificationForCreate;
    }
    public sealed record CasePartiesToUpdate : CaseParties, ImmediatelyIdentifiable
    {
        public required List<int> OrganizationIdsToAdd { get; init; }
        public required List<int> PersonIdsToAdd { get; init; }
        public required List<int> OrganizationIdsToRemove { get; init; }
        public required List<int> PersonIdsToRemove { get; init; }
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public Identification Identification => IdentificationForUpdate;
    }
}
