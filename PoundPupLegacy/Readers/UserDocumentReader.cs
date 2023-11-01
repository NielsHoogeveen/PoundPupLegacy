namespace PoundPupLegacy.Readers;

using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using Request = UserDocumentReaderRequest;

public sealed record UserDocumentReaderRequest : IRequest
{
    public required int UserId { get; init; }
    public required int TenantId { get; init; }
}

internal sealed class UserDocumentReaderFactory : MandatorySingleItemDatabaseReaderFactory<Request, UserWithDetails>
{
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };

    private static readonly FieldValueReader<UserWithDetails> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = $"""
        with 
        user_action as (
        	select distinct
        	*
        	from (
        		select
        		uru.user_id,
        		t.id tenant_id,
        		ta.action_id
        		from tenant t
        		join user_role_user uru on uru.user_role_id = t.administrator_role_id
        		join tenant_action ta on ta.tenant_id = t.id
        		union
        		select
        		uar.user_id,
        		uar.tenant_id,	
        		arp.action_id
        		from(
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
        			pug.access_role_id_not_logged_in,
        			t.id tenant_id
        			from tenant t
                    join publishing_user_group pug on pug.id = t.id
        		) uar
        		join access_role_privilege arp on arp.access_role_id = uar.access_role_id
        	) x
        ),
        actions_document as(
        	select
        		jsonb_agg(
        			jsonb_build_object(
        				'Path',
        				path
        			)
        		) document,
        		user_id
        	from(
        		select
        		distinct
        		*
        		from(
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
        		join user_role ur on ur.id = arp.access_role_id
        		join tenant t on t.id = ur.user_group_id
                join publishing_user_group pug on pug.id = t.id
        		where arp.access_role_id = pug.access_role_id_not_logged_in
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
        			'/' || replace(lower(nt.name), ' ', '_') || '/create' "action"
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
        			'/' || replace(lower(nt.name), ' ', '_') || '/create' "action"
        		from create_node_action cna
        		join node_type nt on nt.id = cna.node_type_id
        		join access_role_privilege arp on arp.action_id = cna.id
        		join user_group_user_role_user ugur on ugur.user_role_id = arp.access_role_id
        		join tenant t on t.id = ugur.user_group_id
                join publishing_user_group pug on pug.id = t.id
        		where arp.access_role_id = pug.access_role_id_not_logged_in
        		union
        		select
        		distinct
        		ugur2.user_id,
        		t.id tenant_id,
        		'/' || replace(lower(nt.name), ' ', '_') || '/create' "action"
        		from create_node_action cna
        		join node_type nt on nt.id = cna.node_type_id
        		join tenant_action ta on ta.action_id = cna.id
        		join user_group_user_role_user ugur on ugur.user_group_id = ta.tenant_id
        		join user_group ug on ug.id = ugur.user_group_id
        		join tenant t on t.id = ug.id
        		join user_group_user_role_user ugur2 on ugur2.user_group_id = ug.id and ugur2.user_role_id = ug.administrator_role_id
        		) x
        	) x 
        	where tenant_id = @tenant_id
        	group by user_id
        ),
        create_actions_document as(
        	select
        		jsonb_agg(
        			jsonb_build_object(
        				'NodeTypeId',
        				node_type_id,
        				'NodeTypeName',
        				node_type_name,
                        'UserGroupId',
                        user_group_id
        			)
        		) document,
        		user_id
        	from(
        		select
        			distinct
        			ugur.user_id,
                    t.id tenant_id,
        			ba.node_type_id,
        			nt.name node_type_name,
                    t.user_group_id
        		from create_node_action ba
        		join node_type nt on nt.id = ba.node_type_id
        		join access_role_privilege arp on arp.action_id = ba.id
        		join user_group_user_role_user ugur on ugur.user_role_id = arp.access_role_id
        		join (
                    select  
                    t.id,
                    t.id user_group_id
                    from
                    tenant  t
                    union
                    select
                    s.tenant_id,
                    s.id user_group_id
                    from subgroup s
                ) t on t.user_group_id = ugur.user_group_id
        		union
        		select
        			distinct
        			0,
        			t.id tenant_id,
        			ba.node_type_id,
        			nt.name node_type_name,
                    t.id user_group_id
        		from create_node_action ba
        		join node_type nt on nt.id = ba.node_type_id
        		join access_role_privilege arp on arp.action_id = ba.id
        		join user_group_user_role_user ugur on ugur.user_role_id = arp.access_role_id
        		join tenant t on t.id = ugur.user_group_id
                join publishing_user_group pug on pug.id = t.id
        		where arp.access_role_id = pug.access_role_id_not_logged_in
        		union
        		select
        			uguru.user_id,
        			t.id tenant_id,
        			ba.node_type_id,
        			nt.name node_type_name,
                    t.id user_group_id
        		from create_node_action ba
        		join node_type nt on nt.id = ba.node_type_id
        		join tenant t on 1=1
                join user_group ug on ug.id = t.id
        		join user_group_user_role_user uguru on uguru.user_group_id = ug.id and uguru.user_role_id = ug.administrator_role_id
        	) x
        	where tenant_id = @tenant_id
        	group by user_id
        ),
        edit_actions_document as(
        	select
        		jsonb_agg(
        			jsonb_build_object(
        				'NodeTypeId',
        				node_type_id,
        				'NodeTypeName',
        				node_type_name
        			)
        		) document,
        		user_id
        	from(
        		select
        			distinct
        			ugur.user_id,
        			t.id tenant_id,
        			ba.node_type_id,
        			nt.name node_type_name
        		from edit_node_action ba
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
        		from edit_node_action ba
        		join node_type nt on nt.id = ba.node_type_id
        		join access_role_privilege arp on arp.action_id = ba.id
        		join user_group_user_role_user ugur on ugur.user_role_id = arp.access_role_id
        		join tenant t on t.id = ugur.user_group_id
                join publishing_user_group pug on pug.id = t.id
        		where arp.access_role_id = pug.access_role_id_not_logged_in
        		union
        		select
        			uguru.user_id,
        			tn.id tenant_id,
        			ba.node_type_id,
        			nt.name node_type_name
        		from edit_node_action ba
        		join node_type nt on nt.id = ba.node_type_id
        		join tenant tn on 1=1
        		join user_group ug on ug.id = tn.id
        		join user_group_user_role_user uguru on uguru.user_group_id = ug.id and uguru.user_role_id = ug.administrator_role_id
        	) x
        	where tenant_id = @tenant_id
        	group by user_id
        ),
        edit_own_actions_document as(
        	select
        		jsonb_agg(
        			jsonb_build_object(
        				'NodeTypeId',
        				node_type_id,
        				'NodeTypeName',
        				node_type_name
        			)
        		) document,
        		user_id
        	from(
        		select
        			distinct
        			ugur.user_id,
        			t.id tenant_id,
        			ba.node_type_id,
        			nt.name node_type_name
        		from edit_node_action ba
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
        		from edit_node_action ba
        		join node_type nt on nt.id = ba.node_type_id
        		join access_role_privilege arp on arp.action_id = ba.id
        		join user_group_user_role_user ugur on ugur.user_role_id = arp.access_role_id
        		join tenant t on t.id = ugur.user_group_id
                join publishing_user_group pug on pug.id = t.id
        		where arp.access_role_id = pug.access_role_id_not_logged_in
        		union
        		select
        			uguru.user_id,
        			tn.id tenant_id,
        			ba.node_type_id,
        			nt.name node_type_name
        		from edit_node_action ba
        		join node_type nt on nt.id = ba.node_type_id
        		join tenant tn on 1=1
        		join user_group ug on ug.id = tn.id
        		join user_group_user_role_user uguru on uguru.user_group_id = ug.id and uguru.user_role_id = ug.administrator_role_id
        	) x
        	where tenant_id = @tenant_id
        	group by user_id
        ),
        named_actions_document as(
        	select
        		jsonb_agg(
        			jsonb_build_object(
        				'Name',
        				name
        			)
        		) document,
        		user_id
        	from(
                select
                t.id tenant_id,
                uguru.user_id,
                na.name
                from tenant t
                join user_group ug on ug.id = t.id
                join user_group_user_role_user uguru on uguru.user_group_id = ug.id and uguru.user_role_id = ug.administrator_role_id
                join named_action na on 1=1
                union
                select
                t.id tenant_id,
                uguru.user_id,
                na.name
                from tenant t
                join user_group ug on ug.id = t.id
                join user_group_user_role_user uguru on uguru.user_group_id = ug.id
                join named_action na on 1=1
                join access_role_privilege ap on ap.access_role_id = uguru.user_role_id and ap.action_id = na.id
        		where tenant_id = @tenant_id
        	) x
        	group by user_id
        ),
        menu_items_document as(
        	select
        	jsonb_agg(
        		jsonb_build_object(
        			'MenuItemId',
        			menu_item_id,
        			'ActionId',
        			action_id,
        			'Path', 
        			path,
        			'Title', 
        			"name"
        		)
        	) document,
        	user_id
        	from(
        		select
        		*
        		from(
        		select 
        		ua.user_id,
        		ua.tenant_id,
        		ua.action_id,
        		mi.id menu_item_id,
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
        		null::integer action_id,
        		mi.id menu_item_id,
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
        		null::integer action_id,
        		mi.id menu_item_id,
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
        		where tenant_id = @tenant_id
        		order by user_id, weight 
        	) m
        	group by user_id
        )
        select
        	jsonb_build_object(
                'Id',
                @user_id,
                'Name',
                p.name,
                'NameIdentifier',
                u.name_identifier,
        		'Actions',
        		(select document from actions_document where user_id = @user_id),
        		'CreateActions',
        		(select document from create_actions_document where user_id = @user_id),
        		'EditActions',
        		(select document from edit_actions_document where user_id = @user_id),
        		'EditOwnActions',
        		(select document from edit_own_actions_document where user_id = @user_id),
        		'NamedActions',
        		(select document from named_actions_document where user_id = @user_id),
        		'MenuItems',
        		(select document from menu_items_document where user_id = @user_id)
        	) document
            from tenant t 
            left join "user" u on u.id = @user_id
            left join publisher p on p.id = u.id
            where t.id = @tenant_id
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(UserIdParameter, request.UserId),
            ParameterValue.Create(TenantIdParameter, request.TenantId),
        };
    }

    protected override UserWithDetails Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }

    protected override string GetErrorMessage(Request request)
    {
        return $"User {request.UserId} could not be found";
    }
}
