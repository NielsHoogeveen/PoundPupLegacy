using PoundPupLegacy.ViewModel;

namespace PoundPupLegacy.Services;

public interface ISiteDataService
{
    (int, string) GetDefaultCountry(int tenantId);
    Task InitializeAsync();
    string? GetUrlPathForId(int tenantId, int urlId);
    bool HasAccess(int userId, int tenantId, HttpRequest request);
    bool CanEdit(Node node, int userId, int tenantId);
    int GetTenantId(HttpRequest httpRequest);
    int? GetIdForUrlPath(HttpRequest httpRequest);
    IEnumerable<Link> GetMenuItemsForUser(int userId, int tenantId);
    string GetLayout(int userId, int tenantId);
    Task RefreshTenants();
}
