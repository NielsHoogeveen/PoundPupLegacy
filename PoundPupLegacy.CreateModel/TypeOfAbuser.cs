namespace PoundPupLegacy.CreateModel;

public sealed record NewTypeOfAbuser : NewNameableBase, EventuallyIdentifiableTypeOfAbuser
{
}
public sealed record ExistingTypeOfAbuser : ExistingNameableBase, ImmediatelyIdentifiableTypeOfAbuser
{
}
public interface ImmediatelyIdentifiableTypeOfAbuser : TypeOfAbuser, ImmediatelyIdentifiableNameable
{
}
public interface EventuallyIdentifiableTypeOfAbuser : TypeOfAbuser, EventuallyIdentifiableNameable
{
}
public interface TypeOfAbuser : Nameable
{
}