using PoundPupLegacy.Common;
using PoundPupLegacy.EditModel.UI.Services;

namespace PoundPupLegacy.Services.Implementation;

public class TenantRefreshService: ITenantRefreshService
{
    private readonly ISiteDataService _siteDataService;
    public TenantRefreshService(ISiteDataService siteDataService)
    {
        _siteDataService = siteDataService;
    }

    public async Task Refresh()
    {
        await _siteDataService.RefreshTenants();
    }
}
