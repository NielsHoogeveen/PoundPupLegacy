namespace PoundPupLegacy.CreateModel;
public interface EndoRelationTypeToUpdate : EndoRelationType, NameableToUpdate
{
}
public interface EndoRelationTypeToCreate: EndoRelationType, NameableToCreate
{
}
public interface EndoRelationType: Nameable
{
    EndoRelationTypeDetails EndoRelationTypeDetails { get; }
}
public sealed record EndoRelationTypeDetails
{
    public required Boolean IsSymmetric { get; init; }
}
