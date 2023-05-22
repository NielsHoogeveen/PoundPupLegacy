namespace PoundPupLegacy.CreateModel;

public sealed record NewFamilySize : NewNameableBase, EventuallyIdentifiableFamilySize
{
}
public sealed record ExistingFamilySize : ExistingNameableBase, ImmediatelyIdentifiableFamilySize
{
}

public interface ImmediatelyIdentifiableFamilySize : FamilySize, ImmediatelyIdentifiableNameable
{
}

public interface EventuallyIdentifiableFamilySize : FamilySize, EventuallyIdentifiableNameable
{
}

public interface FamilySize: Nameable
{
}
