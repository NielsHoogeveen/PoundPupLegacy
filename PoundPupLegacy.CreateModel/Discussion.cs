namespace PoundPupLegacy.CreateModel;

public sealed record NewDiscussion : NewSimpleTextNodeBase, EventuallyIdentifiableDiscussion
{
}
public sealed record ExistingDiscussion : ExistingSimpleTextNodeBase, ImmediatelyIdentifiableDiscussion
{
}
public interface ImmediatelyIdentifiableDiscussion : Discussion, ImmediatelyIdentifiableSimpleTextNode
{
}
public interface EventuallyIdentifiableDiscussion : Discussion, EventuallyIdentifiableSimpleTextNode
{
}
public interface Discussion: SimpleTextNode
{
}