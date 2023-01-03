namespace PoundPupLegacy.Model;

public record Organization : Party
{
    public required int? Id { get; set; }
    public required int AccessRoleId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int NodeStatusId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string? WebsiteURL { get; init; }
    public required string? EmailAddress { get; init; }
    public required string Description { get; init; }
    public required DateTime? Established { get; init; }
    public required DateTime? Terminated { get; init; }
    public required List<VocabularyName> VocabularyNames { get; init; }
    public required int? FileIdTileImage { get; init; }

}
