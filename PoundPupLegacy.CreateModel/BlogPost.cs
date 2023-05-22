namespace PoundPupLegacy.CreateModel;

public sealed record NewBlogPost : NewSimpleTextNodeBase, EventuallyIdentifiableSimpleTextNode
{
}
public sealed record ExistingBlogPost : ExistingSimpleTextNodeBase, ImmediatelyIdentifiableSimpleTextNode
{
}
