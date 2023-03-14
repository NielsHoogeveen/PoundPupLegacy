namespace PoundPupLegacy.ViewModel;

public record Document : Node
{
    public required int NodeId { get; init; }
    public required int UrlId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Title { get; init; }
    public required string Text { get; init; }
    public DateTime? DateTime { get; init; }
    public DateTime? DateTimeFrom { get; init; }
    public DateTime? DateTimeTo { get; init; }
    public Link? DocumentType { get; init; }
    public string? PublicationDate {
        get {
            if (DateTime.HasValue) {
                return DateTime.Value.ToString("yyyy MMMM dd");
            }
            if (DateTimeFrom.HasValue && DateTimeTo.HasValue) {
                if (DateTimeFrom.Value.Month == DateTimeTo.Value.Month) {
                    return DateTimeFrom.Value.ToString("yyyy MMMM");
                }
                else {
                    return DateTimeFrom.Value.ToString("yyyy");
                }
            }
            return null;
        }
    }

    public string? SourceUrl { get; init; }
    public string? SourceUrlHost => SourceUrl is null ? null : new Uri(SourceUrl).Host;
    public required Authoring Authoring { get; init; }
    public required bool HasBeenPublished { get; init; }

    private Link[] tags = Array.Empty<Link>();
    public Link[] Tags {
        get => tags;
        init {
            if (value is not null) {
                tags = value;
            }

        }
    }
    private Link[] documentables = Array.Empty<Link>();
    public Link[] Documentables {
        get => documentables;
        init {
            if (value is not null) {
                documentables = value;
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

    public Comment[] Comments => this.GetComments();
    public required Link[] BreadCrumElements { get; init; }

    private File[] _files = Array.Empty<File>();

    public Document()
    {
    }

    public required File[] Files {
        get => _files;
        init {
            if (value is not null) {
                _files = value;
            }
        }
    }


}
