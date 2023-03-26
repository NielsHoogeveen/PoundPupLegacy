using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using System.Data;

namespace PoundPupLegacy.Readers;

public class UserTenantEditOwnActionReader: DatabaseReader, IEnumerableDatabaseReader<UserTenantEditOwnActionReader, UserTenantEditOwnActionReader.UserTenantEditOwnActionRequest, UserTenantEditOwnAction>
{
    public record UserTenantEditOwnActionRequest
    {

    }
    private UserTenantEditOwnActionReader(NpgsqlCommand command) : base(command)
    {
    }
    public async IAsyncEnumerable<UserTenantEditOwnAction> ReadAsync(UserTenantEditOwnActionRequest request)
    {
        await using var reader = await _command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            var userId = reader.GetInt32(0);
            var tenantId = reader.GetInt32(1);
            var nodeTypeId = reader.GetInt32(2);
            yield return  new UserTenantEditOwnAction {
                UserId = userId,
                TenantId = tenantId,
                NodeTypeId = nodeTypeId,
            };
        }

    }
    public static async Task<UserTenantEditOwnActionReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = SQL;
        await command.PrepareAsync();
        return new UserTenantEditOwnActionReader(command);
    }
    const string SQL = """
        select
            distinct
            *
        from
        (
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


}
