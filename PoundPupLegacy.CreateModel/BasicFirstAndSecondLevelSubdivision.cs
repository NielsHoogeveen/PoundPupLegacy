namespace PoundPupLegacy.CreateModel;

public sealed record ExistingBasicFirstAndSecondLevelSubdivision : ExistingISOCodedSubdivisionBase, ImmediatelyIdentifiableBasicFirstAndSecondLevelSubdivision
{
    public required int IntermediateLevelSubdivisionId { get; init; }
}

public sealed record NewBasicFirstAndSecondLevelSubdivision : NewISOCodedSubdivisionBase, EventuallyIdentifiableBasicFirstAndSecondLevelSubdivision
{
    public required int IntermediateLevelSubdivisionId { get; init; }
}
public interface ImmediatelyIdentifiableBasicFirstAndSecondLevelSubdivision : BasicFirstAndSecondLevelSubdivision, ImmediatelyIdentifiableFirstAndSecondLevelSubdivision
{
}

public interface EventuallyIdentifiableBasicFirstAndSecondLevelSubdivision : BasicFirstAndSecondLevelSubdivision, EventuallyIdentifiableFirstAndSecondLevelSubdivision
{
}

public interface BasicFirstAndSecondLevelSubdivision: FirstAndSecondLevelSubdivision
{
}
