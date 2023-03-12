using Npgsql;
using PoundPupLegacy.ViewModel;
using System.Data;
using System.Diagnostics;
using MenuItem = PoundPupLegacy.ViewModel.Link;
using Tenant = PoundPupLegacy.ViewModel.Tenant;

namespace PoundPupLegacy.Services.Implementation;

internal class SiteDataService : ISiteDataService
{
    private record UserTenantAction
    {
        public required int UserId { get; init; }
        public required int TenantId { get; init; }
        public required string Action { get; init; }
    }
    private record UserTenantEditAction
    {
        public required int UserId { get; init; }
        public required int TenantId { get; init; }
        public required int NodeTypeId { get; init; }
    }
    private record UserTenantEditOwnAction
    {
        public required int UserId { get; init; }
        public required int TenantId { get; init; }
        public required int NodeTypeId { get; init; }
    }

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

    public SiteDataService(
        NpgsqlConnection connection,
        ILogger<SiteDataService> logger)
    {
        _connection = connection;
        _logger = logger;
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
            using (var readCommand = _connection.CreateCommand()) {
                var sql = $"""
            select
            t.id,
            t.domain_name
            from tenant t
            """;
                readCommand.CommandType = CommandType.Text;
                readCommand.CommandTimeout = 300;
                readCommand.CommandText = sql;
                await readCommand.PrepareAsync();
                await using var reader = await readCommand.ExecuteReaderAsync();
                while (await reader.ReadAsync()) {
                    var tenantId = reader.GetInt32(0);
                    var domainName = reader.GetString(1);
                    tenants.Add(new Tenant {
                        Id = tenantId,
                        DomainName = domainName,
                        IdToUrl = new Dictionary<int, string>(),
                        UrlToId = new Dictionary<string, int>()
                    });
                }
            }
            using (var readCommand = _connection.CreateCommand()) {
                var sql = $"""
                    select
                    tn.tenant_id,
                    tn.url_id,
                    tn.url_path
                    from tenant_node tn 
                    where tn.url_path is not null
                    """;
                readCommand.CommandType = CommandType.Text;
                readCommand.CommandTimeout = 300;
                readCommand.CommandText = sql;
                await readCommand.PrepareAsync();
                await using var reader = await readCommand.ExecuteReaderAsync();
                while (await reader.ReadAsync()) {
                    var tenant = tenants.Find(x => x.Id == reader.GetInt32(0));
                    if (tenant is null) {
                        throw new NullReferenceException("Tenant should not be null");
                    }
                    tenant.UrlToId.Add(reader.GetString(2), reader.GetInt32(1));
                    tenant.IdToUrl.Add(reader.GetInt32(1), reader.GetString(2));
                }
            }

            _logger.LogInformation($"Loaded tenant urls in {sw.ElapsedMilliseconds}ms");
            return tenants;
        }
        finally {
            await _connection.CloseAsync();
        }

    }

    private async Task<Dictionary<(int, int), List<MenuItem>>> LoadUserMenusAsync()
    {
        var sw = Stopwatch.StartNew();
        var userMenus = new Dictionary<(int, int), List<MenuItem>>();
        try {
            await _connection.OpenAsync();
            var sql = $"""
            with 
            user_action as (
            	select
            	uar.user_id,
            	uar.tenant_id,	
            	arp.action_id
            	from(
            		select
            		distinct
            		u.id user_id,
            		u.id access_role_id,
            		t.id tenant_id
            		from "user" u
            		join user_group_user_role_user ug on ug.user_id = u.id
            		join tenant t on t.id = ug.user_group_id
            		union
            		select
            		u.id user_id,
            		ug.user_role_id access_role_id,
            		t.id tenant_id
            		from "user" u
            		JOIN user_group_user_role_user ug on ug.user_id = u.id
            		join tenant t on t.id = ug.user_group_id
            		union
            		select
            		0,
            		t.access_role_id_not_logged_in,
            		t.id tenant_id
            		from tenant t
            	) uar
            	join access_role_privilege arp on arp.access_role_id = uar.access_role_id
            )

            select
            user_id,
            tenant_id,
            json_agg(
            	json_build_object(
            		'Path', path,
            		'Name', "name"
            	)
            )
            from(
            	select
            	*
            	from(
            	select 
            	ua.user_id,
            	ua.tenant_id,
            	mi.weight,
            	case 
            		when ba.path is not null then ba.path
            		when cna.id is not null then '/' || replace(lower(nt.name), ' ', '_') || '/create'
            	end path,
            	ami.name
            	from user_action ua
            	join action_menu_item ami on ami.action_id = ua.action_id
            	join menu_item mi on mi.id = ami.id
            	left join basic_action ba on ba.id = ua.action_id
            	left join create_node_action cna on cna.id = ua.action_id
            	left join node_type nt on nt.id = cna.node_type_id
            	union
                select
            	distinct
            	ug.user_id,
            	tn.tenant_id,
            	weight,
            	case 
            		when tn.url_path is  null then '/node/' || tn.url_id
            		else '/' || tn.url_path
            	end	path,
            	tmi.name
            	from user_group_user_role_user ug 
            	join tenant_node tn on tn.tenant_id = ug.user_group_id
            	join tenant_node_menu_item tmi on tmi.tenant_node_id = tn.id
            	join menu_item mi on mi.id = tmi.id
            	union
                select
            	distinct
            	0 user_id,
            	tn.tenant_id,
            	weight,
            	case 
            		when tn.url_path is  null then '/node/' || tn.url_id
            		else '/' || tn.url_path
            	end	path,
            	tmi.name
            	from tenant_node tn 
            	join tenant_node_menu_item tmi on tmi.tenant_node_id = tn.id
            	join menu_item mi on mi.id = tmi.id
            	) a
            	order by user_id, tenant_id, weight 

            ) m
            group by user_id, tenant_id
            
            """;

            using var readCommand = _connection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;
            await readCommand.PrepareAsync();
            await using var reader = await readCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync()) {
                var user_id = reader.GetInt32(0);
                var tenant_id = reader.GetInt32(1);
                var menuItems = reader.GetFieldValue<List<Link>>(2);
                userMenus.Add((user_id, tenant_id), menuItems);
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
            var sql = """
            select
            distinct
            *
            from(
            select
            distinct
            ugur.user_id,
            t.id tenant_id,
            ba.node_type_id
            from edit_node_action ba
            join access_role_privilege arp on arp.action_id = ba.id
            join user_group_user_role_user ugur on ugur.user_role_id = arp.access_role_id
            join tenant t on t.id = ugur.user_group_id
            union
            select
            distinct
            0,
            t.id tenant_id,
            ba.node_type_id
            from edit_node_action ba
            join access_role_privilege arp on arp.action_id = ba.id
            join user_group_user_role_user ugur on ugur.user_role_id = arp.access_role_id
            join tenant t on t.id = ugur.user_group_id
            where arp.access_role_id = t.access_role_id_not_logged_in
            union
            select
            uguru.user_id,
            tn.id tenant_id,
            ba.node_type_id
            from edit_node_action ba
            join tenant tn on 1=1
            join user_group ug on ug.id = tn.id
            join user_group_user_role_user uguru on uguru.user_group_id = ug.id and uguru.user_role_id = ug.administrator_role_id
            ) x
            """;
            using var readCommand = _connection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;
            await readCommand.PrepareAsync();
            await using var reader = await readCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync()) {
                var userId = reader.GetInt32(0);
                var tenantId = reader.GetInt32(1);
                var nodeTypeId = reader.GetInt32(2);

                userTenantActions.Add(
                     new UserTenantEditAction {
                         UserId = userId,
                         TenantId = tenantId,
                         NodeTypeId = nodeTypeId,
                     }
                );
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
            var sql = """
            select
            distinct
            *
            from(
            select
            distinct
            ugur.user_id,
            t.id tenant_id,
            ba.node_type_id
            from edit_own_node_action ba
            join access_role_privilege arp on arp.action_id = ba.id
            join user_group_user_role_user ugur on ugur.user_role_id = arp.access_role_id
            join tenant t on t.id = ugur.user_group_id
            union
            select
            distinct
            0,
            t.id tenant_id,
            ba.node_type_id
            from edit_own_node_action ba
            join access_role_privilege arp on arp.action_id = ba.id
            join user_group_user_role_user ugur on ugur.user_role_id = arp.access_role_id
            join tenant t on t.id = ugur.user_group_id
            where arp.access_role_id = t.access_role_id_not_logged_in
            union
            select
            uguru.user_id,
            tn.id tenant_id,
            ba.node_type_id
            from edit_own_node_action ba
            join tenant tn on 1=1
            join user_group ug on ug.id = tn.id
            join user_group_user_role_user uguru on uguru.user_group_id = ug.id and uguru.user_role_id = ug.administrator_role_id
            ) x
            """;
            using var readCommand = _connection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;
            await readCommand.PrepareAsync();
            await using var reader = await readCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync()) {
                var userId = reader.GetInt32(0);
                var tenantId = reader.GetInt32(1);
                var nodeTypeId = reader.GetInt32(2);

                userTenantActions.Add(
                     new UserTenantEditOwnAction {
                         UserId = userId,
                         TenantId = tenantId,
                         NodeTypeId = nodeTypeId,
                     }
                );
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
            var sql = """
            select
            distinct
            ugur.user_id,
            t.id tenant_id,
            ba.path
            from basic_action ba
            join access_role_privilege arp on arp.action_id = ba.id
            join user_group_user_role_user ugur on ugur.user_role_id = arp.access_role_id
            join tenant t on t.id = ugur.user_group_id
            union
            select
            distinct
            0,
            t.id tenant_id,
            ba.path
            from basic_action ba
            join access_role_privilege arp on arp.action_id = ba.id
            join user_group_user_role_user ugur on ugur.user_role_id = arp.access_role_id
            join tenant t on t.id = ugur.user_group_id
            where arp.access_role_id = t.access_role_id_not_logged_in
            """;
            using var readCommand = _connection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;
            await readCommand.PrepareAsync();
            await using var reader = await readCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync()) {
                var userId = reader.GetInt32(0);
                var tenantId = reader.GetInt32(1);
                var action = reader.GetFieldValue<string>(2);

                userTenantActions.Add(
                     new UserTenantAction {
                         UserId = userId,
                         TenantId = tenantId,
                         Action = action,
                     }
                );
            }
            _logger.LogInformation($"Loaded user privileges in {sw.ElapsedMilliseconds}ms");
            return userTenantActions;
        }
        finally {
            await _connection.CloseAsync();
        }
    }

    public IEnumerable<Link> GetMenuItemsForUser(int userId, int tenantId)
    {
        if (_data.UserMenus.TryGetValue((userId, tenantId), out var lst)) {
            return lst;
        }
        else {
            return new List<Link>();
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
