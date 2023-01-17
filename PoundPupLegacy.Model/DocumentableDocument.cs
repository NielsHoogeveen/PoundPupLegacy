namespace PoundPupLegacy.Model;

public sealed record DocumentableDocument
{
    public required int DocumentableId { get; init; }

    public required int DocumentId { get; init; }
}
