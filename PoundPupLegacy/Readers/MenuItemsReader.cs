using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using System.Data;
using System.Data.Common;

namespace PoundPupLegacy.Readers;
public class MenuItemsReaderFactory : IDatabaseReaderFactory<MenuItemsReader>
{
    public async Task<MenuItemsReader> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;
        var command = postgresConnection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = SQL;
        await command.PrepareAsync();
        return new MenuItemsReader(command);
    }

    const string SQL = """
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
        jsonb_build_object(
            'UserId', 
            user_id,
            'TenantId', 
            tenant_id,
            'MenuItems', 
            jsonb_agg(
        	    jsonb_build_object(
        		    'Path', path,
        		    'Name', "name"
        	    )
            )
        
        ) document
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


}

public class MenuItemsReader : EnumerableDatabaseReader<MenuItemsReader.Request, UserTenantMenuItems>
{
    public record Request
    {
    }
    internal MenuItemsReader(NpgsqlCommand command) : base(command)
    {
    }
    public override async IAsyncEnumerable<UserTenantMenuItems> ReadAsync(Request request)
    {
        await using var reader = await _command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            yield return reader.GetFieldValue<UserTenantMenuItems>(0);
        }
    }

}
