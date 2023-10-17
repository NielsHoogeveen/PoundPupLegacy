namespace PoundPupLegacy.Services;

public interface ISiteMapService
{
    public Task<string> GetSiteMapIndex(int tenantId);
    public Task<string> GetSiteMap(int tenantId, int index);
}
