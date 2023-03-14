namespace PoundPupLegacy.ViewModel;

public record class SingleQuestionPoll : Poll, PollQuestion
{
    public required int NodeId { get; init; }
    public required int UrlId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Title { get; init; }
    public required string Text { get; init; }
    public required Authoring Authoring { get; init; }
    public required bool HasBeenPublished { get; init; }
    public required int PollStatusId { get; init; }
    public required string Question { get; init; }

    public DateTime? DateTimeClosure { get; init; }

    private PollOption[] pollOptions = Array.Empty<PollOption>();
    public PollOption[] PollOptions {
        get => pollOptions;
        init {
            if (value is not null) {
                pollOptions = value;
            }

        }
    }

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
