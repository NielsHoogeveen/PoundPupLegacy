namespace PoundPupLegacy.CreateModel;

public sealed record NewFirstLevelGlobalRegion : NewGlobalRegionBase, EventuallyIdentifiableFirstLevelGlobalRegion
{
}

public sealed record ExistingFirstLevelGlobalRegion : ExistingGlobalRegionBase, ImmediatelyIdentifiableFirstLevelGlobalRegion
{
}

public interface ImmediatelyIdentifiableFirstLevelGlobalRegion : FirstLevelGlobalRegion, ImmediatelyIdentifiableGlobalRegion
{

}
public interface EventuallyIdentifiableFirstLevelGlobalRegion : FirstLevelGlobalRegion, EventuallyIdentifiableGlobalRegion
{

}
public interface FirstLevelGlobalRegion: GlobalRegion
{
}