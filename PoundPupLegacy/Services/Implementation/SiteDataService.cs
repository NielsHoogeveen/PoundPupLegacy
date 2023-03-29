﻿using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using PoundPupLegacy.Readers;
using PoundPupLegacy.ViewModel;
using System.Data;
using System.Diagnostics;

namespace PoundPupLegacy.Services.Implementation;

internal class SiteDataService : ISiteDataService
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
    private readonly IDatabaseReaderFactory<TenantsReader> _tenantsReaderFactory;
    private readonly IDatabaseReaderFactory<TenantNodesReader> _tenantNodesReaderFactory;
    private readonly IDatabaseReaderFactory<MenuItemsReader> _menuItemsReaderFactory;
    private readonly IDatabaseReaderFactory<UserTenantEditActionReader> _userTenantEditActionReaderFactory;
    private readonly IDatabaseReaderFactory<UserTenantEditOwnActionReader> _userTenantEditOwnActionReaderFactory;
    private readonly IDatabaseReaderFactory<UserTenantActionReader> _userTenantActionReaderFactory;



    public SiteDataService(
        NpgsqlConnection connection,
        ILogger<SiteDataService> logger,
        IDatabaseReaderFactory<TenantsReader> tenantsReaderFactory,
        IDatabaseReaderFactory<TenantNodesReader> tenantNodesReaderFactory,
        IDatabaseReaderFactory<MenuItemsReader> menuItemsReaderFactory,
        IDatabaseReaderFactory<UserTenantEditActionReader> userTenantEditActionReaderFactory,
        IDatabaseReaderFactory<UserTenantEditOwnActionReader> userTenantEditOwnActionReaderFactory,
        IDatabaseReaderFactory<UserTenantActionReader> userTenantActionReaderFactory)
    {
        _connection = connection;
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

    public bool HasAccess(int userId, int tenantId, HttpRequest request)
    {
        return _data.UserTenantActions.Contains(
            new UserTenantAction {
                UserId = userId,
                TenantId = tenantId,
                Action = request.Path
            }
        );
    }

    public int GetTenantId(HttpRequest httpRequest)
    {
        var domainName = httpRequest.Host.Value;
        if (domainName == "localhost:7141") {
            return 1;
        }
        var tenant = _data.Tenants.Find(x => x.DomainName == domainName);
        if (tenant is not null) {
            return tenant.Id;
        }
        return 1;
    }

    public int? GetIdForUrlPath(HttpRequest httpRequest)
    {
        var tenantId = GetTenantId(httpRequest);
        var urlPath = httpRequest.Path.Value!.Substring(1);
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
            await foreach (var tenant in tenantsReader.ReadAsync(new TenantsReader.Request())) {
                tenants.Add(tenant);
            }
            await using var tenantNodesReader = await _tenantNodesReaderFactory.CreateAsync(_connection);
            await foreach (var tenantNode in tenantNodesReader.ReadAsync(new TenantNodesReader.Request())) {
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
            await foreach (var item in reader.ReadAsync(new MenuItemsReader.Request())) {
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
            await foreach (var item in reader.ReadAsync(new UserTenantEditActionReader.Request())) {
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
            await foreach (var item in reader.ReadAsync(new UserTenantEditOwnActionReader.Request())) {
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
            await foreach (var item in reader.ReadAsync(new UserTenantActionReader.Request())) {
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
}
