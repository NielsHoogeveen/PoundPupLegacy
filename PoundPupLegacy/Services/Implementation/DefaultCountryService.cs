using PoundPupLegacy.EditModel.UI.Services;

namespace PoundPupLegacy.Services.Implementation;

public class DefaultCountryService(ISiteDataService siteDataService) : IDefaultCountryService
{

    public (int, string) GetDefaultCountry(int tenantId)
    {
        return siteDataService.GetDefaultCountry(tenantId);
    }
}
