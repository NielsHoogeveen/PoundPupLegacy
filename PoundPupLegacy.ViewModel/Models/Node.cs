﻿namespace PoundPupLegacy.ViewModel.Models;

public static class NodeHelper
{
    public static Comment[] GetComments(this Node node)
    {
        var lst = new List<Comment>();
        foreach (var elem in node.CommentListItems.OrderBy(x => x.Id)) {
            if (elem.CommentIdParent == 0) {
                lst.Add(new Comment {
                    Id = elem.Id,
                    Authoring = elem.Authoring,
                    NodeStatusId = elem.NodeStatusId,
                    Text = elem.Text,
                    Title = elem.Title,
                    Comments = new List<Comment>(),
                });
            }
            else {
                var parentElement = lst.FirstOrDefault(x => x.Id == elem.CommentIdParent);
                if (parentElement is not null) {
                    var comments = parentElement.Comments.ToList();
                    parentElement.Comments.Add(new Comment {
                        Id = elem.Id,
                        Authoring = elem.Authoring,
                        NodeStatusId = elem.NodeStatusId,
                        Text = elem.Text,
                        Title = elem.Title,
                        Comments = new List<Comment>(),
                    });
                }

            }
        }
        return lst.ToArray();
    }
}


public abstract record NodeBase : Node
{
    public required int NodeId { get; init; }
    public required int UrlId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Title { get; init; }
    public required Authoring Authoring { get; init; }
    public required bool HasBeenPublished { get; init; }

    private TagListEntry[] tags = Array.Empty<TagListEntry>();
    public TagListEntry[] Tags {
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

public interface Node
{
    public int UrlId { get; }
    public int NodeId { get; }
    public int NodeTypeId { get; }
    public string Title { get; }
    public Authoring Authoring { get; }
    public bool HasBeenPublished { get; }
    public TagListEntry[] Tags { get; }
    public CommentListItem[] CommentListItems { get; }
    public Comment[] Comments { get; }
    public BasicLink[] BreadCrumElements { get; }
    public File[] Files { get; }

}
