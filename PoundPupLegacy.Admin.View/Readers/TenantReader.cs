namespace PoundPupLegacy.Admin.View.Readers;

using Request = TenantReaderRequest;

public sealed record TenantReaderRequest : IRequest
{
    public required int TenantId { get; init; }

}

internal sealed class TenantReaderFactory : SingleItemDatabaseReaderFactory<Request, Tenant>
{

    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };

    private static readonly FieldValueReader<Tenant> DocumentReader = new() { Name = "document" };
    public override string Sql => SQL;

    const string SQL = """
        select
        jsonb_build_object(
        	'TenantId',
        	tenant_id,
        	'DomainName',
        	domain_name,
        	'AccessRoleIdNotLoggedIn',
        	access_role_id_not_logged_in,
        	'CountryIdDefault',
        	country_id_default,
            'CountryNameDefault',
            country_name_default,
        	'FrontPageText',
        	front_page_text,
            'Logo',
            logo,
        	'Subtitle',
        	sub_title,
        	'FooterText',
        	footer_text,
        	'CssFile',
        	css_file,
        	'UserRoles',
        	jsonb_agg(
        		jsonb_build_object(
        			'UserRoleId',
        			user_role_id,
        			'UserRoleName',
        			user_role_name,
        			'IsAdministrator',
        			is_administrator,
        			'BasicActionPrivileges',
        			basic_action_privileges,
        			'CreateNodeActionPrivileges',
        			create_node_action_privileges,
        			'EditNodeActionPrivileges',
        			edit_node_action_privileges,
        			'EditOwnNodeActionPrivileges',
        			edit_own_node_action_privileges,
        			'Users',
        			users,
                    'NamedActionPrivileges',
                    null,
        			'TenantNodeMenuItem',
        			 tenant_node_menu_item
        		)
        	)
        ) document
        from(
        select
        t.id tenant_id,
        t.domain_name,
        t.access_role_id_not_logged_in,
        t.country_id_default,
        c.title country_name_default,
        t.front_page_text,
        t.sub_title,
        t.logo,
        t.footer_text,
        t.css_file,
        ur.id user_role_id,
        ur.name user_role_name,
        case 
        	when ug.administrator_role_id = ur.id then true
        	else false
        end is_administrator,
        (
        	select 
        	jsonb_agg(
        		jsonb_build_object(
        			'ActionId',
        			ba.id,
        			'Path',
        			ba.path,
        			'Description',
        			ba.description,
        			'MenuItem',
        			case 
        				when ami.id is not null then jsonb_build_object(
        					'Id',
        					ami.id,
        					'Name',
        					ami.name
        				)
        				else null
        			end
        		)
        	)
        	from access_role_privilege arp
        	join basic_action ba on ba.id = arp.action_id
        	left join action_menu_item ami on ami.action_id = arp.action_id
        	where arp.access_role_id = ur.id
        ) basic_action_privileges,
        (
        	select 
        	jsonb_agg(
        		jsonb_build_object(
        			'ActionId',
        			na.id,
        			'Name',
        			na.name
        		)
        	)
        	from access_role_privilege arp
        	join named_action na on na.id = arp.action_id
        	where arp.access_role_id = ur.id
        ) named_action_privileges,
        (
        	select 
        	jsonb_agg(
        		jsonb_build_object(
        			'ActionId',
        			ena.id,
        			'NodeTypeId',
        			nt.id,
        			'NodeTypeName',
        			nt.name,
        			'MenuItem',
        			case 
        				when ami.id is not null then jsonb_build_object(
        					'Id',
        					ami.id,
        					'Name',
        					ami.name
        				)
        				else null
        			end
        		)
        	)
        	from access_role_privilege arp
        	join create_node_action ena on ena.id = arp.action_id
        	join node_type nt on nt.id = ena.node_type_id
        	left join action_menu_item ami on ami.action_id = arp.action_id
        	where arp.access_role_id = ur.id
        ) create_node_action_privileges,
        (
        	select 
        	jsonb_agg(
        		jsonb_build_object(
        			'ActionId',
        			ena.id,
        			'NodeTypeId',
        			nt.id,
        			'NodeTypeName',
        			nt.name
        		)
        	)
        	from access_role_privilege arp
        	join edit_node_action ena on ena.id = arp.action_id
        	join node_type nt on nt.id = ena.node_type_id
        	where arp.access_role_id = ur.id
        ) edit_node_action_privileges,
        (
        	select 
        	jsonb_agg(
        		jsonb_build_object(
        			'ActionId',
        			ena.id,
        			'NodeTypeId',
        			nt.id,
        			'NodeTypeName',
        			nt.name
        		)
        	)
        	from access_role_privilege arp
        	join edit_own_node_action ena on ena.id = arp.action_id
        	join node_type nt on nt.id = ena.node_type_id
        	where arp.access_role_id = ur.id
        ) edit_own_node_action_privileges,
        (
        	select
        	jsonb_agg(
        		jsonb_build_object(
        			'UserId',
        			p.id,
        			'Name',
        			p.name
        		)
        	)
        	from user_group_user_role_user uguru
        	join publisher p on p.id = uguru.user_id
        	where uguru.user_group_id = t.id and uguru.user_role_id = ur.id
        ) users,
        (
        	select
        	jsonb_agg(
        		jsonb_build_object(
        			'MenuItemId',
        			tnmi.id,
        			'TenantNodeId',
        			tn.id,
        			'Name',
        			tnmi.name,
        			'Title',
        			n.title,
                    'ActionId',
                    1,
                    'Path',
                    ''
        		)
        	)
        	from tenant_node tn
        	join tenant_node_menu_item tnmi on tnmi.tenant_node_id = tn.id
        	join node n on n.id = tn.node_id
        	where tn.tenant_id = t.id
        ) tenant_node_menu_item
        from tenant t
        join node c on c.id = t.country_id_default
        join user_group ug on ug.id = t.id
        join user_role ur on ur.user_group_id = ug.id
        where t.id = @tenant_id
        ) t
        group by
        t.tenant_id,
        t.domain_name,
        t.access_role_id_not_logged_in,
        t.country_id_default,
        t.country_name_default,
        t.front_page_text,
        t.sub_title,
        t.logo,
        t.footer_text,
        t.css_file
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantIdParameter, request.TenantId),
        };
    }

    protected override Tenant Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
