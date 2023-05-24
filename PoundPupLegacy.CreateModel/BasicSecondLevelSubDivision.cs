namespace PoundPupLegacy.CreateModel;

public sealed record NewBasicSecondLevelSubdivision : NewISOCodedSubdivisionBase, EventuallyIdentifiableBasicSecondLevelSubdivision
{
    public required int IntermediateLevelSubdivisionId { get; init; }
}

public sealed record ExistingBasicSecondLevelSubdivision : ExistingISOCodedSubdivisionBase, ImmediatelyIdentifiableBasicSecondLevelSubdivision
{
    public required int IntermediateLevelSubdivisionId { get; init; }
}

public interface ImmediatelyIdentifiableBasicSecondLevelSubdivision : BasicSecondLevelSubdivision, ImmediatelyIdentifiableSecondLevelSubdivision
{

}
public interface EventuallyIdentifiableBasicSecondLevelSubdivision : BasicSecondLevelSubdivision, EventuallyIdentifiableSecondLevelSubdivision
{

}
public interface BasicSecondLevelSubdivision : SecondLevelSubdivision
{
    int IntermediateLevelSubdivisionId { get; }
}