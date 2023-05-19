namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(CaseListEntry))]
public partial class CaseListEntryJsonContext : JsonSerializerContext { }

public record CaseListEntry : TaggedTeaserListEntry
{
    public required string Path { get; init; }
    public required string Title { get; init; }

    public required string? Text { get; init; }
    public required bool HasBeenPublished { get; init; }
    public required FuzzyDate? Date { get; init; }

    private TagListEntry[] tags = Array.Empty<TagListEntry>();
    public TagListEntry[] Tags {
        get => tags;
        init {
            if (value is not null) {
                tags = value;
            }

        }
    }

}
