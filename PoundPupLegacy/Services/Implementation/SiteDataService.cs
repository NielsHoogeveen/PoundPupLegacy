using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using PoundPupLegacy.Readers;
using PoundPupLegacy.ViewModel.Models;
using System.Data;
using System.Diagnostics;
using static Microsoft.FSharp.Core.ByRefKinds;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class SiteDataService : ISiteDataService
{
    private record Data
    {
        public required HashSet<UserTenantAction> UserTenantActions { get; init; }

        public required HashSet<UserTenantEditAction> UserTenantEditActions { get; init; }
        public required HashSet<UserTenantEditOwnAction> UserTenantEditOwnActions { get; init; }

        public required Dictionary<(int, int), List<MenuItem>> UserMenus { get; init; }

        public required List<Tenant> Tenants { get; init; }

    }
    private Data _data = new Data {
        Tenants = new List<Tenant>(),
        UserMenus = new Dictionary<(int, int), List<MenuItem>>(),
        UserTenantActions = new HashSet<UserTenantAction>(),
        UserTenantEditActions = new HashSet<UserTenantEditAction>(),
        UserTenantEditOwnActions = new HashSet<UserTenantEditOwnAction>()
    };

    private readonly NpgsqlConnection _connection;
    private readonly ILogger<SiteDataService> _logger;
    private readonly IEnumerableDatabaseReaderFactory<TenantsReaderRequest, Tenant> _tenantsReaderFactory;
    private readonly IEnumerableDatabaseReaderFactory<TenantNodesReaderRequest, TenantNode> _tenantNodesReaderFactory;
    private readonly IEnumerableDatabaseReaderFactory<MenuItemsReaderRequest, UserTenantMenuItems> _menuItemsReaderFactory;
    private readonly IEnumerableDatabaseReaderFactory<UserTenantEditActionReaderRequest, UserTenantEditAction> _userTenantEditActionReaderFactory;
    private readonly IEnumerableDatabaseReaderFactory<UserTenantEditOwnActionReaderRequest, UserTenantEditOwnAction> _userTenantEditOwnActionReaderFactory;
    private readonly IEnumerableDatabaseReaderFactory<UserTenantActionReaderRequest, UserTenantAction> _userTenantActionReaderFactory;



    public SiteDataService(
        IDbConnection connection,
        ILogger<SiteDataService> logger,
        IEnumerableDatabaseReaderFactory<TenantsReaderRequest, Tenant> tenantsReaderFactory,
        IEnumerableDatabaseReaderFactory<TenantNodesReaderRequest, TenantNode> tenantNodesReaderFactory,
        IEnumerableDatabaseReaderFactory<MenuItemsReaderRequest, UserTenantMenuItems> menuItemsReaderFactory,
        IEnumerableDatabaseReaderFactory<UserTenantEditActionReaderRequest, UserTenantEditAction> userTenantEditActionReaderFactory,
        IEnumerableDatabaseReaderFactory<UserTenantEditOwnActionReaderRequest, UserTenantEditOwnAction> userTenantEditOwnActionReaderFactory,
        IEnumerableDatabaseReaderFactory<UserTenantActionReaderRequest, UserTenantAction> userTenantActionReaderFactory)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _logger = logger;
        _tenantsReaderFactory = tenantsReaderFactory;
        _tenantNodesReaderFactory = tenantNodesReaderFactory;
        _menuItemsReaderFactory = menuItemsReaderFactory;
        _userTenantEditActionReaderFactory = userTenantEditActionReaderFactory;
        _userTenantEditOwnActionReaderFactory = userTenantEditOwnActionReaderFactory;
        _userTenantActionReaderFactory = userTenantActionReaderFactory;
    }

    /*
     * Assignment is atomic, therefore if we prepare all data in a single object,
     * we can reinitialize using a single assignment. As a result, from the 
     * users perspective reinitialization of site data is an atomic action
     */
    public async Task InitializeAsync()
    {
        _logger.LogInformation("Loading site data");
        var data = new Data {
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
        return _data.UserTenantActions.Contains(
            new UserTenantAction {
                UserId = userId,
                TenantId = tenantId,
                Action = path
            }
        );
    }

    public int GetTenantId(Uri uri)
    {
        var domainName = uri.Host;
        if (domainName == "localhost:7141") {
            return 1;
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

        try {
            await _connection.OpenAsync();
            await using var tenantsReader = await _tenantsReaderFactory.CreateAsync(_connection);
            await foreach (var tenant in tenantsReader.ReadAsync(new TenantsReaderRequest())) {
                tenants.Add(tenant);
            }
            await using var tenantNodesReader = await _tenantNodesReaderFactory.CreateAsync(_connection);
            await foreach (var tenantNode in tenantNodesReader.ReadAsync(new TenantNodesReaderRequest())) {
                var tenant = tenants.Find(x => x.Id == tenantNode.TenantId);
                if (tenant is null) {
                    throw new NullReferenceException("Tenant should not be null");
                }
                tenant.UrlToId.Add(tenantNode.UrlPath, tenantNode.UrlId);
                tenant.IdToUrl.Add(tenantNode.UrlId, tenantNode.UrlPath);
            }
            _logger.LogInformation($"Loaded tenant urls in {sw.ElapsedMilliseconds}ms");
            return tenants;
        }
        finally {
            await _connection.CloseAsync();
        }
    }

    private async Task<Dictionary<(int, int), List<Models.MenuItem>>> LoadUserMenusAsync()
    {
        var sw = Stopwatch.StartNew();
        var userMenus = new Dictionary<(int, int), List<Models.MenuItem>>();
        try {
            await _connection.OpenAsync();
            await using var reader = await _menuItemsReaderFactory.CreateAsync(_connection);
            await foreach (var item in reader.ReadAsync(new MenuItemsReaderRequest())) {
                userMenus.Add((item.UserId, item.TenantId), item.MenuItems);
            }



            _logger.LogInformation($"Loaded user menus in {sw.ElapsedMilliseconds}ms");
            return userMenus;
        }
        finally {
            await _connection.CloseAsync();
        }
    }
    private async Task<HashSet<UserTenantEditAction>> LoadUserTenantEditActionsAsync()
    {
        var sw = Stopwatch.StartNew();
        var userTenantActions = new HashSet<UserTenantEditAction>();
        try {
            await _connection.OpenAsync();
            await using var reader = await _userTenantEditActionReaderFactory.CreateAsync(_connection);
            await foreach (var item in reader.ReadAsync(new UserTenantEditActionReaderRequest())) {
                userTenantActions.Add(item);
            }
            _logger.LogInformation($"Loaded user privileges in {sw.ElapsedMilliseconds}ms");
            return userTenantActions;
        }
        finally {
            await _connection.CloseAsync();
        }
    }
    private async Task<HashSet<UserTenantEditOwnAction>> LoadUserTenantEditOwnActionsAsync()
    {
        var sw = Stopwatch.StartNew();
        var userTenantActions = new HashSet<UserTenantEditOwnAction>();
        try {
            await _connection.OpenAsync();
            await using var reader = await _userTenantEditOwnActionReaderFactory.CreateAsync(_connection);
            await foreach (var item in reader.ReadAsync(new UserTenantEditOwnActionReaderRequest())) {
                userTenantActions.Add(item);
            }
            _logger.LogInformation($"Loaded user privileges in {sw.ElapsedMilliseconds}ms");
            return userTenantActions;
        }
        finally {
            await _connection.CloseAsync();
        }
    }
    private async Task<HashSet<UserTenantAction>> LoadUserTenantActionsAsync()
    {
        var sw = Stopwatch.StartNew();
        var userTenantActions = new HashSet<UserTenantAction>();
        try {
            await _connection.OpenAsync();
            await using var reader = await _userTenantActionReaderFactory.CreateAsync(_connection);
            await foreach (var item in reader.ReadAsync(new UserTenantActionReaderRequest())) {
                userTenantActions.Add(item);
            }
            _logger.LogInformation($"Loaded user privileges in {sw.ElapsedMilliseconds}ms");
            return userTenantActions;
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
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

    public string GetLayout(int userId, int tenantId)
    {
        var signedIn = userId != 0;

        return (tenantId, signedIn) switch {
            (1, false) => "_LayoutPPL",
            (1, true) => "_LayoutPPL",
            (6, false) => "_LayoutCPCT",
            (6, true) => "_LayoutCPCT",
            _ => "_LayoutPPL"
        };
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

    public string GetLogoName(int tenantId)
    {
        if(tenantId == 1) {
            return "PPL";
        }
        else if(tenantId == 6) {
            return "CPCT";
        }
        else {
            return "";
        }
    }

    public string GetSubTitle(int tenantId)
    {
        if(tenantId == 1) {
            return "exposing the dark side of adoption";
        }
        return "";
    }

    public Link[] GetFooterMenuItems(int tenantId)
    {
        if(tenantId == 1) {
            return new Link[] {
                new BasicLink { Title = "About", Path = "/about_us" },
                new BasicLink { Title = "Our Position", Path = "/our_position" },
                new BasicLink { Title = "FAQ", Path = "/faq" },
                new BasicLink { Title = "Ways to help", Path = "/ways_to_help" },
                new BasicLink { Title = "Contact", Path = "/contact" },
            };
        }
        return Array.Empty<Link>();
    }

    public string GetFooterTitle(int tenantId)
    {
        if(tenantId == 1) {
            return "Pound Pup Legacy";
        }
        return "";
    }
}
