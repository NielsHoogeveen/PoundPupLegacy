using System.Text.Json.Serialization;

namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(BasicPollQuestion))]
public partial class BasicPollQuestionJsonContext : JsonSerializerContext { }

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

    private TagListEntry[] tags = Array.Empty<TagListEntry>();
    public TagListEntry[] Tags {
        get => tags;
        init {
            if (value is not null) {
                tags = value;
            }

        }
    }


    public CommentListItem[] CommentListItems => Array.Empty<CommentListItem>();

    public Comment[] Comments => Array.Empty<Comment>();

    public BasicLink[] BreadCrumElements => Array.Empty<BasicLink>();

    public File[] Files => Array.Empty<File>();
}
