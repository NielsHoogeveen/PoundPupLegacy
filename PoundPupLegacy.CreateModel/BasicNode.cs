namespace PoundPupLegacy.CreateModel;

public sealed record NewBasicNode : NewNodeBase, EventuallyIdentifiableNode
{
}
public sealed record ExistingBasicNode : ExistingNodeBase, ImmediatelyIdentifiableNode
{
}
