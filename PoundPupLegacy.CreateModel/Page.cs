namespace PoundPupLegacy.CreateModel;

public sealed record NewPage : NewSimpleTextNodeBase, EventuallyIdentifiablePage
{
}
public sealed record ExistingPage : ExistingSimpleTextNodeBase, ImmediatelyIdentifiablePage
{
}
public interface ImmediatelyIdentifiablePage : Page, ImmediatelyIdentifiableSimpleTextNode
{
}
public interface EventuallyIdentifiablePage : Page, EventuallyIdentifiableSimpleTextNode
{
}
public interface Page : SimpleTextNode
{
}
