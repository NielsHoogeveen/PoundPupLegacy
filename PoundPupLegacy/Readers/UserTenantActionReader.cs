using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using System.Data;

namespace PoundPupLegacy.Readers;
public class UserTenantActionReaderFactory : IDatabaseReaderFactory<UserTenantActionReader>
{
    public async Task<UserTenantActionReader> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;
        var command = postgresConnection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = SQL;
        await command.PrepareAsync();
        return new UserTenantActionReader(command);
    }
    const string SQL = """
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

}
public class UserTenantActionReader : EnumerableDatabaseReader<UserTenantActionReader.Request, UserTenantAction>
{
    public record Request
    {

    }
    internal UserTenantActionReader(NpgsqlCommand command) : base(command)
    {
    }
    public override async IAsyncEnumerable<UserTenantAction> ReadAsync(Request request)
    {
        await using var reader = await _command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            var userId = reader.GetInt32(0);
            var tenantId = reader.GetInt32(1);
            var action = reader.GetFieldValue<string>(2);
            yield return new UserTenantAction {
                UserId = userId,
                TenantId = tenantId,
                Action = action,
            };
        }

    }


}
