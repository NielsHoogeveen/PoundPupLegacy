namespace PoundPupLegacy.Model;

public sealed record Document : Node
{
    public required int? Id { get; set; }
    public required int PublisherId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int? OwnerId { get; init; }
    public required int NodeTypeId { get; init; }
    public required DateTimeRange? PublicationDate { get; init; }
    public required string? SourceUrl { get; init; }
    public required string Text { get; init; }
    public required int? DocumentTypeId { get; init; }
    public required List<TenantNode> TenantNodes { get; init; }
}
