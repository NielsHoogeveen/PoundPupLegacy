namespace PoundPupLegacy.CreateModel;

public interface ImmediatelyIdentifiableGlobalRegion : GlobalRegion, ImmediatelyIdentifiableGeographicalEntity
{

}
public interface EventuallyIdentifiableGlobalRegion : GlobalRegion, EventuallyIdentifiableGeographicalEntity
{

}
public interface GlobalRegion : GeographicalEntity
{
    string Name { get; }
}

public abstract record NewGlobalRegionBase: NewNameableBase, EventuallyIdentifiableGlobalRegion
{
    public required string Name { get; init; }
}

public abstract record ExistingGlobalRegionBase : ExistingNameableBase, ImmediatelyIdentifiableGlobalRegion
{
    public required string Name { get; init; }
}
