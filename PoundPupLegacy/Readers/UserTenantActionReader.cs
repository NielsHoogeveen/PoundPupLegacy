﻿using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;

namespace PoundPupLegacy.Readers;

using Request = UserTenantActionReaderRequest;

public sealed record UserTenantActionReaderRequest : IRequest
{
}
internal sealed class UserTenantActionReaderFactory : EnumerableDatabaseReaderFactory<Request, UserTenantAction>
{
    private static readonly IntValueReader UserIdReader = new() { Name = "user_id" };
    private static readonly IntValueReader TenantIdReader = new() { Name = "tenant_id" };
    private static readonly StringValueReader ActionReader = new() { Name = "action" };

    public override string Sql => SQL;

    const string SQL = """
        select
        distinct
        *
        from(
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
        join user_role ur on ur.id = arp.access_role_id
        join tenant t on t.id = ur.user_group_id
        where arp.access_role_id = t.access_role_id_not_logged_in
        union
        select
        distinct
        ugur2.user_id,
        t.id tenant_id,
        ba.path
        from basic_action ba
        join tenant_action ta on ta.action_id = ba.id
        join user_group_user_role_user ugur on ugur.user_group_id = ta.tenant_id
        join user_group ug on ug.id = ugur.user_group_id
        join tenant t on t.id = ug.id
        join user_group_user_role_user ugur2 on ugur2.user_group_id = ug.id and ugur2.user_role_id = ug.administrator_role_id
        union
        select
        	distinct
        	ugur.user_id,
        	t.id tenant_id,
        	'/' || replace(nt.name, ' ', '_') || '/create' "action"
        from create_node_action cna
        join node_type nt on nt.id = cna.node_type_id
        join access_role_privilege arp on arp.action_id = cna.id
        join user_group_user_role_user ugur on ugur.user_role_id = arp.access_role_id
        join tenant t on t.id = ugur.user_group_id
        union
        select
        	distinct
        	0,
        	t.id tenant_id,
        	'/' || replace(nt.name, ' ', '_') || '/create' "action"
        from create_node_action cna
        join node_type nt on nt.id = cna.node_type_id
        join access_role_privilege arp on arp.action_id = cna.id
        join user_group_user_role_user ugur on ugur.user_role_id = arp.access_role_id
        join tenant t on t.id = ugur.user_group_id
        where arp.access_role_id = t.access_role_id_not_logged_in
        union
        select
        distinct
        ugur2.user_id,
        t.id tenant_id,
        '/' || replace(nt.name, ' ', '_') || '/create' "action"
        from create_node_action cna
        join node_type nt on nt.id = cna.node_type_id
        join tenant_action ta on ta.action_id = cna.id
        join user_group_user_role_user ugur on ugur.user_group_id = ta.tenant_id
        join user_group ug on ug.id = ugur.user_group_id
        join tenant t on t.id = ug.id
        join user_group_user_role_user ugur2 on ugur2.user_group_id = ug.id and ugur2.user_role_id = ug.administrator_role_id
        ) x
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] { };
    }

    protected override UserTenantAction Read(NpgsqlDataReader reader)
    {
        return new UserTenantAction {
            UserId = UserIdReader.GetValue(reader),
            TenantId = TenantIdReader.GetValue(reader),
            Action = ActionReader.GetValue(reader),
        };
    }
}
