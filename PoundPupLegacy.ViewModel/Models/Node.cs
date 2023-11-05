using System.Net;

namespace PoundPupLegacy.ViewModel.Models;

public static class NodeHelper
{
    public static List<Comment> GetComments(this Node node)
    {
        var lst = new List<Comment>();
        var lst2 = new List<Comment>();
        foreach (var elem in node.CommentListItems.OrderBy(x => x.Id)) {
            if (!elem.CommentIdParent.HasValue) {
                var comment = new Comment {
                    Id = elem.Id,
                    CommentIdParent = null,
                    NodeId = node.NodeId,
                    Authoring = elem.Authoring,
                    NodeStatusId = elem.NodeStatusId,
                    Text = elem.Text,
                    Title = elem.Title,
                    Comments = new List<Comment>(),
                };
                lst.Add(comment);
                lst2.Add(comment);
            }
            else {
                var parentElement = lst2.FirstOrDefault(x => x.Id == elem.CommentIdParent.Value);
                if (parentElement is not null) {
                    var comment = new Comment {
                        Id = elem.Id,
                        CommentIdParent = parentElement.Id,
                        NodeId = node.NodeId,
                        Authoring = elem.Authoring,
                        NodeStatusId = elem.NodeStatusId,
                        Text = elem.Text,
                        Title = elem.Title,
                        Comments = new List<Comment>(),
                    };
                    parentElement.Comments.Add(comment);
                    lst2.Add(comment);
                }

            }
        }
        return lst.ToList();
    }
}


public abstract record NodeBase : Node
{
    public required int NodeId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Title { get; init; }
    public required Authoring Authoring { get; init; }
    public required bool HasBeenPublished { get; init; }
    public required int PublicationStatusId { get; init; }

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

    private List<Comment>? _comments = null;
    public List<Comment> Comments { 
        get {
            if (_comments is null) {
                _comments = this.GetComments();
            }
            return _comments; 
        } 
    }
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
    int NodeId { get; }
    int NodeTypeId { get; }
    string Title { get; }
    Authoring Authoring { get; }
    bool HasBeenPublished { get; }
    int PublicationStatusId { get; }
    TagListEntry[] Tags { get; }
    CommentListItem[] CommentListItems { get; }
    List<Comment> Comments { get; }
    BasicLink[] BreadCrumElements { get; }
    File[] Files { get; }

}
