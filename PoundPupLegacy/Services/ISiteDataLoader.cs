using PoundPupLegacy.Models;

namespace PoundPupLegacy.Services;

public interface ISiteDataLoader
{
    Task<SiteData> GetSiteData();

    Task<UserWithDetails> LoadUser(int tenantId, int userId);

}
