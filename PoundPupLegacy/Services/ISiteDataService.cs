using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.Services;

public interface ISiteDataService
{
    (int, string) GetDefaultCountry(int tenantId);

    Task<bool> InitializeAsync();

    int? GetUserByNameIdentifier(string id);

    string? GetUrlPathForId(int urlId);

    Task<bool> HasAccess(int userId, string path);

    Task<bool> CanEdit(Node node, int userId);

    Task<bool> CanViewNodeAccess(int userId);

    Task<bool> CanCreate(int nodeTypeId, int userId);

    Tenant GetTenant();

    int? GetIdForUrlPath(string urlPath);

    Task<List<MenuItem>> GetMenuItemsForUser(int userId);

    Task RefreshTenants();
    void RemoveUser(int userId);
}
