namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(PollListEntry))]
public partial class PollListEntryJsonContext : JsonSerializerContext { }

public record PollListEntry : TeaserListEntry
{
    public required string Path { get; init; }
    public required string Title { get; init; }

    public required string Text { get; init; }
    public required bool HasBeenPublished { get; init; }

}
