using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Services;

public interface IDefaultCountryService
{
    (int, string) GetDefaultCountry(int tenantId);
}
