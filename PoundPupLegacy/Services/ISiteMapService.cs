namespace PoundPupLegacy.Services;

public interface ISiteMapService
{
    public Task WriteSiteMap(int tenantId, StreamWriter writer);
}
