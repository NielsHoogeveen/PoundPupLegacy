namespace PoundPupLegacy.ViewModel;

public record Discussion : SimpleTextNode
{
    public required int NodeId { get; init; }
    public required int UrlId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Title { get; init; }
    public required string Text { get; init; }
    public required Authoring Authoring { get; init; }
    public required bool HasBeenPublished { get; init; }

    private Link[]? tags;
    public required Link[] Tags {
        get => tags is null ? Array.Empty<Link>() : tags;
        init => tags = value;
    }

    private Link[]? seeAlsoBoxElements;
    public required Link[] SeeAlsoBoxElements {
        get => seeAlsoBoxElements is null ? Array.Empty<Link>() : seeAlsoBoxElements;
        init => seeAlsoBoxElements = value;
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
    public required File[] Files {
        get => _files;
        init {
            if (value is not null) {
                _files = value;
            }
        }
    }


}
