using PoundPupLegacy.EditModel.UI.Services;

namespace PoundPupLegacy.Services.Implementation;

public class TenantRefreshService(ISiteDataService siteDataService) : ITenantRefreshService
{
    public async Task Refresh()
    {
        await siteDataService.RefreshTenants();
    }
}
