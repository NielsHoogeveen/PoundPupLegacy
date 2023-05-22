namespace PoundPupLegacy.CreateModel;

public sealed record NewSubdivisionType : NewNameableBase, EventuallyIdentifiableSubdivisionType
{
}
public sealed record ExistingSubdivisionType : ExistingNameableBase, ImmediatelyIdentifiableSubdivisionType
{
}
public interface ImmediatelyIdentifiableSubdivisionType : SubdivisionType, ImmediatelyIdentifiableNameable
{
}
public interface EventuallyIdentifiableSubdivisionType : SubdivisionType, EventuallyIdentifiableNameable
{
}
public interface SubdivisionType : Nameable
{
}
