namespace PoundPupLegacy.EditModel;

public record DocumentType
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required bool IsSelected { get; set; }
}
