namespace PoundPupLegacy.CreateModel;

public sealed record NewDiscussion : NewSimpleTextNodeBase, EventuallyIdentifiableDiscussion
{
}
public sealed record ExistingDiscussion : ExistingSimpleTextNodeBase, ImmediatelyIdentifiableDiscussion
{
}
public interface ImmediatelyIdentifiableDiscussion : ImmediatelyIdentifiableSimpleTextNode
{
}
public interface EventuallyIdentifiableDiscussion : EventuallyIdentifiableSimpleTextNode
{
}
public interface Discussion: SimpleTextNode
{
}