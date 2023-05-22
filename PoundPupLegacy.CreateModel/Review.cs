namespace PoundPupLegacy.CreateModel;

public sealed record NewReview : NewSimpleTextNodeBase, EventuallyIdentifiableReview
{
}
public sealed record ExistingReview : ExistingSimpleTextNodeBase, ImmediatelyIdentifiableReview
{
}
public interface ImmediatelyIdentifiableReview : Review, ImmediatelyIdentifiableSimpleTextNode
{
}
public interface EventuallyIdentifiableReview : Review, EventuallyIdentifiableSimpleTextNode
{
}
public interface Review : SimpleTextNode
{
}
