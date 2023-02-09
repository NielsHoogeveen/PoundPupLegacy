using PoundPupLegacy.ViewModel;

namespace PoundPupLegacy.Services;

public interface ISiteDataService
{
    int GetUserId(HttpContext context);
    Task InitializeAsync();
    string? GetUrlPathForId(int tenantId, int urlId);
    bool HasAccess(HttpContext context);
    int GetTenantId(HttpContext context);
    int? GetIdForUrlPath(HttpContext context);
    IEnumerable<Link> GetMenuItemsForUser(HttpContext context);
    string GetLayout(HttpContext context);

}
