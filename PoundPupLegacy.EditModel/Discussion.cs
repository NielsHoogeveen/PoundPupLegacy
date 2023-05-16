namespace PoundPupLegacy.EditModel;


public interface Discussion : SimpleTextNode
{

}
public abstract record DiscussionBase : SimpleTextNodeBase, Discussion
{

}

[JsonSerializable(typeof(ExistingDiscussion))]
public partial class ExistingDiscussionJsonContext : JsonSerializerContext { }

public sealed record ExistingDiscussion : DiscussionBase, ExistingNode
{
    public int NodeId { get; init; }

    public int UrlId { get; set; }

}

[JsonSerializable(typeof(NewDiscussion))]
public partial class NewDiscussionJsonContext : JsonSerializerContext { }

public sealed record NewDiscussion : DiscussionBase, NewNode
{
}
