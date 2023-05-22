namespace PoundPupLegacy.CreateModel;

public sealed record NewChildPlacementType : NewNameableBase, EventuallyIdentifiableNameable
{
}
public sealed record ExistingChildPlacementType : ExistingNameableBase, ImmediatelyIdentifiableNameable
{
}
