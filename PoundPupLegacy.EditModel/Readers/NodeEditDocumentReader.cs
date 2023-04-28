namespace PoundPupLegacy.EditModel.Readers;
public record NodeUpdateDocumentRequest : IRequest
{
    public int UrlId { get; init; }
    public int UserId { get; init; }
    public int TenantId { get; init; }

}
public record NodeCreateDocumentRequest : IRequest
{
    public int NodeTypeId { get; init; }
    public int UserId { get; init; }
    public int TenantId { get; init; }

}

public abstract class NodeEditDocumentReaderFactory<TRequest, TResponse> : SingleItemDatabaseReaderFactory<TRequest, TResponse>
where TRequest : IRequest
where TResponse : class, Node
{
    protected const string CTE_EDIT = $"""
        WITH
        {TENANT_NODES_DOCUMENT},
        {TENANTS_DOCUMENT},
        {DOCUMENT_TYPES_DOCUMENT},
        {ATTACHMENTS_DOCUMENT},
        {ORGANIZATION_ORGANIZATION_TYPES_DOCUMENT},
        {ORGANIZATION_TYPES_DOCUMENT},
        {SUBDIVISIONS_DOCUMENT},
        {LOCATIONS_DOCUMENT},
        {COUNTRIES_DOCUMENT}
        """;

    protected const string CTE_CREATE = $"""
        WITH
        {TENANTS_DOCUMENT}
        """;


    const string DOCUMENT_TYPES_DOCUMENT = """
        document_types_document as (
            select
                jsonb_agg(
        	        jsonb_build_object(
        		        'Id',
        		        n.id,
        		        'Name',
        		        n.title
        	        )
                ) document
            from document_type dt
            join term t on t.nameable_id = dt.id
            join tenant_node tn on tn.node_id = t.vocabulary_id
            join node n on n.id = dt.id 
            where tn.url_id = 42416 and tn.tenant_id = 1
        )
        """;

    const string ATTACHMENTS_DOCUMENT = """
        attachments_document as (
            select
                jsonb_agg(
        	        jsonb_build_object(
        		        'Id',
        		        f.id,
                        'Name',
                        f.name,
        		        'Path',
                        f.path,
                        'Size',
                        f.size,
                        'MimeType',
                        f.mime_type,
                        'NodeId',
                        nf.node_id,
                        'HasBeenStored',
                        true
        	        )
                ) document
            from file f
            join node_file nf on nf.file_id = f.id
            join tenant_node tn on tn.node_id = nf.node_id
            where tn.url_id = @url_id and tn.tenant_id = @tenant_id
        )
        """;

    const string SUBDIVISIONS_DOCUMENT = """
        subdivisions_document as(
            select
            c.country_id,
            jsonb_agg(
        	    jsonb_build_object(
        		    'Id',
        		    c.id,
        		    'Name',
        		    t.name
        	    )
            ) document
            from subdivision c
            join bottom_level_subdivision b on b.id = c.id
            join term t on t.nameable_id = c.id
            join tenant_node tn on tn.node_id = t.vocabulary_id
            where tn.tenant_id = 1 and tn.url_id = 4126
            group by c.country_id
        )
        """;

    const string TENANT_NODES_DOCUMENT = """
        tenant_nodes_document as(
        select
            jsonb_agg(
                jsonb_build_object(
                    'Id',
                    id,
                    'TenantId',
                    tenant_id,
                    'UrlId',
                    url_id,
                    'UrlPath',
                    url_path,
                    'NodeId',
                    node_id,
                    'SubgroupId',
                    subgroup_id,
                    'PublicationStatusId',
                    publication_status_id,
                    'HasBeenStored',
                    true,
                    'CanBeUnchecked',
                    false
                )
            ) document
        from(
            select
        		tn2.id,
        		tn2.tenant_id,
        		tn2.url_id,
        		tn2.url_path,
        		tn2.node_id,
        		tn2.subgroup_id,
        		tn2.publication_status_id,
        		(
                    select
                        case 
                            when max(status) = 1 then true
                            else false
                        end
                    from(
                        select
                            distinct
                            1 status
                        from user_group_user_role_user uguru
                        join user_group ug on ug.id = uguru.user_group_id
                        where uguru.user_id = @user_id
                        and user_group_id = tn2.tenant_id
                        and ug.administrator_role_id = uguru.user_role_id
                        union
                        select
                            distinct
                            1 status
                        from user_group_user_role_user uguru
                        join access_role_privilege arp on arp.access_role_id = uguru.user_role_id
                        join create_node_action cna on cna.id = arp.action_id
                        where uguru.user_id = @user_id
                        and user_group_id = tn2.tenant_id
                        and cna.node_type_id = @node_type_id
                    ) x
                ) allow_access
                from tenant_node tn
                join tenant_node tn2 on tn2.node_id = tn.node_id
                where tn.url_id = @url_id and tn.tenant_id = @tenant_id
        ) x 
        where allow_access = true
        )
        """;

    const string TENANTS_DOCUMENT = """
        tenants_document as(
            select
                jsonb_agg(
        	        jsonb_build_object(
        		        'Id',
        		        id,
        		        'DomainName',
        		        domain_name,
        		        'AllowAccess',
        		        allow_access,
                        'PublicationStatusIdDefault',
                        tenant_publication_status_id_default,
        		        'Subgroups',
        		        case 
        			        when subgroups = '[null]' then null
        			        else subgroups
        		        end
        	        )
                ) document
            from(
                select
                    id,
                    domain_name,
                    allow_access,
                    tenant_publication_status_id_default,
                    jsonb_agg(
        	            case when subgroup_id is null then null
        	            else 
        	            jsonb_build_object(
        		            'Id',
        		            subgroup_id,
        		            'Name',
        		            subgroup_name,
                            'PublicationStatusIdDefault',
                            subgroup_publication_status_id_default
        	            )
        	            end
                    ) subgroups
                from(
        		    select
        		        distinct
                        t.id,
                        t.domain_name,
                        pug.publication_status_id_default tenant_publication_status_id_default,
        		        s.name subgroup_name,
                        s.publication_status_id_default subgroup_publication_status_id_default,
        		        s.id subgroup_id,
                        (
                	        select
                		        case 
                			        when max(status) = 1 then true
                			        else false
                		        end
                	        from(
                		        select
                			        distinct
                			        1 status
                		        from user_group_user_role_user uguru
                		        join user_group ug on ug.id = uguru.user_group_id
                		        where uguru.user_id = @user_id
                		        and user_group_id = t.id
                		        and ug.administrator_role_id = uguru.user_role_id
                		        union
                		        select
                			        distinct
                			        1 status
                		        from user_group_user_role_user uguru
                		        join access_role_privilege arp on arp.access_role_id = uguru.user_role_id
                		        join create_node_action cna on cna.id = arp.action_id
                		        where uguru.user_id = @user_id
                		        and user_group_id = t.id
                		        and cna.node_type_id = @node_type_id
                	        ) x
                        ) allow_access
                    from tenant t
                    join publishing_user_group pug on pug.id = t.id
                    left join(
                        select
                	        name,
                	        id,
                	        tenant_id,
                            publication_status_id_default
                        from(
                	        select
                		        distinct
                		        ug.name,
                		        s.id,
                                pug.publication_status_id_default,
                		        s.tenant_id,
                		        (
                			        select
                				        case 
                					        when max(status) = 1 then true
                					        else false
                				        end
                			        from(
                				        select
                					        distinct
                					        1 status
                				        from user_group_user_role_user uguru
                				        join user_group ug on ug.id = uguru.user_group_id
                				        where uguru.user_id = @user_id
                				        and user_group_id = s.id
                				        and ug.administrator_role_id = uguru.user_role_id
                				        union
                				        select
                					        distinct
                					        1 status
                				        from user_group_user_role_user uguru
                				        join access_role_privilege arp on arp.access_role_id = uguru.user_role_id
                				        join create_node_action cna on cna.id = arp.action_id
                				        where uguru.user_id = @user_id
                				        and user_group_id = s.id
                				        and cna.node_type_id = @node_type_id
                			        ) x
                		        ) allow_access
                	        from subgroup s 
                            join publishing_user_group pug on pug.id = s.id
                	        join user_group ug on ug.id = s.id
                        ) x
                        where allow_access =  true
                    ) s on s.tenant_id = t.id
                    join user_group ug on ug.id = t.id
                    join user_group_user_role_user uguru on uguru.user_group_id = ug.id
                    where uguru.user_id = @user_id
                ) x
                group by
                id, domain_name, allow_access,tenant_publication_status_id_default
        	) x
        )
        """;

    const string ORGANIZATION_TYPES_DOCUMENT = """
        organization_types_document as (
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Id',
                        ot.id,
                        'Name',
                        t.name
                    )
                ) document
            from organization_type ot
            join term t on t.nameable_id = ot.id
            join tenant_node tn on tn.node_id = t.vocabulary_id
            where tn.tenant_id = 1 and tn.url_id = 12622
        )
        """;

    const string ORGANIZATION_ORGANIZATION_TYPES_DOCUMENT = """
        organization_organization_types_document as (
            select
                jsonb_agg(
        	        jsonb_build_object(
        		        'OrganizationId',
        		        oot.organization_id,
        		        'OrganizationTypeId',
        		        oot.organization_type_id,
                        'Name',
                        t.name,
        		        'HasBeenStored',
        		        true,
        		        'HasBeenDeleted',
        		        false
        	        )
                ) document
            from organization_organization_type oot
            join tenant_node tn on tn.node_id = oot.organization_id
            join term t on t.nameable_id = oot.organization_type_id
            join tenant_node tn2 on tn2.node_id = t.vocabulary_id
            where tn2.tenant_id = 1 and tn2.url_id = 12622
            and tn.tenant_id = @tenant_id and tn.url_id = @url_id
        )
        """;
    const string LOCATIONS_DOCUMENT = """
        locations_document as(
            select
                jsonb_agg(jsonb_build_object(
        			'LocationId', "location_id",
                    'LocatableId', locatable_id,
        			'Street', street,
        			'Additional', additional,
        			'City', city,
        			'PostalCode', postal_code,
        			'SubdivisionId', subdivision_id,
                    'SubdivisionName', subdivision_name,
        			'CountryId', country_id,
                    'CountryName', country_name,
                    'Latitude', latitude,
                    'Longitude', longitude,
                    'Subdivisions', subdivisions
        		)) document
            from(
                select 
                ll.location_id,
                ll.locatable_id,
                l.street,
                l.additional,
                l.city,
                l.postal_code,
                l.subdivision_id,
                s.name subdivision_name,
                l.country_id,
                nc.title country_name,
                l.latitude,
                l.longitude,
                (select document from subdivisions_document where country_id = l.country_id) subdivisions
                from "location" l
                join location_locatable ll on ll.location_id = l.id
                join node nc on nc.id = l.country_id
                left join subdivision s on s.id = l.subdivision_id
                join tenant_node tn on tn.node_id = ll.locatable_id and tn.tenant_id = @tenant_id and tn.url_id = @url_id
                left join tenant_node tn2 on tn2.node_id = s.id and tn2.tenant_id = @tenant_id
                join tenant_node tn3 on tn3.node_id = nc.id and tn3.tenant_id = @tenant_id
            )x
        )
        """;

    const string COUNTRIES_DOCUMENT = $"""
        countries_document as(
            select
                jsonb_agg(
        	        jsonb_build_object(
        		        'Id',
        		        c.id,
        		        'Name',
        		        t.name
        	        )
                ) document
            from country c
            join term t on t.nameable_id = c.id
            join tenant_node tn on tn.node_id = t.vocabulary_id
            where tn.tenant_id = 1 and tn.url_id = 4126
        )
        """;

}
