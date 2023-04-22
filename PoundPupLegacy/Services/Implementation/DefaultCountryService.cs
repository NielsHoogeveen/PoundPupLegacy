using PoundPupLegacy.EditModel.UI.Services;

namespace PoundPupLegacy.Services.Implementation;

public class DefaultCountryService : IDefaultCountryService
{

    private readonly ISiteDataService _siteDataService;
    public DefaultCountryService(ISiteDataService siteDataService)
    {
        _siteDataService = siteDataService;
    }
    public (int, string) GetDefaultCountry(int tenantId)
    {
        return _siteDataService.GetDefaultCountry(tenantId);
    }
}
