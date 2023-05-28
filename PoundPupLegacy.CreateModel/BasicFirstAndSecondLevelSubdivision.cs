namespace PoundPupLegacy.CreateModel;

public sealed record ExistingBasicFirstAndSecondLevelSubdivision : ExistingISOCodedSubdivisionBase, ImmediatelyIdentifiableBasicFirstAndSecondLevelSubdivision
{
    public required int IntermediateLevelSubdivisionId { get; init; }
}

public sealed record NewBasicFirstAndSecondLevelSubdivision : NewISOCodedSubdivisionBase, EventuallyIdentifiableBasicFirstAndSecondLevelSubdivision
{
    public required int IntermediateLevelSubdivisionId { get; init; }
}
public interface ImmediatelyIdentifiableBasicFirstAndSecondLevelSubdivision : BasicFirstAndSecondLevelSubdivision, FirstAndSecondLevelSubdivisionToUpdate
{
}

public interface EventuallyIdentifiableBasicFirstAndSecondLevelSubdivision : BasicFirstAndSecondLevelSubdivision, FirstAndSecondLevelSubdivisionToCreate
{
}

public interface BasicFirstAndSecondLevelSubdivision: FirstAndSecondLevelSubdivision
{
}
