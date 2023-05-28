namespace PoundPupLegacy.CreateModel;

public sealed record NewBasicSecondLevelSubdivision : NewISOCodedSubdivisionBase, EventuallyIdentifiableBasicSecondLevelSubdivision
{
    public required int IntermediateLevelSubdivisionId { get; init; }
}

public sealed record ExistingBasicSecondLevelSubdivision : ExistingISOCodedSubdivisionBase, ImmediatelyIdentifiableBasicSecondLevelSubdivision
{
    public required int IntermediateLevelSubdivisionId { get; init; }
}

public interface ImmediatelyIdentifiableBasicSecondLevelSubdivision : BasicSecondLevelSubdivision, SecondLevelSubdivisionToUpdate
{

}
public interface EventuallyIdentifiableBasicSecondLevelSubdivision : BasicSecondLevelSubdivision, SecondLevelSubdivisionToCreate
{

}
public interface BasicSecondLevelSubdivision : SecondLevelSubdivision
{
    int IntermediateLevelSubdivisionId { get; }
}