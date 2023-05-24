namespace PoundPupLegacy.CreateModel;

public sealed record NewBlogPost : NewSimpleTextNodeBase, EventuallyIdentifiableBlogPost
{
}
public sealed record ExistingBlogPost : ExistingSimpleTextNodeBase, ImmediatelyIdentifiableBlogPost
{
}
public interface ImmediatelyIdentifiableBlogPost : BlogPost, ImmediatelyIdentifiableSimpleTextNode
{
}

public interface EventuallyIdentifiableBlogPost : BlogPost, EventuallyIdentifiableSimpleTextNode
{
}

public interface BlogPost: SimpleTextNode
{
}
