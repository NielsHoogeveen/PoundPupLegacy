namespace PoundPupLegacy.DomainModel;

public interface GlobalRegionToUpdate : GlobalRegion, GeographicalEntityToUpdate
{
}
public interface GlobalRegionToCreate : GlobalRegion, GeographicalEntityToCreate
{
}
public interface GlobalRegion : GeographicalEntity
{
    GlobalRegionDetails GlobalRegionDetails { get; }
}

public sealed record GlobalRegionDetails
{
    public required string Name { get; init; }
}

