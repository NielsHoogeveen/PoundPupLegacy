namespace PoundPupLegacy.CreateModel;

public sealed record NewBasicSecondLevelSubdivision : NewISOCodedSubdivisionBase, EventuallyIdentifiableSecondLevelSubdivision
{
    public required int IntermediateLevelSubdivisionId { get; init; }
}

public sealed record ExistingBasicSecondLevelSubdivision : ExistingISOCodedSubdivisionBase, ImmediatelyIdentifiableSecondLevelSubdivision
{
    public required int IntermediateLevelSubdivisionId { get; init; }
}
