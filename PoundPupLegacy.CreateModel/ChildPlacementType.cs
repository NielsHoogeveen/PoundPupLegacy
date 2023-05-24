namespace PoundPupLegacy.CreateModel;

public sealed record NewChildPlacementType : NewNameableBase, EventuallyIdentifiableChildPlacementType
{
}
public sealed record ExistingChildPlacementType : ExistingNameableBase, ImmediatelyIdentifiableChildPlacementType
{
}
public interface ImmediatelyIdentifiableChildPlacementType : ChildPlacementType, ImmediatelyIdentifiableNameable
{
}
public interface EventuallyIdentifiableChildPlacementType : ChildPlacementType, EventuallyIdentifiableNameable 
{ 
}
public interface ChildPlacementType: Nameable
{
}