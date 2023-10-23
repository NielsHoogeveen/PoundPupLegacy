namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(PollListEntry))]
public partial class PollListEntryJsonContext : JsonSerializerContext { }

public sealed record PollListEntry : TeaserListEntry
{
    public required string Path { get; init; }
    public required string Title { get; init; }

    public required string Text { get; init; }
    public required bool HasBeenPublished { get; init; }

    public required int PublicationStatusId { get; init; }
}
