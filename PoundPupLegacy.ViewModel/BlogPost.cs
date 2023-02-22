namespace PoundPupLegacy.ViewModel;

public record class BlogPost : SimpleTextNode
{
    public required int Id { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Title { get; init; }
    public required string Text { get; init; }
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

    private Link[] seeAlsoBoxElements = Array.Empty<Link>();
    public Link[] SeeAlsoBoxElements {
        get => seeAlsoBoxElements;
        init {
            if (value is not null) {
                seeAlsoBoxElements = value;
            }
        }
    }

    private CommentListItem[] commentListItems = Array.Empty<CommentListItem>();
    public CommentListItem[] CommentListItems {
        get => commentListItems;
        init {
            if (value is not null) {
                commentListItems = value;
                comments = this.GetComments();
            }
        }
    }

    private Comment[] comments = Array.Empty<Comment>();
    public Comment[] Comments => comments;

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
