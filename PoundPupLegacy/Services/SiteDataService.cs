using Npgsql;
using PoundPupLegacy.ViewModel;
using System.Data;
using System.Diagnostics;
using Tenant = PoundPupLegacy.ViewModel.Tenant;
using MenuItem = PoundPupLegacy.ViewModel.Link;

namespace PoundPupLegacy.Services;

public class SiteDataService
{
    private readonly NpgsqlConnection _connection;
    private readonly ILogger<SiteDataService> _logger;

    private readonly Dictionary<(int, int), List<MenuItem>> _userMenus = new Dictionary<(int, int), List<MenuItem>>();

    private readonly List<Tenant> _tenants = new List<Tenant>();
    public SiteDataService(NpgsqlConnection connection, ILogger<SiteDataService> logger) 
    { 
        _connection = connection;
        _logger = logger;

    }

    public int GetUserId(HttpContext context)
    {
        var cp = context.User;
        if (cp == null)
        {
            return 0;
        }
        var useIdText = cp.Claims.FirstOrDefault (x => x.Type == "user_id");
        if(useIdText == null)
        {
            return 0;

        }
        return int.Parse(useIdText.Value);

    }

    public async Task InitializeAsync()
    {
        _logger.LogInformation("Loading site data");
        await LoadTenants();
        await LoadUserMenusAsync();
    }

    public string? GetUrlPathForId(int tenantId, int urlId)
    {
        var tenant = _tenants.Find(x => x.Id == tenantId);
        if (tenant is null)
        {
            throw new NullReferenceException("Tenant should not be null");
        }
        if(tenant.IdToUrl.TryGetValue(urlId, out var urlPath))
        {
            return urlPath;
        }
        return null;
    }

    public int GetTenantId(HttpContext context)
    {
        var domainName = context.Request.Host.Value;
        if(domainName == "localhost:7141")
        {
            return 1;
        }
        var tenant = _tenants.Find(x => x.DomainName == domainName);
        if(tenant is not null)
        {
            return tenant.Id;
        }
        return 1;
    }

    public int? GetIdForUrlPath(HttpContext context)
    {

        var tenantId = GetTenantId(context);
        var urlPath = context.Request.Path.Value!.Substring(1);
        var tenant = _tenants.Find(x => x.Id == tenantId);
        if (tenant is null)
        {
            throw new NullReferenceException("Tenant should not be null");
        }
        if (tenant.UrlToId.TryGetValue(urlPath, out var urlId))
        {
            return urlId;
        }
        return null;
    }

    private async Task LoadTenants()
    {
        var sw = Stopwatch.StartNew();

        await _connection.OpenAsync();
        using (var readCommand = _connection.CreateCommand())
        {
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
            while (await reader.ReadAsync())
            {
                var tenantId = reader.GetInt32(0);
                var domainName = reader.GetString(1);
                _tenants.Add(new Tenant { Id = tenantId, DomainName = domainName, IdToUrl = new Dictionary<int, string>(), UrlToId = new Dictionary<string, int>() });
            }
        }
        using (var readCommand = _connection.CreateCommand())
        {
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
            while (await reader.ReadAsync())
            {
                var tenant = _tenants.Find(x => x.Id == reader.GetInt32(0));
                if(tenant is null)
                {
                    throw new NullReferenceException("Tenant should not be null");
                }
                tenant.UrlToId.Add(reader.GetString(2), reader.GetInt32(1));
                tenant.IdToUrl.Add(reader.GetInt32(1), reader.GetString(2));
            }
        }
        await _connection.CloseAsync();
        _logger.LogInformation($"Loaded tenant urls in {sw.ElapsedMilliseconds}ms");

    }

    private async Task LoadUserMenusAsync()
    {
        var sw = Stopwatch.StartNew();
        
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
            		t.user_role_id_not_logged_in,
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
        while (await reader.ReadAsync())
        {
            var user_id = reader.GetInt32(0);
            var tenant_id = reader.GetInt32(1);
            var menuItems = reader.GetFieldValue<List<Link>>(2);
            _userMenus.Add((user_id, tenant_id), menuItems);
        }
        await _connection.CloseAsync();
        _logger.LogInformation($"Loaded user menus in {sw.ElapsedMilliseconds}ms");
    }
    public IEnumerable<Link> GetMenuItemsForUser(HttpContext context)
    {
        if(_userMenus.TryGetValue((GetUserId(context), GetTenantId(context)), out var lst))
        {
            return lst;
        }
        else
        {
            return new List<Link>();
        }
    }
}
