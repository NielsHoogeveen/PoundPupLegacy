namespace PoundPupLegacy.Services;

public interface ISiteMapService
{
    public Task<string> GetSiteMapIndex();
    public Task<string> GetSiteMap(int index);
}
