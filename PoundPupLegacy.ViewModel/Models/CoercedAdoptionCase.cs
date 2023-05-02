namespace PoundPupLegacy.ViewModel.Models;

public record CoercedAdoptionCase : Case
{
    public required string Description { get; init; }
    public required int NodeId { get; init; }
    public required int UrlId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Title { get; init; }
    public required Authoring Authoring { get; init; }
    public required bool HasBeenPublished { get; init; }

    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public FuzzyDate? FuzzyDate {
        get {
            if (DateFrom is not null && DateTo is not null) {
                var dateTimeRange = new DateTimeRange(DateFrom, DateTo);
                if (FuzzyDate.TryFromDateTimeRange(dateTimeRange, out var result)) {
                    return result;
                }
            }
            return null;
        }
    }
    private TagListEntry[] tags = Array.Empty<TagListEntry>();
    public required TagListEntry[] Tags {
        get => tags;
        init {
            if (value is not null) {
                tags = value;
            }

        }
    }
    private CommentListItem[] commentListItems = Array.Empty<CommentListItem>();
    public CommentListItem[] CommentListItems {
        get => commentListItems;
        init {
            if (value is not null) {
                commentListItems = value;
            }
        }
    }

    public Comment[] Comments => this.GetComments(); public required BasicLink[] BreadCrumElements { get; init; }

    private DocumentListEntry[] documents = Array.Empty<DocumentListEntry>();
    public required DocumentListEntry[] Documents {
        get => documents;
        init {
            if (value is not null) {
                documents = value;
            }
        }
    }

    private Location[] _locations = Array.Empty<Location>();
    public required Location[] Locations {
        get => _locations;
        init {
            if (value is not null) {
                _locations = value;
            }
        }
    }


    private BasicLink[] subTopics = Array.Empty<BasicLink>();
    public required BasicLink[] SubTopics {
        get => subTopics;
        init {
            if (value is not null) {
                subTopics = value;
            }
        }
    }

    private BasicLink[] superTopics = Array.Empty<BasicLink>();
    public required BasicLink[] SuperTopics {
        get => superTopics;
        init {
            if (value is not null) {
                superTopics = value;
            }
        }
    }

    private CaseParties[] _caseParties = Array.Empty<CaseParties>();
    public required CaseParties[] CaseParties {
        get => _caseParties;
        init {
            if (value is not null) {
                _caseParties = value;
            }
        }
    }
    private File[] _files = Array.Empty<File>();
    public required File[] Files {
        get => _files;
        init {
            if (value is not null) {
                _files = value;
            }
        }
    }
}
