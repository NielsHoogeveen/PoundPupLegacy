﻿using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;

namespace PoundPupLegacy.Readers;

using Request = UserTenantEditOwnActionReaderRequest;
using Factory = UserTenantEditOwnActionReaderFactory;
using Reader = UserTenantEditOwnActionReader;

public sealed record UserTenantEditOwnActionReaderRequest : IRequest
{
}

internal sealed class UserTenantEditOwnActionReaderFactory : EnumerableDatabaseReaderFactory<Request, UserTenantEditOwnAction, Reader>
{

    internal static readonly IntValueReader UserIdReader = new() { Name = "user_id" };
    internal static readonly IntValueReader TenantIdReader = new() { Name = "tenant_id" };
    internal static readonly IntValueReader NodeTypeIdReader = new() { Name = "node_type_id" };

    public override string Sql => SQL;

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
internal sealed class UserTenantEditOwnActionReader : EnumerableDatabaseReader<Request, UserTenantEditOwnAction>
{
    public UserTenantEditOwnActionReader(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return Array.Empty<ParameterValue>();
    }

    protected override UserTenantEditOwnAction Read(NpgsqlDataReader reader)
    {
        return new UserTenantEditOwnAction {
            UserId = Factory.UserIdReader.GetValue(reader),
            TenantId = Factory.TenantIdReader.GetValue(reader),
            NodeTypeId = Factory.NodeTypeIdReader.GetValue(reader),
        };
    }
}
