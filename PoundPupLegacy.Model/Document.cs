namespace PoundPupLegacy.Model;

public record Document : Node
{
    public required int Id { get; set; }
    public required int UserId { get; init; }
    public required DateTime Created { get; init; }
    public required DateTime Changed { get; init; }
    public required string Title { get; init; }
    public required int Status { get; init; }
    public required int NodeTypeId { get; init; }
    public required DateTimeRange? PublicationDate { get; init; }
    public required string? SourceUrl { get; init; }
    public required string Text { get; init; }
    public required int? DocumentTypeId { get; init; }
}
