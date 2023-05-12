namespace PoundPupLegacy.CreateModel;

public sealed record Person : Party
{
    public required int? Id { get; set; }
    public required int PublisherId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int OwnerId { get; init; }
    public required int AuthoringStatusId { get; init; }
    public required int NodeTypeId { get; init; }
    public required DateTime? DateOfBirth { get; init; }
    public required DateTime? DateOfDeath { get; init; }
    public required int? FileIdPortrait { get; init; }
    public required string Description { get; init; }
    public required List<VocabularyName> VocabularyNames { get; init; }
    public required int? FileIdTileImage { get; init; }
    public required List<TenantNode> TenantNodes { get; init; }
    public required string? FirstName { get; init; }
    public required string? MiddleName { get; init; }
    public required string? LastName { get; init; }
    public required string? FullName { get; init; }
    public required string? Suffix { get; init; }
    public required int? GovtrackId { get; init; }
    public required string? Bioguide { get; init; }
    public required List<ProfessionalRole> ProfessionalRoles { get; init; }
    public required List<PersonOrganizationRelation> PersonOrganizationRelations { get; init; }
}
