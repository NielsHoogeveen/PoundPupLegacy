using PoundPupLegacy.ViewModel;

namespace PoundPupLegacy.Services;

public interface ISiteDataService
{
    int GetUserId();
    Task InitializeAsync();
    string? GetUrlPathForId(int tenantId, int urlId);
    bool HasAccess();
    int GetTenantId();
    int? GetIdForUrlPath();
    IEnumerable<Link> GetMenuItemsForUser();
    string GetLayout();

}
