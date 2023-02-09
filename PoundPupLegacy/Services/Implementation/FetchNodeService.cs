using Npgsql;
using PoundPupLegacy.ViewModel;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchNodeService: IFetchNodeService
{
    private readonly NpgsqlConnection _connection;
    private readonly ISiteDataService _siteDateService;
    public FetchNodeService(
        NpgsqlConnection connection, 
        ISiteDataService siteDataService)
    {
        _connection = connection;
        _siteDateService = siteDataService;
    }

    public async Task<Node?> FetchNode(int id, HttpContext context)
    {
        try
        {
            await _connection.OpenAsync();
            var sql = $"""
            WITH 
            {AUTHENTICATED_NODE},
            {SEE_ALSO_DOCUMENT},
            {LOCATIONS_DOCUMENT},
            {INTER_ORGANIZATIONAL_RELATION_DOCUMENT},
            {ORGANIZATION_TYPES_DOCUMENT},
            {TAGS_DOCUMENT},
            {SUBTOPICS_DOCUMENT},
            {SUPERTOPICS_DOCUMENT},
            {DOCUMENTS_DOCUMENT},
            {COMMENTS_DOCUMENT},
            {ORGANIZATIONS_OF_COUNTRY_DOCUMENT},
            {COUNTRY_SUBDIVISIONS_DOCUMENT},
            {BLOG_POST_BREADCRUM_DOCUMENT},
            {ORGANIZATION_BREADCRUM_DOCUMENT},
            {ARTICLE_BREADCRUM_DOCUMENT},
            {ABUSE_CASE_BREADCRUM_DOCUMENT},
            {CHILD_TRAFFICKING_CASE_BREADCRUM_DOCUMENT},
            {COUNTRY_BREADCRUM_DOCUMENT},
            {TOPICS_BREADCRUM_DOCUMENT},
            {ADOPTION_IMPORTS_DOCUMENT},
            {BLOG_POST_DOCUMENT},
            {ARTICLE_DOCUMENT},
            {ABUSE_CASE_DOCUMENT},
            {CHILD_TRAFFICKING_CASE_DOCUMENT},
            {BASIC_NAMEABLE_DOCUMENT},
            {ORGANIZATION_DOCUMENT},
            {BASIC_COUNTRY_DOCUMENT},
            {NODE_DOCUMENT}
            SELECT node_type_id, document from node_document
            """;

            using var readCommand = _connection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;
            readCommand.Parameters.Add("url_id", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
            readCommand.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Integer);
            await readCommand.PrepareAsync();
            readCommand.Parameters["url_id"].Value = id;
            readCommand.Parameters["tenant_id"].Value = 1;
            readCommand.Parameters["user_id"].Value = _siteDateService.GetUserId(context);
            await using var reader = await readCommand.ExecuteReaderAsync();
            await reader.ReadAsync();
            if (!reader.HasRows)
            {
                return null;
            }
            var node_type_id = reader.GetInt32(0);
            var txt = reader.GetString(1);
            Node node = node_type_id switch
            {
                13 => reader.GetFieldValue<BasicCountry>(1),
                23 => reader.GetFieldValue<Organization>(1),
                26 => reader.GetFieldValue<AbuseCase>(1),
                35 => reader.GetFieldValue<BlogPost>(1),
                36 => reader.GetFieldValue<Article>(1),
                37 => reader.GetFieldValue<Discussion>(1),
                41 => reader.GetFieldValue<BasicNameable>(1),
                _ => throw new Exception($"Node {id} has Unsupported type {node_type_id}")
            };
            
            return node!;
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    const string AUTHENTICATED_NODE = """
        authenticated_node as (
            select
                id,
                title,
                node_type_id,
                tenant_id,
                node_id,
                publisher_id,
                created_date_time,
                changed_date_time,
                url_id,
                url_path,
                subgroup_id,
                publication_status_id,
                case 
                    when status = 0 then false
                    else true
                end has_been_published
            from(
                select
                tn.id,
                n.title,
                n.node_type_id,
                tn.tenant_id,
                tn.node_id,
                n.publisher_id,
                n.created_date_time,
                n.changed_date_time,
                tn.url_id,
                case 
                    when tn.url_path is null then '/node/' || tn.url_id
                    else '/' || url_path
                end url_path,
                tn.subgroup_id,
                tn.publication_status_id,
                case
                    when tn.publication_status_id = 0 then (
                        select
                            case 
                                when count(*) > 0 then 0
                                else -1
                            end status
                        from user_group_user_role_user ugu
                        join user_group ug on ug.id = ugu.user_group_id
                        WHERE ugu.user_group_id = 
                        case
                            when tn.subgroup_id is null then tn.tenant_id 
                            else tn.subgroup_id 
                        end 
                        AND ugu.user_role_id = ug.administrator_role_id
                        AND ugu.user_id = @user_id
                    )
                    when tn.publication_status_id = 1 then 1
                    when tn.publication_status_id = 2 then (
                        select
                            case 
                                when count(*) > 0 then 1
                                else -1
                            end status
                        from user_group_user_role_user ugu
                        WHERE ugu.user_group_id = 
                            case
                                when tn.subgroup_id is null then tn.tenant_id 
                                else tn.subgroup_id 
                            end
                            AND ugu.user_id = @user_id
                        )
                    end status	
                    from
                    tenant_node tn
                    join node n on n.id = tn.node_id
                    WHERE tn.tenant_id = @tenant_id AND tn.url_id = @url_id
                ) an
                where an.status <> -1
        )
        """;

    const string SUBTOPICS_DOCUMENT = """
        subtopics_document as(
            select
                json_agg(
                    json_build_object(
                        'Name', "name", 
                        'Path', url_path
                    )
                ) "document"
            from(
                select
                    n.title "name",
                    case 
                        when tn.url_path is null then '/node/' || tn.url_id
                        else '/' || tn.url_path
                    end url_path,
                    case
                        when tn.publication_status_id = 0 then (
                            select
                                case 
                                    when count(*) > 0 then 0
                                    else -1
                                end status
                            from user_group_user_role_user ugu
                            join user_group ug on ug.id = ugu.user_group_id
                            WHERE ugu.user_group_id = 
                            case
                                when tn.subgroup_id is null then tn.tenant_id 
                                else tn.subgroup_id 
                            end 
                            AND ugu.user_role_id = ug.administrator_role_id
                            AND ugu.user_id = @user_id
                        )
                        when tn.publication_status_id = 1 then 1
                        when tn.publication_status_id = 2 then (
                            select
                                case 
                                    when count(*) > 0 then 1
                                    else -1
                                end status
                            from user_group_user_role_user ugu
                            WHERE ugu.user_group_id = 
                                case
                                    when tn.subgroup_id is null then tn.tenant_id 
                                    else tn.subgroup_id 
                                end
                                AND ugu.user_id = @user_id
                            )
                    end status	
                from tenant_node tn
        		join tenant tt on tt.id = tn.tenant_id
                join node n on n.id = tn.node_id
        		join term t1 on t1.nameable_id =  n.id and t1.vocabulary_id = tt.vocabulary_id_tagging
        		join term_hierarchy th on th.term_id_child = t1.id
        		join term t2 on t2.id = th.term_id_parent
        		join tenant_node tn2 on tn2.tenant_id = tn.tenant_id and tn2.node_id = t2.nameable_id
                WHERE tn.tenant_id = @tenant_id AND tn2.url_id = @url_id
            ) an
            where an.status <> -1
        )
        """;

    const string SUPERTOPICS_DOCUMENT = """
        supertopics_document as(
            select
                json_agg(
                    json_build_object(
                        'Name', "name", 
                        'Path', url_path
                    )
                ) "document"
            from(
                select
                    n.title "name",
                    case 
                        when tn.url_path is null then '/node/' || tn.url_id
                        else '/' || tn.url_path
                    end url_path,
                    case
                        when tn.publication_status_id = 0 then (
                            select
                                case 
                                    when count(*) > 0 then 0
                                    else -1
                                end status
                            from user_group_user_role_user ugu
                            join user_group ug on ug.id = ugu.user_group_id
                            WHERE ugu.user_group_id = 
                            case
                                when tn.subgroup_id is null then tn.tenant_id 
                                else tn.subgroup_id 
                            end 
                            AND ugu.user_role_id = ug.administrator_role_id
                            AND ugu.user_id = @user_id
                        )
                        when tn.publication_status_id = 1 then 1
                        when tn.publication_status_id = 2 then (
                            select
                                case 
                                    when count(*) > 0 then 1
                                    else -1
                                end status
                            from user_group_user_role_user ugu
                            WHERE ugu.user_group_id = 
                                case
                                    when tn.subgroup_id is null then tn.tenant_id 
                                    else tn.subgroup_id 
                                end
                                AND ugu.user_id = @user_id
                            )
                    end status	
                from tenant_node tn
        		join tenant tt on tt.id = tn.tenant_id
                join node n on n.id = tn.node_id
        		join term t1 on t1.nameable_id =  n.id and t1.vocabulary_id = tt.vocabulary_id_tagging
        		join term_hierarchy th on th.term_id_parent = t1.id
        		join term t2 on t2.id = th.term_id_child
        		join tenant_node tn2 on tn2.tenant_id = tn.tenant_id and tn2.node_id = t2.nameable_id
                WHERE tn.tenant_id = @tenant_id AND tn2.url_id = @url_id
            ) an
            where an.status <> -1
        )
        """;

    const string LOCATIONS_DOCUMENT = """
        locations_document as(
            select
                json_agg(json_build_object(
        			'Id', "id",
        			'Street', street,
        			'Additional', additional,
        			'City', city,
        			'PostalCode', postal_code,
        			'Subdivision', subdivision,
        			'Country', country,
                    'Latitude', latitude,
                    'Longitude', longitude
        		))::jsonb document
            from(
                select 
                l.id,
                l.street,
                l.additional,
                l.city,
                l.postal_code,
                json_build_object(
                	'Path', case when tn2.url_path is null then '/node/' || tn2.url_id else '/' || tn2.url_path end,
                	'Name', s.name
                ) subdivision,
                json_build_object(
                	'Path', case when tn3.url_path is null then '/node/' || tn3.url_id else '/' || tn3.url_path end,
                	'Name', nc.title
                ) country,
                l.latitude,
                l.longitude
                from "location" l
                join location_locatable ll on ll.location_id = l.id
                join node nc on nc.id = l.country_id
                join subdivision s on s.id = l.subdivision_id
                join tenant_node tn on tn.node_id = ll.locatable_id and tn.tenant_id = @tenant_id and tn.url_id = @url_id
                join tenant_node tn2 on tn2.node_id = s.id and tn2.tenant_id = @tenant_id
                join tenant_node tn3 on tn3.node_id = nc.id and tn3.tenant_id = @tenant_id
            )x
        )
        """;

    const string INTER_ORGANIZATIONAL_RELATION_DOCUMENT = """
        inter_organizational_relation_document as(
            select
                json_agg(json_build_object(
        	        'OrganizationFrom', organization_from,
        	        'OrganizationTo', organization_to,
        	        'InterOrganizationalRelationType', inter_organizational_relation_type,
        	        'GeographicEntity', geographic_entity,
        	        'DateRange', date_range,
        	        'MoneyInvolved', money_involved,
        	        'NumberOfChildrenInvolved', number_of_children_involved,
        	        'Description', description,
        	        'Direction', direction
                ))::jsonb document
            from(
        	    select
        	    json_build_object(
        		    'Name', organization_name_from,
        		    'Path', organization_path_from
        	    ) organization_from,
        	    json_build_object(
        		    'Name', organization_name_to,
        		    'Path', organization_path_to
        	    ) organization_to,
        	    json_build_object(
        		    'Name', inter_organizational_relation_type_name,
        		    'Path', inter_organizational_relation_type_path
        	    ) inter_organizational_relation_type,
        	    case
        	    when geographic_entity_name is null then null
        	    else json_build_object(
        			    'Name', geographic_entity_name,
        			    'Path', geographic_entity_path) 
        	    end geographic_entity,
        	    date_range,
        	    money_involved,
        	    number_of_children_involved,
        	    description,
        	    direction
        	    from(
        		    select
        		    organization_name_from,
        		    case 
        			    when organization_path_from is null then '/node/' || organization_id_from
        			    else '/' || organization_path_from
        		    end organization_path_from,
        		    organization_name_to,
        		    case 
        			    when organization_path_to is null then '/node/' || organization_id_to
        			    else '/' || organization_path_to
        		    end organization_path_to,
        		    inter_organizational_relation_type_name,
        		    case 
        			    when inter_organizational_relation_type_path is null then '/node/' || inter_organizational_relation_type_id
        			    else '/' || inter_organizational_relation_type_path
        		    end inter_organizational_relation_type_path,
        		    geographic_entity_name,
        		    case
        			    when geographic_entity_path is null and geographic_entity_id is null then null
        			    when geographic_entity_path is null  then '/node/' || geographic_entity_id
        			    else '/' || geographic_entity_path
        		    end geographic_entity_path,
        		    date_range,
        		    money_involved,
        		    number_of_children_involved,
        		    description,
        		    direction
        		    from(
        			    select
        			    distinct
        			    tn1.url_id organization_id_from,
        			    tn1.url_path organization_path_from,
        			    n1.title organization_name_from,
        			    tn2.url_id organization_id_to,
        			    tn2.url_path organization_path_to,
        			    n2.title organization_name_to,
        			    tn3.url_id inter_organizational_relation_type_id,
        			    tn3.url_path inter_organizational_relation_type_path,
        			    n3.title inter_organizational_relation_type_name,
        			    tn4.url_id geographic_entity_id,
        			    tn4.url_path geographic_entity_path,
        			    tn4.title  geographic_entity_name,
        			    r.date_range,
        			    r.money_involved,
        			    r.number_of_children_involved,
        			    r.description,
        			    1 direction
        			    from node n
        			    join inter_organizational_relation r on r.id = n.id
        			    join inter_organizational_relation_type rt on rt.id = r.inter_organizational_relation_type_id
        			    join node n3 on n3.id = rt.id
        			    join node n1 on n1.id = r.organization_id_from
        			    join organization o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.organization_id_to
        			    join organization o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id and tn1.url_id = @url_id
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn2.tenant_id = @tenant_id
        			    left join (
        				    select 
        				    tn.url_id,
        				    n.title,
        				    tn.node_id,
        				    tn.url_path
        				    from node n
        				    join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id 
        			    ) tn4 on tn4.node_id = r.geographical_entity_id
        			    where rt.is_symmetric
        			    union 
        			    select
        			    distinct
        			    tn2.url_id organization_id_to,
        			    tn2.url_path organization_path_to,
        			    n2.title organization_name_to,
        			    tn1.url_id organization_id_from,
        			    tn1.url_path organization_path_from,
        			    n1.title organization_name_from,
        			    tn3.url_id inter_organizational_relation_type_id,
        			    tn3.url_path inter_organizational_relation_type_path,
        			    n3.title inter_organizational_relation_type_name,
        			    tn4.url_id geographic_entity_id,
        			    tn4.url_path geographic_entity_path,
        			    tn4.title  geographic_entity_name,
        			    r.date_range,
        			    r.money_involved,
        			    r.number_of_children_involved,
        			    r.description,
        			    1 direction
        			    from node n
        			    join inter_organizational_relation r on r.id = n.id
        			    join inter_organizational_relation_type rt on rt.id = r.inter_organizational_relation_type_id
        			    join node n3 on n3.id = rt.id
        			    join node n1 on n1.id = r.organization_id_from
        			    join organization o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.organization_id_to
        			    join organization o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id and tn2.url_id = @url_id
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn2.tenant_id = @tenant_id
        			    left join (
        				    select 
        				    tn.url_id,
        				    n.title,
        				    tn.node_id,
        				    tn.url_path
        				    from node n
        				    join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id 
        			    ) tn4 on tn4.node_id = r.geographical_entity_id
        			    where rt.is_symmetric
        			    union 
        			    select
        			    distinct
        			    tn1.url_id organization_id_from,
        			    tn1.url_path organization_path_from,
        			    n1.title organization_name_from,
        			    tn2.url_id organization_id_to,
        			    tn2.url_path organization_path_to,
        			    n2.title organization_name_to,
        			    tn3.url_id inter_organizational_relation_type_id,
        			    tn3.url_path inter_organizational_relation_type_path,
        			    n3.title inter_organizational_relation_type_name,
        			    tn4.url_id geographic_entity_id,
        			    tn4.url_path geographic_entity_path,
        			    tn4.title  geographic_entity_name,
        			    r.date_range,
        			    r.money_involved,
        			    r.number_of_children_involved,
        			    r.description,
        			    2 direction
        			    from node n
        			    join inter_organizational_relation r on r.id = n.id
        			    join inter_organizational_relation_type rt on rt.id = r.inter_organizational_relation_type_id
        			    join node n3 on n3.id = rt.id
        			    join node n1 on n1.id = r.organization_id_from
        			    join organization o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.organization_id_to
        			    join organization o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id and tn1.url_id = @url_id
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id 
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn2.tenant_id = @tenant_id
        			    left join (
        				    select 
        				    tn.url_id,
        				    n.title,
        				    tn.node_id,
        				    tn.url_path
        				    from node n
        				    join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id 
        			    ) tn4 on tn4.node_id = r.geographical_entity_id
        			    where not rt.is_symmetric
        			    union 
        			    select
        			    distinct
        			    tn1.url_id organization_id_from,
        			    tn1.url_path organization_path_from,
        			    n1.title organization_name_from,
        			    tn2.url_id organization_id_to,
        			    tn2.url_path organization_path_to,
        			    n2.title organization_name_to,
        			    tn3.url_id inter_organizational_relation_type_id,
        			    tn3.url_path inter_organizational_relation_type_path,
        			    n3.title inter_organizational_relation_type_name,
        			    tn4.url_id geographic_entity_id,
        			    tn4.url_path geographic_entity_path,
        			    tn4.title  geographic_entity_name,
        			    r.date_range,
        			    r.money_involved,
        			    r.number_of_children_involved,
        			    r.description,
        			    3 direction
        			    from node n
        			    join inter_organizational_relation r on r.id = n.id
        			    join inter_organizational_relation_type rt on rt.id = r.inter_organizational_relation_type_id
        			    join node n3 on n3.id = rt.id
        			    join node n1 on n1.id = r.organization_id_from
        			    join organization o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.organization_id_to
        			    join organization o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id 
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id and tn2.url_id = @url_id
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn2.tenant_id = @tenant_id
        			    left join (
        				    select 
        				    tn.url_id,
        				    n.title,
        				    tn.node_id,
        				    tn.url_path
        				    from node n
        				    join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id 
        			    ) tn4 on tn4.node_id = r.geographical_entity_id
        			    where not rt.is_symmetric
        		    ) x
        	    ) x
            )x
        )
        """;

    const string SEE_ALSO_DOCUMENT = """
        see_also_document AS(
            SELECT
                json_agg(
                    json_build_object(
                        'Path', sa.path,
                        'Name', sa.title
                    )::jsonb
                )::jsonb document
            FROM (
                SELECT 
                    case 
                        when tn.url_path is null then '/node/' || tn.url_id
                        else '/' || tn.url_path
                    end path,
                    n2.title
                FROM authenticated_node an
                JOIN node_term nt1 on nt1.node_id = an.node_id
                JOIN node_term nt2 on nt2.term_id = nt1.term_id and nt2.node_id <> nt1.node_id
                JOIN tenant_node tn on tn.node_id = nt2.node_id and tn.tenant_id = @tenant_id and tn.publication_status_id = 1
                JOIN node n2 on n2.id = tn.node_id
                GROUP BY an.node_id, tn.node_id, tn.url_path, tn.url_id, n2.title
                HAVING COUNT(tn.node_id) > 2 
                ORDER BY count(tn.node_id) desc, n2.title
                LIMIT 10
            ) sa
        )
        """;

    const string TAGS_DOCUMENT = """
        tags_document AS (
            SELECT
                json_agg(
                    json_build_object(
                        'Path',  t.path,
                        'Name', t.name
                    )::jsonb
                )::jsonb as document
            FROM (
                select
                    case 
                        when tn2.url_path is null then '/node/' || tn2.url_id
                        else '/' || tn2.url_path
                    end path,
                    t.name
                FROM node_term nt 
                JOIN tenant_node tn on tn.node_id = nt.node_id
                JOIN term t on t.id = nt.term_id
                JOIN tenant_node tn2 on tn2.node_id = t.nameable_id and tn2.tenant_id = @tenant_id and tn2.publication_status_id = 1
                WHERE tn.url_id = @url_id and tn.tenant_id = @tenant_id and tn.publication_status_id = 1
            ) t
        )
        """;

    const string ORGANIZATIONS_OF_COUNTRY_DOCUMENT = """
        organizations_of_country_document as(
            select
                json_agg(
                    json_build_object(
        	            'OrganizationTypeName', organization_type,
        	            'Organizations', organizations
                    )
                )::jsonb document
            from(
                select
        	        organization_type,
        	        json_agg(
        	            json_build_object(
        		            'Name', organization_name,
        		            'Path', "path"
        	            )
                    ) organizations
                from(
                    select
                        n2.title organization_type,
                        n.title organization_name,
                        case 
        	                when tn2.url_path is null then '/node/' || tn2.url_id
        	                else '/' || tn2.url_path
                        end "path"
                    from node n
                    join tenant_node tn2 on tn2.node_id = n.id and tn2.tenant_id = @tenant_id
                    join organization o on o.id = n.id
                    join organization_organization_type oot on oot.organization_id = o.id
                    join organization_type ot on ot.id = oot.organization_type_id
                    join node n2 on n2.id = ot.id
                    join location_locatable ll on ll.locatable_id = n.id
                    join "location" l on l.id = ll.location_id
                    join tenant_node tn on tn.url_id = @url_id and tn.node_id = l.country_id and tn.tenant_id = @tenant_id
        	        ) x
                group by x.organization_type
            ) x
        )
        """;

    const string DOCUMENTS_DOCUMENT = """
        documents_document as(
            select
                json_agg(
                    json_build_object(
                        'Path', path,
                        'Title', title,
                        'PublicationDate', publication_date,
                        'SortOrder', sort_order
                    )::jsonb
                )::jsonb document
            from(
                select
                    path,
                    title,
                    publication_date,
                    row_number() over(order by sort_date desc) sort_order
                from(
                    select
                        case 
        	                when tn2.url_path is null then '/node/' || tn2.url_id
        	                else '/' || tn2.url_path
                        end path,
                        n2.title,
                        case 
        	                when d.publication_date is not null then d.publication_date
        	                else lower(d.publication_date_range)
                        end sort_date,
                        case 
        	                when d.publication_date is not null 
        		                then extract(year from d.publication_date) || ' ' || to_char(d.publication_date, 'Month') || ' ' || extract(DAY FROM d.publication_date)
        	                when extract(month from lower(d.publication_date_range)) = extract(month from upper(d.publication_date_range)) 
        		                then extract(year from lower(d.publication_date_range)) || ' ' || to_char(lower(d.publication_date_range), 'Month') 
        	                when extract(year from lower(d.publication_date_range)) = extract(year from upper(d.publication_date_range)) 
        		                then extract(year from lower(d.publication_date_range))  || ''
        	                else ''
                        end publication_date
                    from documentable_document dd
                    join tenant_node tn on tn.url_id = @url_id and tn.tenant_id = @tenant_id and tn.node_id = dd.documentable_id
                    join tenant_node tn2 on tn2.node_id = dd.document_id and tn2.tenant_id = @tenant_id
                    join node n2 on n2.Id = tn2.node_id
                    join "document" d on d.id = n2.id
                ) x
            ) docs
        )
        """;

    const string BLOG_POST_BREADCRUM_DOCUMENT = """
        blog_post_bread_crum_document AS (
            SELECT json_agg(
                json_build_object(
                    'Path', url,
                    'Name', "name"
                )::jsonb 
            )::jsonb document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/blogs', 
                    'blogs', 
                    1
                UNION
                SELECT 
                    '/blog+/' || p.id, 
                    p.name || '''s blog', 
                    2
                FROM authenticated_node an
                JOIN publisher p on p.id = an.publisher_id
                WHERE an.url_id = @url_id and an.tenant_id = @tenant_id
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string ARTICLE_BREADCRUM_DOCUMENT = """
        article_bread_crum_document AS (
            SELECT json_agg(
                json_build_object(
                    'Path', url,
                    'Name', "name"
                )::jsonb 
            )::jsonb document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/articles', 
                    'aricles', 
                    1
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string TOPICS_BREADCRUM_DOCUMENT = """
        topics_bread_crum_document AS (
            SELECT json_agg(
                json_build_object(
                    'Path', url,
                    'Name', "name"
                )::jsonb 
            )::jsonb document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/topics', 
                    'topics', 
                    1
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string ORGANIZATION_BREADCRUM_DOCUMENT = """
        organization_bread_crum_document AS (
            SELECT json_agg(
                json_build_object(
                    'Path', url,
                    'Name', "name"
                )::jsonb 
            )::jsonb document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/organizations', 
                    'organizations', 
                    1
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;

    const string COUNTRY_SUBDIVISIONS_DOCUMENT = """
        country_subdivisions_document as (
            select
        	    json_agg(json_build_object(
        		    'Name', subdivision_type_name,
        		    'Subdivisions', subdivisions
        	    ))::jsonb document
        	from(
                select
        	        subdivision_type_name,
        	        json_agg(json_build_object(
        		        'Name', subdivision_name,
        		        'Path', case 
        			        when url_path is null then '/node/' || url_id
        			        else url_path
        		        end
        		        )) "subdivisions"
                FROM(
        	        select
        		        distinct
        		        n.title subdivision_type_name, 
        		        s.name subdivision_name,
        		        tn2.url_path,
        		        tn2.url_id
        	        from country c
        	        join tenant_node tn on tn.node_id = c.id and tn.tenant_id = 1 and tn.url_id = @url_id
        	        join tenant t on t.id = tn.tenant_id
        	        join subdivision s on s.country_id = c.id
        	        join node n on n.id = s.subdivision_type_id
        	        join tenant_node tn2 on tn2.node_id = s.id and tn.tenant_id = 1
        	        join term tp on tp.nameable_id = c.id and tp.vocabulary_id = t.vocabulary_id_tagging
        	        join term tc on tc.nameable_id = s.id and tc.vocabulary_id = t.vocabulary_id_tagging
        	        join term_hierarchy th on th.term_id_parent = tp.id and th.term_id_child = tc.id
                ) x
                GROUP BY subdivision_type_name
            ) x
        )
        """;

    const string COUNTRY_BREADCRUM_DOCUMENT = """
        country_bread_crum_document AS (
            SELECT json_agg(
                json_build_object(
                    'Path', url,
                    'Name', "name"
                )::jsonb
            )::jsonb document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/countries', 
                    'countries', 
                    1
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;

    const string ABUSE_CASE_BREADCRUM_DOCUMENT = """
        abuse_case_bread_crum_document AS (
            SELECT json_agg(
                json_build_object(
                    'Path', url,
                    'Name', "name"
                )::jsonb
            )::jsonb document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/cases', 
                    'Cases', 
                    1
                UNION
                SELECT 
                    '/abuse_cases', 
                    'Abuse cases', 
                    2
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;

    const string CHILD_TRAFFICKING_CASE_BREADCRUM_DOCUMENT = """
        child_trafficking_case_bread_crum_document AS (
            SELECT json_agg(
                json_build_object(
                    'Path', url,
                    'Name', "name"
                )::jsonb
            )::jsonb document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/home' url, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    '/cases', 
                    'Cases', 
                    1
                UNION
                SELECT 
                    '/child_trafficking_cases', 
                    'Child trafficing cases', 
                    2
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;

    const string COMMENTS_DOCUMENT = """
        comments_document AS (
            SELECT json_agg(tree)::jsonb document
            FROM (
                SELECT to_jsonb(sub)::jsonb AS tree
                FROM (
        	        SELECT 
        		        c.id AS "Id", 
        		        c.node_status_id AS "NodeStatusId",
        		        json_build_object(
        			        'Id', p.id, 
        			        'Name', p.name,
                            'CreatedDateTime', c.created_date_time,
                            'ChangedDateTime', c.created_date_time
                        )::jsonb AS "Authoring",
        		        c.title AS "Title", 
        		        c.text AS "Text", 
        		        f_comment_tree(c.id) AS "Comments"
        	        FROM comment c
        	        JOIN publisher p on p.id = c.publisher_id
                    JOIN authenticated_node an on an.node_id = c.node_id
        	        WHERE an.url_id = @url_id and an.tenant_id = @tenant_id
        	        AND c.comment_id_parent is null
                ) sub
        	) agg        
        )
        """;

    const string ADOPTION_IMPORTS_DOCUMENT = """
        adoption_imports_document as(
            select
                json_build_object(
                    'StartYear', start_year,
                    'EndYear', end_year,
                    'Imports', json_agg(
                        json_build_object(
                           'CountryFrom', name,
                            'RowType', row_type,
                            'Values', y
                        )::jsonb
                    )::jsonb
                )::jsonb document
            from(
                select
        	        name,
        	        row_type,
        	        start_year,
        	        end_year,
        	        json_agg(
                        json_build_object(
            		        'Year', "year",
            		        'NumberOfChildren', number_of_children
            	        )::jsonb
                    )::jsonb y
                from(
        	        select
        		        row_number() over () id,
        		        case 
        			        when sub is not null then 1
        			        when origin is not null then 2
        			        else 3
        		        end row_type,
        		        case 
        			        when sub is not null then sub
        			        when origin is not null then origin
        			        else null
        		        end name,
        		        number_of_children,
        		        case when "year" is null then 10000
        		        else "year"
        		        end "year",
        		        min("year") over() start_year,
        		        max("year") over() end_year
        	        from(
        		        select
        		        distinct
        		        t.*
        		        from(
        			        select
        			        * 
        			        from
        			        (
        				        select
        					        *,
        					        SUM(number_of_children_involved) over (partition by country_to, "year") toty,
        					        SUM(number_of_children_involved) over (partition by country_to, region_from, "year") totry,
        					        SUM(number_of_children_involved) over (partition by country_to, country_from, "year") totcy,
        					        SUM(number_of_children_involved) over (partition by country_to) tot,
        					        SUM(number_of_children_involved) over (partition by country_to, region_from) totr,
        					        SUM(number_of_children_involved) over (partition by country_to, country_from) totc
        				        from(
        					        select
        						        nto.title country_to,
        						        rfm.title region_from,
        						        nfm.title country_from,
        						        case when 
        							        icr.number_of_children_involved is null then 0
        							        else icr.number_of_children_involved
        						        end number_of_children_involved,
        						        extract('year' from upper(cr.date_range)) "year"
        					        from country_report cr
                                    join node nto on nto.id = cr.country_id
        					        join tenant_node tn on tn.node_id = nto.id and tn.tenant_id = @tenant_id and tn.publication_status_id = 1
        					        join top_level_country cto on cto.id = nto.id
        					        join top_level_country cfm on true 
        					        join node rfm on rfm.id = cfm.global_region_id
        					        join node nfm on nfm.id = cfm.id
                                    join tenant_node tn2 on tn2.tenant_id = @tenant_id and tn2.url_id = 144
        					        LEFT join inter_country_relation icr on icr.country_id_from = cto.id and cfm.id = icr.country_id_to and icr.date_range = cr.date_range and icr.inter_country_relation_type_id = tn2.node_id
        					        WHERE tn.url_id = @url_id 

        				        ) a
        			        ) a
        			        where totc <> 0
        			        ORDER BY country_to, region_from, country_from, "year"
        		        ) c
        		        cross join lateral(
        			        values
        			        (null, null, toty, c."year"),
        			        (region_from, null, totry, c."year"),
        			        (region_from, country_from, totcy, c."year"),
        			        (null, null, tot, null),
        			        (region_from, null, totr, null),
        			        (region_from, country_from, totc, null)
        		        ) as t(origin, sub, number_of_children, "year")
        		        order by t.origin, t.sub, t."year"
        	        ) x
                ) imports
                group by imports.name, row_type, start_year, end_year
                order by min(id)
            ) y
            group by start_year, end_year
        )
        """;


    const string BASIC_COUNTRY_DOCUMENT = """
        basic_country_document AS (
            SELECT 
                json_build_object(
                'Id', n.url_id,
                'Title', n.title, 
                'Description', n.description,
                'HasBeenPublished', n.has_been_published,
                'Authoring', json_build_object(
                    'Id', n.publisher_id, 
                    'Name', n.publisher_name,
                    'CreatedDateTime', n.created_date_time,
                    'ChangedDateTime', n.changed_date_time
                ),
                'HasBeenPublished', n.has_been_published,
                'BreadCrumElements', (SELECT document FROM country_bread_crum_document),
                'Tags', (SELECT document FROM tags_document),
                'Comments', (SELECT document FROM  comments_document),
                'AdoptionImports', (SELECT document FROM adoption_imports_document),
                'Documents', (SELECT document from documents_document),
                'OrganizationTypes', (SELECT document FROM organizations_of_country_document),
                'SubdivisionTypes', (SELECT document FROM country_subdivisions_document),
                'SubTopics', (SELECT document from subtopics_document),
                'SuperTopics', (SELECT document from supertopics_document)
            ) :: jsonb document
            FROM (
                 SELECT
                    an.url_id, 
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    nm.description, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published
                FROM authenticated_node an
                join top_level_country tlc on tlc.id = an.node_id 
                join nameable nm on nm.id = an.node_id
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;

    const string ORGANIZATION_TYPES_DOCUMENT = """
        organization_types_document AS (
            select
                json_agg(json_build_object(
        	        'Name', "name",
        	        'Path', "path"
                ))::jsonb "document"
            from(
            select
            n.title "name",
            case 
        	    when tn2.url_path is null then '/node/' || tn2.url_id
        	    else tn2.url_path
            end path	
            from organization_type ot
            join node n on n.id = ot.id
            join organization_organization_type oot on oot.organization_type_id = ot.id
            join organization o on o.id = oot.organization_id
            join tenant_node tn1 on tn1.node_id = o.id and tn1.tenant_id = @tenant_id and tn1.url_id = @url_id
            join tenant_node tn2 on tn2.node_id = ot.id and tn2.tenant_id = @tenant_id 
            ) x
        )
        """;

    const string ORGANIZATION_DOCUMENT = """
        organization_document AS (
            SELECT 
                json_build_object(
                'Id', n.url_id,
                'Title', n.title, 
                'Description', n.description,
                'HasBeenPublished', n.has_been_published,
                'Authoring', json_build_object(
                    'Id', n.publisher_id, 
                    'Name', n.publisher_name,
                    'CreatedDateTime', n.created_date_time,
                    'ChangedDateTime', n.changed_date_time
                ),
                'HasBeenPublished', n.has_been_published,
                'WebsiteUrl', n.website_url,
                'EmailAddress', n.email_address,
                'Established', n.established,
                'Terminated', n.terminated,
                'BreadCrumElements', (SELECT document FROM organization_bread_crum_document),
                'Tags', (SELECT document FROM tags_document),
                'Comments', (SELECT document FROM  comments_document),
                'Documents', (SELECT document FROM documents_document),
                'OrganizationTypes', (SELECT document FROM organization_types_document),
                'Locations', (SELECT document FROM locations_document),
                'InterOrganizationalRelations', (SELECT document FROM inter_organizational_relation_document),
                'SubTopics', (SELECT document from subtopics_document),
                'SuperTopics', (SELECT document from supertopics_document)
            ) :: jsonb document
            FROM (
                 SELECT
                    an.url_id, 
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    nm.description, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published,
                    o.website_url,
                    o.email_address,
                    o.established,
                    o.terminated
                FROM authenticated_node an
                join organization o on o.id = an.node_id 
                join nameable nm on nm.id = an.node_id
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;


    const string BLOG_POST_DOCUMENT = """
        blog_post_document AS (
            SELECT 
                json_build_object(
                'Id', n.url_id,
                'Title', n.title, 
                'Text', n.text,
                'HasBeenPublished', n.has_been_published,
                'Authoring', json_build_object(
                    'Id', n.publisher_id, 
                    'Name', n.publisher_name,
                    'CreatedDateTime', n.created_date_time,
                    'ChangedDateTime', n.changed_date_time
                ),
                'HasBeenPublished', n.has_been_published,
                'BreadCrumElements', (SELECT document FROM blog_post_bread_crum_document),
                'Tags', (SELECT document FROM tags_document),
                'SeeAlsoBoxElements', (SELECT document FROM see_also_document),
                'Comments', (SELECT document FROM  comments_document)
            ) :: jsonb document
            FROM (
                SELECT
                    an.url_id, 
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    stn.text, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published
                FROM authenticated_node an
                join simple_text_node stn on stn.id = an.node_id 
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;

    const string ARTICLE_DOCUMENT = """
        article_document AS (
            SELECT 
                json_build_object(
                'Id', n.url_id,
                'Title', n.title, 
                'Text', n.text,
                'HasBeenPublished', n.has_been_published,
                'Authoring', json_build_object(
                    'Id', n.publisher_id, 
                    'Name', n.publisher_name,
                    'CreatedDateTime', n.created_date_time,
                    'ChangedDateTime', n.changed_date_time
                ),
                'HasBeenPublished', n.has_been_published,
                'BreadCrumElements', (SELECT document FROM article_bread_crum_document),
                'Tags', (SELECT document FROM tags_document),
                'SeeAlsoBoxElements', (SELECT document FROM see_also_document),
                'Comments', (SELECT document FROM  comments_document)
                    ) :: jsonb document
            FROM (
                SELECT
                    an.url_id, 
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    stn.text, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published
                FROM authenticated_node an
                join simple_text_node stn on stn.id = an.node_id 
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;

    const string BASIC_NAMEABLE_DOCUMENT = """
        basic_nameable_document AS (
            SELECT 
                json_build_object(
                    'Id', n.url_id,
                    'Title', n.title, 
                    'Description', n.description,
                    'HasBeenPublished', n.has_been_published,
                    'Authoring', json_build_object(
                        'Id', n.publisher_id, 
                        'Name', n.publisher_name,
                        'CreatedDateTime', n.created_date_time,
                        'ChangedDateTime', n.changed_date_time
                    ),
                    'HasBeenPublished', n.has_been_published,
                    'BreadCrumElements', (SELECT document FROM topics_bread_crum_document),
                    'SubTopics', (SELECT document from subtopics_document),
                    'SuperTopics', (SELECT document from supertopics_document)
                ) :: jsonb document
            FROM (
                SELECT
                    an.url_id, 
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    n.description, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published
                FROM authenticated_node an
                join basic_nameable bn on bn.id = an.node_id 
                join nameable n on n.id = an.node_id 
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;
    const string ABUSE_CASE_DOCUMENT = """
        abuse_case_document AS (
            SELECT 
                json_build_object(
                    'Id', n.url_id,
                    'Title', n.title, 
                    'Description', n.description,
                    'HasBeenPublished', n.has_been_published,
                    'Authoring', json_build_object(
                        'Id', n.publisher_id, 
                        'Name', n.publisher_name,
                        'CreatedDateTime', n.created_date_time,
                        'ChangedDateTime', n.changed_date_time
                    ),
                    'HasBeenPublished', n.has_been_published,
                    'BreadCrumElements', (SELECT document FROM abuse_case_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'Comments', (SELECT document FROM  comments_document),
                    'Documents', (SELECT document FROM documents_document),
                    'Locations', (SELECT document FROM locations_document),
                    'SubTopics', (SELECT document from subtopics_document),
                    'SuperTopics', (SELECT document from supertopics_document)
                ) :: jsonb document
            FROM (
                SELECT
                    an.url_id, 
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    n.description, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published
                FROM authenticated_node an
                join abuse_case ac on ac.id = an.node_id 
                join nameable n on n.id = an.node_id 
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;
    const string CHILD_TRAFFICKING_CASE_DOCUMENT = """
        child_trafficking_case_document AS (
            SELECT 
                json_build_object(
                    'Id', n.url_id,
                    'Title', n.title, 
                    'Description', n.description,
                    'HasBeenPublished', n.has_been_published,
                    'Authoring', json_build_object(
                        'Id', n.publisher_id, 
                        'Name', n.publisher_name,
                        'CreatedDateTime', n.created_date_time,
                        'ChangedDateTime', n.changed_date_time
                    ),
                    'HasBeenPublished', n.has_been_published,
                    'BreadCrumElements', (SELECT document FROM child_trafficking_case_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'Comments', (SELECT document FROM  comments_document),
                    'Documents', (SELECT document FROM documents_document),
                    'Locations', (SELECT document FROM locations_document),
                    'SubTopics', (SELECT document from subtopics_document),
                    'SuperTopics', (SELECT document from supertopics_document)
                ) :: jsonb document
            FROM (
                SELECT
                    an.url_id, 
                    an.title, 
                    an.created_date_time, 
                    an.changed_date_time, 
                    n.description, 
                    an.publisher_id, 
                    p.name publisher_name,
                    an.has_been_published
                FROM authenticated_node an
                join child_trafficking_case ac on ac.id = an.node_id 
                join nameable n on n.id = an.node_id 
                JOIN publisher p on p.id = an.publisher_id
            ) n
        ) 
        """;
    const string NODE_DOCUMENT = """
        node_document AS (
            SELECT
                an.node_type_id,
                case
                    when an.node_type_id = 13 then (select document from basic_country_document)
                    when an.node_type_id = 23 then (select document from organization_document)
                    when an.node_type_id = 26 then (select document from abuse_case_document)
                    when an.node_type_id = 27 then (select document from child_trafficking_case_document)
                    when an.node_type_id = 35 then (select document from blog_post_document)
                    when an.node_type_id = 36 then (select document from article_document)
                    when an.node_type_id = 41 then (select document from basic_nameable_document)
                end :: jsonb document
            FROM authenticated_node an 
            WHERE an.url_id = @url_id and an.tenant_id = @tenant_id
        ) 
        """;

}
