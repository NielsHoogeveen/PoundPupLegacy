namespace PoundPupLegacy.ViewModel.Models;

public record BasicPollQuestion : PollQuestion
{
    public required int NodeId { get; init; }
    public required int UrlId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Title { get; init; }
    public required string Text { get; init; }
    public required Authoring Authoring { get; init; }
    public required bool HasBeenPublished { get; init; }

    private PollOption[] pollOptions = Array.Empty<PollOption>();
    public PollOption[] PollOptions {
        get => pollOptions;
        init {
            if (value is not null) {
                pollOptions = value;
            }

        }
    }

    public BasicLink[] SeeAlsoBoxElements => Array.Empty<BasicLink>();

    public BasicLink[] Tags => Array.Empty<BasicLink>();

    public CommentListItem[] CommentListItems => Array.Empty<CommentListItem>();

    public Comment[] Comments => Array.Empty<Comment>();

    public BasicLink[] BreadCrumElements => Array.Empty<BasicLink>();

    public File[] Files => Array.Empty<File>();
}
