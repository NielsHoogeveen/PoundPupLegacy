namespace PoundPupLegacy.CreateModel;

public sealed record NewSecondLevelGlobalRegion : NewGlobalRegionBase, EventuallyIdentifiableSecondLevelGlobalRegion
{
    public required int FirstLevelGlobalRegionId { get; init; }
}
public sealed record ExistingSecondLevelGlobalRegion : ExistingGlobalRegionBase, ImmediatelyIdentifiableSecondLevelGlobalRegion
{
    public required int FirstLevelGlobalRegionId { get; init; }
}
public interface ImmediatelyIdentifiableSecondLevelGlobalRegion : SecondLevelGlobalRegion, ImmediatelyIdentifiableGlobalRegion
{

}
public interface EventuallyIdentifiableSecondLevelGlobalRegion : SecondLevelGlobalRegion, EventuallyIdentifiableGlobalRegion
{

}
public interface SecondLevelGlobalRegion : GlobalRegion
{
    int FirstLevelGlobalRegionId { get; }
}
