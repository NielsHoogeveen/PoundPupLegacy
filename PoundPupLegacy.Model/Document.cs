namespace PoundPupLegacy.Model;

public record Document : Node
{
    public required int? Id { get; set; }
    public required int AccessRoleId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int NodeStatusId { get; init; }
    public required int NodeTypeId { get; init; }
    public required DateTimeRange? PublicationDate { get; init; }
    public required string? SourceUrl { get; init; }
    public required string Text { get; init; }
    public required int? DocumentTypeId { get; init; }
}
