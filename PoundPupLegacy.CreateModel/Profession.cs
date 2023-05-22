namespace PoundPupLegacy.CreateModel;

public sealed record NewProfession : NewNameableBase, EventuallyIdentifiableProfession
{
    public required bool HasConcreteSubtype { get; init; }
}
public sealed record ExistingProfession : ExistingNameableBase, ImmediatelyIdentifiableProfession
{
    public required bool HasConcreteSubtype { get; init; }
}
public interface ImmediatelyIdentifiableProfession : Profession, ImmediatelyIdentifiableNameable
{
}
public interface EventuallyIdentifiableProfession : Profession, EventuallyIdentifiableNameable
{
}
public interface Profession : Nameable
{
    bool HasConcreteSubtype { get; }
}
