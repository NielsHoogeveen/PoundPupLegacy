namespace PoundPupLegacy.CreateModel;

public sealed record DocumentableDocument: IRequest
{
    public required int DocumentableId { get; init; }

    public required int DocumentId { get; init; }
}
