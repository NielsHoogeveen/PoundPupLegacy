using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;

namespace PoundPupLegacy.Readers;

using Request = UserTenantActionReaderRequest;
using Factory = UserTenantActionReaderFactory;
using Reader = UserTenantActionReader;

public sealed record UserTenantActionReaderRequest : IRequest
{
}
internal sealed class UserTenantActionReaderFactory : EnumerableDatabaseReaderFactory<Request, UserTenantAction, Reader>
{
    internal static readonly IntValueReader UserIdReader = new() { Name = "user_id" };
    internal static readonly IntValueReader TenantIdReader = new() { Name = "tenant_id" };
    internal static readonly StringValueReader ActionReader = new() { Name = "action" };

    public override string Sql => SQL;

    const string SQL = """
        select
            distinct
            ugur.user_id,
            t.id tenant_id,
            ba.path action
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
internal sealed class UserTenantActionReader : EnumerableDatabaseReader<Request, UserTenantAction>
{
    public UserTenantActionReader(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] { };
    }

    protected override UserTenantAction Read(NpgsqlDataReader reader)
    {
        return new UserTenantAction {
            UserId = Factory.UserIdReader.GetValue(reader),
            TenantId = Factory.TenantIdReader.GetValue(reader),
            Action = Factory.ActionReader.GetValue(reader),
        };
    }
}
