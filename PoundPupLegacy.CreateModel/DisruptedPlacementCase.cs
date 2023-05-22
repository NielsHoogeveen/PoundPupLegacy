namespace PoundPupLegacy.CreateModel;

public sealed record NewDisruptedPlacementCase : NewCaseBase, EventuallyIdentifiableDisruptedPlacementCase
{
}
public sealed record ExistingDisruptedPlacementCase : ExistingCaseBase, ImmediatelyIdentifiableDisruptedPlacementCase
{
}
public interface ImmediatelyIdentifiableDisruptedPlacementCase : ImmediatelyIdentifiableCase
{
}
public interface EventuallyIdentifiableDisruptedPlacementCase : EventuallyIdentifiableCase
{
}
public interface DisruptedPlacementCase: Case
{
}
