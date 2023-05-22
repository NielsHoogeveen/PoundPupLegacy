namespace PoundPupLegacy.CreateModel;

public sealed record NewTypeOfAbuse : NewNameableBase, EventuallyIdentifiableTypeOfAbuse
{
}
public sealed record ExistingTypeOfAbuse : ExistingNameableBase, ImmediatelyIdentifiableTypeOfAbuse
{
}
public interface ImmediatelyIdentifiableTypeOfAbuse : TypeOfAbuse, ImmediatelyIdentifiableNameable
{
}
public interface EventuallyIdentifiableTypeOfAbuse : TypeOfAbuse, EventuallyIdentifiableNameable
{
}
public interface TypeOfAbuse : Nameable
{
}