using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.Services;

public interface ISiteDataService
{
    (int, string) GetDefaultCountry(int tenantId);
    Task InitializeAsync();

    [RequireNamedArgs]
    string? GetUrlPathForId(int tenantId, int urlId);

    [RequireNamedArgs]
    bool HasAccess(int userId, int tenantId, string path);

    [RequireNamedArgs]
    bool CanEdit(Node node, int userId, int tenantId);

    [RequireNamedArgs]
    bool CanViewNodeAccess(int userId, int tenantId);

    int GetTenantId(Uri uri);

    int? GetIdForUrlPath(string urlPath, int tenantId);

    [RequireNamedArgs]
    IEnumerable<MenuItem> GetMenuItemsForUser(int userId, int tenantId);

    string? GetFrontPageText(int tenantId);

    string? GetLogo(int tenantId);

    string? GetSubTitle(int tenantId);

    string? GetFooterText(int tenantId);

    string? GetCssFile(int tenantId);

    string? GetIcoFile(int tenantId);

    string? GetTitle(int tenantId);
    string? GetDomainName(int tenantId);
    Task RefreshTenants();
}
