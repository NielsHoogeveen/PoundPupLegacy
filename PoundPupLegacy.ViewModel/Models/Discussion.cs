﻿namespace PoundPupLegacy.ViewModel.Models;

public record Discussion : SimpleTextNode
{
    public required int NodeId { get; init; }
    public required int UrlId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Title { get; init; }
    public required string Text { get; init; }
    public required Authoring Authoring { get; init; }
    public required bool HasBeenPublished { get; init; }

    private BasicLink[]? tags;
    public required BasicLink[] Tags {
        get => tags is null ? Array.Empty<BasicLink>() : tags;
        init => tags = value;
    }

    private BasicLink[]? seeAlsoBoxElements;
    public required BasicLink[] SeeAlsoBoxElements {
        get => seeAlsoBoxElements is null ? Array.Empty<BasicLink>() : seeAlsoBoxElements;
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
