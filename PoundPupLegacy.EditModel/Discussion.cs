namespace PoundPupLegacy.EditModel;

public interface Discussion : SimpleTextNode, ResolvedNode
{
}

[JsonSerializable(typeof(ExistingDiscussion))]
public partial class ExistingDiscussionJsonContext : JsonSerializerContext { }

public sealed record ExistingDiscussion : ExistingSimpleTextNodeBase, Discussion
{
}

[JsonSerializable(typeof(NewDiscussion))]
public partial class NewDiscussionJsonContext : JsonSerializerContext { }

public sealed record NewDiscussion : NewSimpleTextNodeBase, ResolvedNewNode, Discussion
{
}
