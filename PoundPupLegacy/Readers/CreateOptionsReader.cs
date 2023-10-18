namespace PoundPupLegacy.ViewModel.Readers;

using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using Request = CreateOptionsReaderRequest;

public sealed record CreateOptionsReaderRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int UserId { get; init; }
}
internal sealed class CreateOptionsReaderFactory : SingleItemDatabaseReaderFactory<Request, List<CreateOption>>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };

    private static readonly FieldValueReader<List<CreateOption>> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = $"""
        select
        jsonb_agg(
        	jsonb_build_object(
        		'Name',
        		name,
        		'Path',
        		path,
        		'Description',
        		description
        	)	
        	order by name
        ) document
        from(
        	select
                distinct
        		lower(node_type_name) name,
                '/' || REPLACE(lower(node_type_name), ' ', '_') || '/create' path,
        		description
            from
            (
                select
                    distinct
                    ugur.user_id,
                    t.id tenant_id,
                    ba.node_type_id,
                    nt.name node_type_name,
        			nt.description
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
                    nt.name node_type_name,
        			nt.description
                from create_node_action ba
                join node_type nt on nt.id = ba.node_type_id
                join access_role_privilege arp on arp.action_id = ba.id
                join user_group_user_role_user ugur on ugur.user_role_id = arp.access_role_id
                join tenant t on t.id = ugur.user_group_id
                where arp.access_role_id = t.access_role_id_not_logged_in
                union
                select
                    uguru.user_id,
                    t.id tenant_id,
                    ba.node_type_id,
                    nt.name node_type_name,
        			nt.description
                from create_node_action ba
                join node_type nt on nt.id = ba.node_type_id and nt.has_editing_support
                join tenant t on 1=1
                join user_group ug on ug.id = t.id
                join user_group_user_role_user uguru on uguru.user_group_id = ug.id and uguru.user_role_id = ug.administrator_role_id
        	) x
        	where user_id = @user_id and tenant_id = @tenant_id
        ) x
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(UserIdParameter, request.UserId),
        };
    }

    protected override List<CreateOption> Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
