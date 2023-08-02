﻿using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;

namespace PoundPupLegacy.Readers;
using Request = UserTenantCreateActionReaderRequest;

public sealed record UserTenantCreateActionReaderRequest : IRequest
{
}

internal sealed class UserTenantCreateActionReaderFactory : EnumerableDatabaseReaderFactory<Request, UserTenantCreateAction>
{

    private static readonly IntValueReader UserIdReader = new() { Name = "user_id" };
    private static readonly IntValueReader TenantIdReader = new() { Name = "tenant_id" };
    private static readonly IntValueReader NodeTypeIdReader = new() { Name = "node_type_id" };
    private static readonly StringValueReader NodeTypeNameReader = new() { Name = "node_type_name" };

    public override string Sql => SQL;

    const string SQL = """
        select
            distinct
            user_id,
            tenant_id,
            node_type_id,
            node_type_name
        from
        (
            select
                distinct
                ugur.user_id,
                t.id tenant_id,
                ba.node_type_id,
                nt.name node_type_name
            from create_node_action ba
            join node_type nt on nt.id = ba.node_type_id
            join access_role_privilege arp on arp.action_id = ba.id
            join user_group_user_role_user ugur on ugur.user_role_id = arp.access_role_id
            join tenant t on t.id = ugur.user_group_id
            union
            select
                distinct
                0,
                t.id tenant_id,
                ba.node_type_id,
                nt.name node_type_name
            from create_node_action ba
            join node_type nt on nt.id = ba.node_type_id
            join access_role_privilege arp on arp.action_id = ba.id
            join user_group_user_role_user ugur on ugur.user_role_id = arp.access_role_id
            join tenant t on t.id = ugur.user_group_id
            where arp.access_role_id = t.access_role_id_not_logged_in
            union
            select
                uguru.user_id,
                tn.id tenant_id,
                ba.node_type_id,
                nt.name node_type_name
            from create_node_action ba
            join node_type nt on nt.id = ba.node_type_id
            join tenant tn on 1=1
            join user_group ug on ug.id = tn.id
            join user_group_user_role_user uguru on uguru.user_group_id = ug.id and uguru.user_role_id = ug.administrator_role_id
        ) x
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] { };
    }

    protected override UserTenantCreateAction Read(NpgsqlDataReader reader)
    {
        return new UserTenantCreateAction {
            UserId = UserIdReader.GetValue(reader),
            TenantId = TenantIdReader.GetValue(reader),
            NodeTypeId = NodeTypeIdReader.GetValue(reader),
            NodeTypeName = NodeTypeNameReader.GetValue(reader),
        };
    }
}