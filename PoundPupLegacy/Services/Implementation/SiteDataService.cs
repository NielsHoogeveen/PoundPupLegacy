using PoundPupLegacy.Models;
using PoundPupLegacy.ViewModel.Models;
using System.Data;
using User = PoundPupLegacy.Models.User;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class SiteDataService(
    ISiteDataLoader siteDataLoader
) : ISiteDataService
{
    SiteData siteData = default!;

    public (int, string) GetDefaultCountry(int tenantId)
    {
        return (siteData.Tenant.CountryIdDefault, siteData.Tenant.CountryNameDefault);

    }
    public async Task<bool> InitializeAsync()
    {
        try {
            siteData = await siteDataLoader.GetSiteData();
            return true;
        }catch(LoadException) {
            return false;
        }

    }

    public async Task RefreshTenants()
    {
        siteData = await siteDataLoader.GetSiteData();

    }

    public string? GetUrlPathForId(int urlId)
    {
        if (siteData.Tenant.IdToUrl.TryGetValue(urlId, out var urlPath)) {
            return urlPath;
        }
        return null;
    }

    public int? GetUserByNameIdentifier(string id)
    {
        return siteData.Users.FirstOrDefault(x => x.Value.NameIdentifier == id).Value?.Id;
    }

    private async Task<User?> GetUser(int userId)
    {
        if (siteData.Users.TryGetValue(userId, out var user)) {

            return user;
        }
        else {
            try {
                var loadedUser = await siteDataLoader.LoadUser(siteData.Tenant.Id, userId);
                siteData.Users.Add(userId, loadedUser);
                return loadedUser;
            }
            catch (LoadException) {
                return null;
            }
        }
    }

    public async Task<bool> HasAccess(int userId, string path)
    {
        if (path == "/") {
            return true;
        }
        var user = await GetUser(userId);
        if (user is not null) {

            return user!.Actions.Contains(new UserAction { Path = path });
        }
        else {

        }
        return false;
    }

    public async Task<bool> CanViewNodeAccess(int userId)
    {
        var user = await GetUser(userId);
        if (user is not null) {

            return user!.NamedActions.Contains(new UserNamedAction { Name = "nodeaccess" });
        }
        return false;

    }

    public int? GetIdForUrlPath(string urlPath)
    {
        if (siteData.Tenant.UrlToId.TryGetValue(urlPath[1..], out var urlId)) {
            return urlId;
        }
        return null;
    }
    public int GetTenantId()
    {
        return siteData.Tenant.Id;
    }

    public int? GetIdForUrlPath(HttpRequest httpRequest)
    {
        var urlPath = httpRequest.Path.Value![1..];
        if (siteData.Tenant.UrlToId.TryGetValue(urlPath, out var urlId)) {
            return urlId;
        }
        return null;
    }


    public async Task<List<MenuItem>> GetMenuItemsForUser(int userId)
    {
        var user = await GetUser(userId);
        if (user is not null) {
            return user.MenuItems;
        }
        return new();
    }
    public async Task<bool> CanEdit(Node node, int userId)
    {
        var user = await GetUser(userId);
        if (user is not null) {
            if (user.EditActions.Where(x => x.NodeTypeId == node.NodeTypeId).Any()) {
                return true;
            }else if (user.EditOwnActions.Where(x => x.NodeTypeId == node.NodeTypeId).Any()) {
                return true;
            }
            return false;
        }
        return false;
    }

    public string? GetLogo()
    {
        return siteData.Tenant.Logo;
    }

    public string? GetSubTitle()
    {
        return siteData.Tenant.Subtitle;
    }

    public string? GetFooterText()
    {
        return siteData.Tenant.FooterText;
    }
    public string? GetFrontPageText()
    {
        return siteData.Tenant.FrontPageText;
    }
    public string? GetCssFile()
    {
        return siteData.Tenant.CssFile;
    }
    public string? GetIcoFile()
    {
        return $"{GetTenantId()}.ico";
    }
    public string? GetTitle()
    {
        return siteData.Tenant.Title;
    }

    public string? GetDomainName()
    {
        return siteData.Tenant.DomainName;
    }
    public string? GetName()
    {
        return siteData.Tenant.Name;
    }
    public string? GetGoogleAnalyticsMearurementId()
    {
        return siteData.Tenant.GoogleAnalyticsMeasurementId;
    }

    public int GetFrontPageId()
    {
        return siteData.Tenant.FrontPageId;
    }
    public SmtpConnection GetSmtpConnection()
    {
        return siteData.Tenant.SmtpConnection;
    }
}
