using PoundPupLegacy.Models;

namespace PoundPupLegacy.Services;

public interface ISiteDataLoader
{
    Task<SiteData> GetSiteData();

    Task<User> LoadUser(int tenantId, int userId);

}
