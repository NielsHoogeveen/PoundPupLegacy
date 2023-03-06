namespace PoundPupLegacy.EditModel;

public record DocumentableDocument
{
    public required int DocumentableId { get; init; }
    public required int DocumentId { get; init; }
    public required string Title { get; init; }
    public bool IsStored { get; set; } = true;
    public bool HasBeenDeleted { get; set; } = false;
    public void SetToDeleted()
    {
        HasBeenDeleted = true;
    }
}
