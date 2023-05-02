namespace PoundPupLegacy.ViewModel.Models;

public record CaseListEntry : TaggedTeaserListEntry
{
    public required string Path { get; init; }
    public required string Title { get; init; }
    
    public required string? Text { get; init; }
    public required bool HasBeenPublished { get; init; }
    public required DateTimeInterval? DateTimeRange { get;  init; }
    public FuzzyDate? Date => DateTimeRange?.ToFuzzyDate();

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
