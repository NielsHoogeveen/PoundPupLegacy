using PoundPupLegacy.Models;
using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.Services;

public interface ISiteDataService
{
    (int, string) GetDefaultCountry(int tenantId);
    Task InitializeAsync();
    string? GetUrlPathForId(int tenantId, int urlId);
    bool HasAccess(int userId, int tenantId, HttpRequest request);
    bool HasAccess(int userId, int tenantId, Uri uri);
    bool CanEdit(Node node, int userId, int tenantId);
    int GetTenantId(HttpRequest httpRequest);
    int GetTenantId(Uri uri);
    int? GetIdForUrlPath(HttpRequest httpRequest);
    int? GetIdForUrlPath(string urlPath, int tenantId);
    IEnumerable<MenuItem> GetMenuItemsForUser(int userId, int tenantId);
    string GetLayout(int userId, int tenantId);
    Task RefreshTenants();
}
