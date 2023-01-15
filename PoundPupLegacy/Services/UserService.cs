using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Npgsql;
using PoundPupLegacy.ViewModel;
using System.Collections;
using System.Data;
using System.Security.Claims;

namespace PoundPupLegacy.Services;

public class UserService
{
    private readonly NpgsqlConnection _connection;

    private readonly Dictionary<int, List<MenuItem>> userMenus = new Dictionary<int, List<MenuItem>>();
    public UserService(NpgsqlConnection connection) 
    { 
        _connection = connection;

    }

    private int? GetUserId(ClaimsPrincipal cp)
    {
        var useIdText = cp.Claims.FirstOrDefault (x => x.Type == "user_id");
        if(useIdText == null)
        {
            return null;

        }
        return int.Parse(useIdText.Value);

    }

    public async Task<IEnumerable<MenuItem>> GetMenuItemsForUserId(ClaimsPrincipal cp)
    {
        var id = GetUserId(cp);
        if(!id.HasValue)
        {
            return Enumerable.Empty<MenuItem>();
        }
        if(userMenus.TryGetValue(id.Value, out var lst))
        {
            return lst;
        }
        else
        {
            var menuItems = await ReadMenuItemsForUserId(id.Value).ToListAsync();
            userMenus.Add(id.Value, menuItems);
            return menuItems;
        }
    }
    private async IAsyncEnumerable<MenuItem> ReadMenuItemsForUserId(int id)
    {
        await _connection.OpenAsync();
        var sql = $"""
            select
            nt.name || '/create' "path",
            'Create ' || nt.name "name"
            from (
            	select 
            	u.id 
            	FROM "user" u
            	WHERE u.id = @user_id
            	UNION
            	select 
            	uar.user_role_id
            	FROM user_group_user_role_user uar 
            	WHERE uar.user_id = @user_id AND uar.user_group_id = 1
            ) ar
            join access_role_privilege arp on arp.access_role_id = ar.id
            join create_node_action cna on cna.id = arp.action_id
            join node_type nt on nt.id = cna.node_type_id
            """;

        using var readCommand = _connection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;
        readCommand.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Integer);
        await readCommand.PrepareAsync();
        readCommand.Parameters["user_id"].Value = id;
        await using var reader = await readCommand.ExecuteReaderAsync();
        while(await reader.ReadAsync())
        {
            yield return new MenuItem { Name = reader.GetString("name"), Path = reader.GetString("path")};
        }
        await _connection.CloseAsync();
    }
}
