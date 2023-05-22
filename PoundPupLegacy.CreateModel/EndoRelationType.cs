namespace PoundPupLegacy.CreateModel;
public interface ImmediatelyIdentifiableEndoRelationType : EndoRelationType, ImmediatelyIdentifiableNameable
{
}
public interface EventuallyIdentifiableEndoRelationType: EndoRelationType, EventuallyIdentifiableNameable
{
}
public interface EndoRelationType: Nameable
{
    Boolean IsSymmetric { get; }
}
public abstract record NewEndoRelationTypeBase: NewNameableBase, EventuallyIdentifiableEndoRelationType
{
    public required Boolean IsSymmetric { get; init; }
}
public abstract record ExistingEndoRelationTypeBase : ExistingNameableBase, ImmediatelyIdentifiableEndoRelationType
{
    public required Boolean IsSymmetric { get; init; }
}
