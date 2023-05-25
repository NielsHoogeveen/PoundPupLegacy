namespace PoundPupLegacy.CreateModel;

public sealed record NewDisruptedPlacementCase : NewCaseBase, EventuallyIdentifiableDisruptedPlacementCase
{
}
public sealed record ExistingDisruptedPlacementCase : ExistingCaseBase, ImmediatelyIdentifiableDisruptedPlacementCase
{
}
public interface ImmediatelyIdentifiableDisruptedPlacementCase : DisruptedPlacementCase, ImmediatelyIdentifiableCase
{
}
public interface EventuallyIdentifiableDisruptedPlacementCase : DisruptedPlacementCase, EventuallyIdentifiableCase
{
}
public interface DisruptedPlacementCase: Case
{
}
