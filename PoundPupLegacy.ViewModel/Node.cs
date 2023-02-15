namespace PoundPupLegacy.ViewModel;

public interface Node
{
    public int Id { get; }
    public int NodeTypeId { get;  }
    public string Title { get; }
    public Authoring Authoring { get; }
    public bool HasBeenPublished { get; }
    public Link[] Tags { get;  }
    public Comment[] Comments { get; }
    public Link[] BreadCrumElements { get;  }
    public File[] Files { get;  }

}
