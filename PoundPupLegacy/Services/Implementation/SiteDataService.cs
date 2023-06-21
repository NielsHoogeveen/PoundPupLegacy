using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using PoundPupLegacy.Readers;
using PoundPupLegacy.ViewModel.Models;
using System.Data;
using System.Diagnostics;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class SiteDataService(
    IDbConnection connection,
    ILogger<SiteDataService> logger,
    IConfiguration configuration,
    IEnumerableDatabaseReaderFactory<NamedActionsReaderRequest, NamedAction> namedActionsReaderFactory,
    IEnumerableDatabaseReaderFactory<TenantsReaderRequest, Tenant> tenantsReaderFactory,
    IEnumerableDatabaseReaderFactory<TenantNodesReaderRequest, TenantNode> tenantNodesReaderFactory,
    IEnumerableDatabaseReaderFactory<MenuItemsReaderRequest, UserTenantMenuItems> menuItemsReaderFactory,
    IEnumerableDatabaseReaderFactory<UserTenantEditActionReaderRequest, UserTenantEditAction> userTenantEditActionReaderFactory,
    IEnumerableDatabaseReaderFactory<UserTenantEditOwnActionReaderRequest, UserTenantEditOwnAction> userTenantEditOwnActionReaderFactory,
    IEnumerableDatabaseReaderFactory<UserTenantActionReaderRequest, UserTenantAction> userTenantActionReaderFactory
) : DatabaseService(connection, logger), ISiteDataService
{
    private record Data
    {
        public required HashSet<NamedAction> NamedActions { get; init; }
        public required HashSet<UserTenantAction> UserTenantActions { get; init; }
        public required HashSet<UserTenantEditAction> UserTenantEditActions { get; init; }
        public required HashSet<UserTenantEditOwnAction> UserTenantEditOwnActions { get; init; }
        public required Dictionary<(int, int), List<MenuItem>> UserMenus { get; init; }
        public required List<Tenant> Tenants { get; init; }

    }
    private Data _data = new Data {
        Tenants = new List<Tenant>(),
        UserMenus = new Dictionary<(int, int), List<MenuItem>>(),
        NamedActions = new HashSet<NamedAction>(),
        UserTenantActions = new HashSet<UserTenantAction>(),
        UserTenantEditActions = new HashSet<UserTenantEditAction>(),
        UserTenantEditOwnActions = new HashSet<UserTenantEditOwnAction>()
    };

    /*
     * Assignment is atomic, therefore if we prepare all data in a single object,
     * we can reinitialize using a single assignment. As a result, from the 
     * users perspective reinitialization of site data is an atomic action
     */
    public async Task InitializeAsync()
    {
        logger.LogInformation("Loading site data");
        var data = new Data {
            NamedActions = await LoadNamedActionsAsync(),
            Tenants = await LoadTenantsAsync(),
            UserMenus = await LoadUserMenusAsync(),
            UserTenantActions = await LoadUserTenantActionsAsync(),
            UserTenantEditActions = await LoadUserTenantEditActionsAsync(),
            UserTenantEditOwnActions = await LoadUserTenantEditOwnActionsAsync(),
        };
        _data = data;
    }
    public (int, string) GetDefaultCountry(int tenantId)
    {
        var tenant = _data.Tenants.First(x => x.Id == tenantId);
        return (tenant.CountryIdDefault, tenant.CountryNameDefault);

    }
    public async Task RefreshTenants()
    {
        var data = new Data {
            Tenants = await LoadTenantsAsync(),
            NamedActions = _data.NamedActions,
            UserMenus = _data.UserMenus,
            UserTenantActions = _data.UserTenantActions,
            UserTenantEditActions = _data.UserTenantEditActions,
            UserTenantEditOwnActions = _data.UserTenantEditOwnActions,
        };
        _data = data;

    }

    public string? GetUrlPathForId(int tenantId, int urlId)
    {
        var tenant = _data.Tenants.Find(x => x.Id == tenantId);
        if (tenant is null) {
            throw new NullReferenceException("Tenant should not be null");
        }
        if (tenant.IdToUrl.TryGetValue(urlId, out var urlPath)) {
            return urlPath;
        }
        return null;
    }

    public bool HasAccess(int userId, int tenantId, string path)
    {
        if (path == "/") {
            return true;
        }
        var userActions = _data.UserTenantActions.Where(x => x.UserId == userId && x.TenantId == tenantId);
        return _data.UserTenantActions.Contains(
            new UserTenantAction {
                UserId = userId,
                TenantId = tenantId,
                Action = path
            }
        );
    }

    public bool CanViewNodeAccess(int userId, int tenantId)
    {
        if(_data.NamedActions.Where(x => x.UserId == userId && x.TenantId == tenantId).Any()) {
            return true;
        }
        return false;
    }

    public int GetTenantId(Uri uri)
    {
        var domainName = uri.Host;
        if(domainName == "184.107.108.149" && uri.Port == 80) {
            return 1;
        }
        if(domainName == "184.107.108.149" && uri.Port == 81) {
            return 6;
        }
        if (domainName == "localhost") {
            var localHostTenantString = configuration["LocalHostTenant"];
            if (localHostTenantString is null) {
                logger.LogError("Local host tenant is not defined in appsettings.json");
                return 1;
            }
            if (int.TryParse(localHostTenantString, out int localHostTenant)) {
                if (_data.Tenants.Find(x => x.Id == localHostTenant) is not null) {
                    return localHostTenant;
                }
                return 1;
            }
        }
        var tenant = _data.Tenants.Find(x => x.DomainName == domainName);
        if (tenant is not null) {
            return tenant.Id;
        }
        return 1;
    }

    public int? GetIdForUrlPath(string urlPath, int tenantId)
    {
        var tenant = _data.Tenants.Find(x => x.Id == tenantId);
        if (tenant is null) {
            throw new NullReferenceException("Tenant should not be null");
        }
        if (tenant.UrlToId.TryGetValue(urlPath[1..], out var urlId)) {
            return urlId;
        }
        return null;
    }


    public int? GetIdForUrlPath(HttpRequest httpRequest)
    {
        var tenantId = GetTenantId(httpRequest.GetUri());
        var urlPath = httpRequest.Path.Value![1..];
        var tenant = _data.Tenants.Find(x => x.Id == tenantId);
        if (tenant is null) {
            throw new NullReferenceException("Tenant should not be null");
        }
        if (tenant.UrlToId.TryGetValue(urlPath, out var urlId)) {
            return urlId;
        }
        return null;
    }

    private async Task<List<Tenant>> LoadTenantsAsync()
    {
        var tenants = new List<Tenant>();
        var sw = Stopwatch.StartNew();

        return await WithConnection(async (connection) => {
            await using var tenantsReader = await tenantsReaderFactory.CreateAsync(connection);
            await foreach (var tenant in tenantsReader.ReadAsync(new TenantsReaderRequest())) {
                tenants.Add(tenant);
            }
            await using var tenantNodesReader = await tenantNodesReaderFactory.CreateAsync(connection);
            await foreach (var tenantNode in tenantNodesReader.ReadAsync(new TenantNodesReaderRequest())) {
                var tenant = tenants.Find(x => x.Id == tenantNode.TenantId);
                if (tenant is null) {
                    throw new NullReferenceException("Tenant should not be null");
                }
                tenant.UrlToId.Add(tenantNode.UrlPath, tenantNode.UrlId);
                tenant.IdToUrl.Add(tenantNode.UrlId, tenantNode.UrlPath);
            }
            logger.LogInformation($"Loaded tenant urls in {sw.ElapsedMilliseconds}ms");
            return tenants;
        });
    }

    private async Task<Dictionary<(int, int), List<Models.MenuItem>>> LoadUserMenusAsync()
    {
        var sw = Stopwatch.StartNew();
        var userMenus = new Dictionary<(int, int), List<Models.MenuItem>>();
        return await WithConnection(async (connection) => {
            await using var reader = await menuItemsReaderFactory.CreateAsync(connection);
            await foreach (var item in reader.ReadAsync(new MenuItemsReaderRequest())) {
                userMenus.Add((item.UserId, item.TenantId), item.MenuItems);
            }
            logger.LogInformation($"Loaded user menus in {sw.ElapsedMilliseconds}ms");
            return userMenus;
        });
    }

    private async Task<HashSet<NamedAction>> LoadNamedActionsAsync()
    {
        var sw = Stopwatch.StartNew();
        var namedActions = new HashSet<NamedAction>();
        return await WithConnection(async (connection) => {
            await using var reader = await namedActionsReaderFactory.CreateAsync(connection);
            await foreach (var item in reader.ReadAsync(new NamedActionsReaderRequest())) {
                namedActions.Add(item);
            }
            logger.LogInformation($"Loaded named actions in {sw.ElapsedMilliseconds}ms");
            return namedActions;
        });
    }

    private async Task<HashSet<UserTenantEditAction>> LoadUserTenantEditActionsAsync()
    {
        var sw = Stopwatch.StartNew();
        var userTenantActions = new HashSet<UserTenantEditAction>();
        return await WithConnection(async (connection) => {
            await using var reader = await userTenantEditActionReaderFactory.CreateAsync(connection);
            await foreach (var item in reader.ReadAsync(new UserTenantEditActionReaderRequest())) {
                userTenantActions.Add(item);
            }
            logger.LogInformation($"Loaded user privileges in {sw.ElapsedMilliseconds}ms");
            return userTenantActions;
        });
    }
    private async Task<HashSet<UserTenantEditOwnAction>> LoadUserTenantEditOwnActionsAsync()
    {
        var sw = Stopwatch.StartNew();
        var userTenantActions = new HashSet<UserTenantEditOwnAction>();
        return await WithConnection(async (connection) => {
            await using var reader = await userTenantEditOwnActionReaderFactory.CreateAsync(connection);
            await foreach (var item in reader.ReadAsync(new UserTenantEditOwnActionReaderRequest())) {
                userTenantActions.Add(item);
            }
            logger.LogInformation($"Loaded user privileges in {sw.ElapsedMilliseconds}ms");
            return userTenantActions;
        });
    }
    private async Task<HashSet<UserTenantAction>> LoadUserTenantActionsAsync()
    {
        var sw = Stopwatch.StartNew();
        var userTenantActions = new HashSet<UserTenantAction>();
        return await WithConnection(async (connection) => {
            await using var reader = await userTenantActionReaderFactory.CreateAsync(connection);
            await foreach (var item in reader.ReadAsync(new UserTenantActionReaderRequest())) {
                userTenantActions.Add(item);
            }
            logger.LogInformation($"Loaded user privileges in {sw.ElapsedMilliseconds}ms");
            return userTenantActions;
        });
    }

    public IEnumerable<MenuItem> GetMenuItemsForUser(int userId, int tenantId)
    {
        if (_data.UserMenus.TryGetValue((userId, tenantId), out var lst)) {
            return lst;
        }
        else {
            return new List<MenuItem>();
        }
    }
    public bool CanEdit(Node node, int userId, int tenantId)
    {
        if (_data.UserTenantEditActions.Contains(new UserTenantEditAction { UserId = userId, TenantId = tenantId, NodeTypeId = node.NodeTypeId })) {
            return true;
        }
        if (node.Authoring.Id == userId) {
            if (_data.UserTenantEditOwnActions.Contains(new UserTenantEditOwnAction { UserId = userId, TenantId = tenantId, NodeTypeId = node.NodeTypeId })) {
                return true;
            }
        }
        return false;
    }

    public string? GetLogo(int tenantId)
    {
        return _data.Tenants.First(x => x.Id == tenantId).Logo;
    }

    public string? GetSubTitle(int tenantId)
    {
        return _data.Tenants.First(x => x.Id == tenantId).SubTitle;
    }

    public string? GetFooterText(int tenantId)
    {
        return _data.Tenants.First(x => x.Id == tenantId).FooterText;
    }
    public string? GetFrontPageText(int tenantId)
    {
        return _data.Tenants.First(x => x.Id == tenantId).FrontPageText;
    }
    public string? GetCssFile(int tenantId)
    {
        return _data.Tenants.First(x => x.Id == tenantId).CssFile;
    }

}
