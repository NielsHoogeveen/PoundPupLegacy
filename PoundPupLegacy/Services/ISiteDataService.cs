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

    int GetTenantId();

    int? GetIdForUrlPath(string urlPath);

    Task<List<MenuItem>> GetMenuItemsForUser(int userId);

    string? GetFrontPageText();

    int GetFrontPageId();

    string? GetLogo();

    string? GetSubTitle();

    string? GetFooterText();

    string? GetCssFile();

    string? GetIcoFile();

    string? GetTitle();
    string? GetDomainName();
    string? GetName();
    string? GetGoogleAnalyticsMearurementId();
    SmtpConnection GetSmtpConnection();
    Task RefreshTenants();
}
