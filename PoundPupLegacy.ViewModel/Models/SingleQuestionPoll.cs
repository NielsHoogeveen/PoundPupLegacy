﻿namespace PoundPupLegacy.ViewModel.Models;

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

    private BasicLink[] tags = Array.Empty<BasicLink>();
    public BasicLink[] Tags {
        get => tags;
        init {
            if (value is not null) {
                tags = value;
            }

        }
    }

    private BasicLink[] seeAlsoBoxElements = Array.Empty<BasicLink>();
    public BasicLink[] SeeAlsoBoxElements {
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

    public required BasicLink[] BreadCrumElements { get; init; }

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
