namespace PoundPupLegacy.ViewModel;

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

public interface Node
{
    public int Id { get; }
    public int NodeTypeId { get;  }
    public string Title { get; }
    public Authoring Authoring { get; }
    public bool HasBeenPublished { get; }
    public Link[] Tags { get;  }
    public CommentListItem[] CommentListItems { get; }
    public Comment[] Comments { get; }
    public Link[] BreadCrumElements { get;  }
    public File[] Files { get;  }

}
