namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.ViewModel.Models;
using Request = NodeDocumentReaderRequest;

internal sealed class UnitedStatesPoliticalPartyDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, UnitedStatesPoliticalParty>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter NodeIdParameter = new() { Name = "node_id" };

    public override string Sql => SQL;
    const string SQL = $"""
            {SharedViewerSql.PARTY_VIEWER},
            {PERSON_ORGANIZATION_RELATIONS_DOCUMENT},
            {INTER_ORGANIZATIONAL_RELATION_DOCUMENT},
            {ORGANIZATION_TYPES_DOCUMENT},
            {ORGANIZATION_CASES_DOCUMENT},
            {BREADCRUM_DOCUMENT},
            {DOCUMENT}
            SELECT document from organization_document
            """
            ;

    const string DOCUMENT = """
        organization_document AS (
            SELECT 
                jsonb_build_object(
                'NodeId', n.node_id,
                'NodeTypeId', n.node_type_id,
                'Title', n.title, 
                'Description', n.description,
                'HasBeenPublished', n.has_been_published,
                'PublicationStatusId', publication_status_id,
                'Authoring', jsonb_build_object(
                    'Id', n.publisher_id, 
                    'Name', n.publisher_name,
                    'CreatedDateTime', n.created_date_time,
                    'ChangedDateTime', n.changed_date_time
                ),
                'WebsiteUrl', n.website_url,
                'EmailAddress', n.email_address,
                'Establishment', n.established,
                'Termination', n.terminated,
                'BreadCrumElements', (SELECT document FROM bread_crum_document),
                'Tags', (SELECT document FROM tags_document),
                'CommentListItems', (SELECT document FROM  comments_document),
                'Documents', (SELECT document FROM documents_document),
                'OrganizationTypes', (SELECT document FROM organization_types_document),
                'Locations', (SELECT document FROM locations_document),
                'InterOrganizationalRelations', (SELECT document FROM inter_organizational_relation_document),
                'SubTopics', (SELECT document from subtopics_document),
                'SuperTopics', (SELECT document from supertopics_document),
                'PartyCaseTypes', (SELECT document from organization_cases_document),
                'PersonOrganizationRelations', (SELECT document from person_organization_relations_document),
                'PartyPoliticalEntityRelations', (SELECT document from party_political_entity_relations_document),
                'Files', (SELECT document FROM files_document)
            ) document
            FROM (
                 SELECT
                    tn.node_id,
                    n.node_type_id,
                    n.title, 
                    n.created_date_time, 
                    n.changed_date_time, 
                    nm.description, 
                    n.publisher_id, 
                    p.name publisher_name,
                    case
                        when tn.publication_status_id = 0 then false
                        else true
                    end has_been_published,
                    tn.publication_status_id,
                    o.website_url,
                    o.email_address,
                    o.established,
                    o.terminated
                FROM node n
                join tenant_node tn on tn.node_id = n.id
                join organization o on o.id = n.id 
                join united_states_political_party pp on pp.id = o.id
                join nameable nm on nm.id = n.id
                JOIN publisher p on p.id = n.publisher_id
                where tn.tenant_id = @tenant_id and tn.node_id = @node_id
                and tn.publication_status_id in 
                (
                    select 
                    publication_status_id  
                    from user_publication_status 
                    where tenant_id = tn.tenant_id 
                    and user_id = @user_id
                    and (
                        subgroup_id = tn.subgroup_id 
                        or subgroup_id is null and tn.subgroup_id is null
                    )
                )
            ) n
        ) 
        """;

    const string BREADCRUM_DOCUMENT = """
        bread_crum_document AS (
            SELECT jsonb_agg(
                jsonb_build_object(
                    'Path', url,
                    'Title', "name"
                ) 
            ) document
            FROM(
            SELECT
        	    url,
        	    "name"
            FROM(
                SELECT 
                    '/' url, 
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

    const string ORGANIZATION_TYPES_DOCUMENT = """
        organization_types_document AS (
            select
                jsonb_agg(jsonb_build_object(
        	        'Title', "name",
        	        'Path', "path"
                )) "document"
            from(
            select
            n.title "name",
            '/' || nt.viewer_path || '/' || tn2.node_id path	
            from organization_type ot
            join node n on n.id = ot.id
            join node_type nt on nt.id = n.node_type_id
            join organization_organization_type oot on oot.organization_type_id = ot.id
            join organization o on o.id = oot.organization_id
            join tenant_node tn1 on tn1.node_id = o.id and tn1.tenant_id = @tenant_id and tn1.node_id = @node_id
            join tenant_node tn2 on tn2.node_id = ot.id and tn2.tenant_id = @tenant_id 
            ) x
        )
        """;
    const string INTER_ORGANIZATIONAL_RELATION_DOCUMENT = """
        inter_organizational_relation_document as(
            select
                jsonb_agg(jsonb_build_object(
        	        'OrganizationFrom', organization_from,
        	        'OrganizationTo', organization_to,
        	        'InterOrganizationalRelationType', inter_organizational_relation_type,
        	        'GeographicEntity', geographic_entity,
        	        'DateFrom', lower(date_range),
                    'DateTo', upper(date_range),
        	        'MoneyInvolved', money_involved,
        	        'NumberOfChildrenInvolved', number_of_children_involved,
        	        'Description', description,
        	        'Direction', direction
                )) document
            from(
        	    select
        	    jsonb_build_object(
        		    'Title', organization_name_from,
        		    'Path', organization_path_from
        	    ) organization_from,
        	    jsonb_build_object(
        		    'Title', organization_name_to,
        		    'Path', organization_path_to
        	    ) organization_to,
        	    jsonb_build_object(
        		    'Title', inter_organizational_relation_type_name,
        		    'Path', inter_organizational_relation_type_path
        	    ) inter_organizational_relation_type,
        	    case
        	    when geographic_entity_name is null then null
        	    else jsonb_build_object(
        			    'Title', geographic_entity_name,
        			    'Path', geographic_entity_path) 
        	    end geographic_entity,
        	    date_range,
        	    money_involved,
                case 
                    when number_of_children_involved is null then 0
                    else number_of_children_involved
                end number_of_children_involved,
        	    description,
        	    direction
        	    from(
        		    select
        		    organization_name_from,
        		    '/' || viewer_path_organization_from || '/' || organization_id_from organization_path_from,
        		    organization_name_to,
        		    '/' || viewer_path_organization_to || '/' || organization_id_to organization_path_to,
        		    inter_organizational_relation_type_name,
        		    '/' || viewer_path_inter_organizational_relation_type || '/' || inter_organizational_relation_type_id inter_organizational_relation_type_path,
        		    geographic_entity_name,
        		    case
        			    when geographical_entity_id is null then null
        			    else '/' || viewer_path_geographical_entity || '/' || geographical_entity_id
        		    end geographic_entity_path,
        		    date_range,
        		    money_involved,
                    case 
                        when number_of_children_involved is null then 0 
                        else number_of_children_involved 
                    end number_of_children_involved,
        		    description,
        		    direction
        		    from(
        			    select
        			    distinct
        			    tn1.node_id organization_id_from,
                        nt1.viewer_path viewer_path_organization_from,
        			    n1.title organization_name_from,
        			    tn2.node_id organization_id_to,
                        nt2.viewer_path viewer_path_organization_to,
        			    n2.title organization_name_to,
        			    tn3.node_id inter_organizational_relation_type_id,
                        nt3.viewer_path viewer_path_inter_organizational_relation_type,
        			    n3.title inter_organizational_relation_type_name,
        			    tn4.node_id geographical_entity_id,
                        nt4.viewer_path viewer_path_geographical_entity,
        			    tn4.title  geographic_entity_name,
        			    r.date_range,
        			    r.money_involved,
                        case 
                            when r.number_of_children_involved is null then 0
                            else r.number_of_children_involved
                        end number_of_children_involved,
        			    r.description,
        			    1 direction
        			    from node n
                        join node_type nt on nt.id = n.node_type_id
        			    join inter_organizational_relation r on r.id = n.id
        			    join inter_organizational_relation_type rt on rt.id = r.inter_organizational_relation_type_id
        			    join node n3 on n3.id = rt.id
                        join node_type nt3 on nt3.id = n3.node_type_id
        			    join node n1 on n1.id = r.organization_id_from
                        join node_type nt1 on nt1.id = n1.node_type_id
        			    join organization o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.organization_id_to
                        join node_type nt2 on nt2.id = n2.node_type_id
        			    join organization o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id and tn1.node_id = @node_id
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn3.tenant_id = @tenant_id
        			    left join (
        				    select 
        				    n.title,
        				    tn.node_id,
                            n.node_type_id
        				    from node n
        				    join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id 
        			    ) tn4 on tn4.node_id = r.geographical_entity_id
                        left join node_type nt4 on nt4.id = tn4.node_type_id
        			    where rt.is_symmetric
        			    union 
        			    select
        			    distinct
        			    tn2.node_id organization_id_to,
                        nt2.viewer_path viewer_path_organization_to,
        			    n2.title organization_name_to,
        			    tn1.node_id organization_id_from,
                        nt1.viewer_path viewer_path_organization_from,
        			    n1.title organization_name_from,
        			    tn3.node_id inter_organizational_relation_type_id,
                        nt3.viewer_path viewer_path_inter_organizational_relation_type,
        			    n3.title inter_organizational_relation_type_name,
        			    tn4.node_id geographical_entity_id,
                        nt4.viewer_path viewer_path_geographic_entity,
        			    tn4.title  geographic_entity_name,
        			    r.date_range,
        			    r.money_involved,
        			    r.number_of_children_involved,
        			    r.description,
        			    1 direction
        			    from node n
                        join node_type nt on nt.id = n.node_type_id
        			    join inter_organizational_relation r on r.id = n.id
        			    join inter_organizational_relation_type rt on rt.id = r.inter_organizational_relation_type_id
        			    join node n3 on n3.id = rt.id
                        join node_type nt3 on nt3.id = n3.node_type_id
        			    join node n1 on n1.id = r.organization_id_from
                        join node_type nt1 on nt1.id = n1.node_type_id
        			    join organization o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.organization_id_to
                        join node_type nt2 on nt2.id = n2.node_type_id
        			    join organization o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id and tn2.node_id = @node_id
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn3.tenant_id = @tenant_id
        			    left join (
        				    select 
        				    n.title,
        				    tn.node_id,
                            n.node_type_id
        				    from node n
        				    join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id 
        			    ) tn4 on tn4.node_id = r.geographical_entity_id
                        left join node_type nt4 on nt4.id = tn4.node_type_id
        			    where rt.is_symmetric
        			    union 
        			    select
        			    distinct
        			    tn1.node_id organization_id_from,
                        nt1.viewer_path viewer_path_organization_from,
        			    n1.title organization_name_from,
        			    tn2.node_id organization_id_to,
                        nt2.viewer_path viewer_path_organization_to,
        			    n2.title organization_name_to,
        			    tn3.node_id inter_organizational_relation_type_id,
                        nt3.viewer_path viewer_path_inter_organizational_relation_type,
        			    n3.title inter_organizational_relation_type_name,
        			    tn4.node_id geographical_entity_id,
                        nt4.viewer_path viewer_path_geographic_entity,
        			    tn4.title  geographic_entity_name,
        			    r.date_range,
        			    r.money_involved,
        			    r.number_of_children_involved,
        			    r.description,
        			    2 direction
        			    from node n
                        join node_type nt on nt.id = n.node_type_id
        			    join inter_organizational_relation r on r.id = n.id
        			    join inter_organizational_relation_type rt on rt.id = r.inter_organizational_relation_type_id
        			    join node n3 on n3.id = rt.id
                        join node_type nt3 on nt3.id = n3.node_type_id
        			    join node n1 on n1.id = r.organization_id_from
                        join node_type nt1 on nt1.id = n1.node_type_id
        			    join organization o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.organization_id_to
                        join node_type nt2 on nt2.id = n2.node_type_id
        			    join organization o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id and tn1.node_id = @node_id
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id 
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn3.tenant_id = @tenant_id
        			    left join (
        				    select 
        				    n.title,
        				    tn.node_id,
                            n.node_type_id
        				    from node n
        				    join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id 
        			    ) tn4 on tn4.node_id = r.geographical_entity_id
                        left join node_type nt4 on nt4.id = tn4.node_type_id
        			    where not rt.is_symmetric
        			    union 
        			    select
        			    distinct
        			    tn1.node_id organization_id_from,
                        nt1.viewer_path viewer_path_organization_from,
        			    n1.title organization_name_from,
        			    tn2.node_id organization_id_to,
                        nt2.viewer_path viewer_path_organization_to,
        			    n2.title organization_name_to,
        			    tn3.node_id inter_organizational_relation_type_id,
                        nt3.viewer_path viewer_path_inter_organizational_relation_type,
        			    n3.title inter_organizational_relation_type_name,
        			    tn4.node_id geographical_entity_id,
                        nt4.viewer_path viewer_path_geographic_entity,
        			    tn4.title  geographic_entity_name,
        			    r.date_range,
        			    r.money_involved,
        			    r.number_of_children_involved,
        			    r.description,
        			    3 direction
        			    from node n
                        join node_type nt on nt.id = n.node_type_id
        			    join inter_organizational_relation r on r.id = n.id
        			    join inter_organizational_relation_type rt on rt.id = r.inter_organizational_relation_type_id
        			    join node n3 on n3.id = rt.id
                        join node_type nt3 on nt3.id = n3.node_type_id
        			    join node n1 on n1.id = r.organization_id_from
                        join node_type nt1 on nt1.id = n1.node_type_id
        			    join organization o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.organization_id_to
                        join node_type nt2 on nt2.id = n2.node_type_id
        			    join organization o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id 
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id and tn2.node_id = @node_id
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn3.tenant_id = @tenant_id
        			    left join (
        				    select 
        				    n.title,
        				    tn.node_id,
                            n.node_type_id
        				    from node n
        				    join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id 
        			    ) tn4 on tn4.node_id = r.geographical_entity_id
                        left join node_type nt4 on nt4.id = tn4.node_type_id
        			    where not rt.is_symmetric
        		    ) x
        	    ) x
            )x
        )
        """;
    const string ORGANIZATION_CASES_DOCUMENT = """
        organization_cases_document as (
            select
            jsonb_agg(
        	    jsonb_build_object(
        		    'CaseTypeName', case_type_name,
        		    'PartyCases', party_cases
        	    )
            ) document
            from(
            select
            case_type_name,
            jsonb_agg(
        	    jsonb_build_object(
        		    'CasePartyTypeName', case_party_type_name,
        		    'Cases', cases
        	    )
            ) party_cases
            from(
        	    select
        	    case_type_name,
        	    case_party_type_name,
        	    jsonb_agg(jsonb_build_object(
        		    'Title', title,
        		    'Path', path
        	    )) cases
        	    from(
        		    select
        			    nt.name case_type_name,
        			    t.name case_party_type_name,
        			    n.title,
        			    '/' || nt.viewer_path || '/' || tn.node_id path
        		    from case_parties cp
                    join case_case_parties ccp on ccp.case_parties_id = cp.id 
        		    join node n on n.id = ccp.case_id
        		    join case_party_type cpt on cpt.id= ccp.case_party_type_id
                    join term t on t.nameable_id = cpt.id
                    join case_parties_organization o on o.case_parties_id = cp.id
        		    join tenant_node tn2 on tn2.node_id = o.organization_id
        		    join node_type nt on nt.id = n.node_type_id
        		    join tenant_node tn on tn.node_id = n.id and tn.tenant_id = tn2.tenant_id
        		    where tn2.tenant_id = @tenant_id and tn2.node_id = @node_id
                    and t.vocabulary_id = 100022
                    and tn.publication_status_id in 
                    (
                        select 
                        publication_status_id 
                        from user_publication_status 
                        where tenant_id = tn.tenant_id 
                        and user_id = @user_id
                        and (
                            subgroup_id = tn.subgroup_id 
                            or subgroup_id is null and tn.subgroup_id is null
                        )
                    )
        	    )x 
        	    group by case_type_name, case_party_type_name
            ) x
            group by case_type_name
            ) x        
        )
        """;

    const string PERSON_ORGANIZATION_RELATIONS_DOCUMENT = """
        person_organization_relations_document as(
            select
                jsonb_agg(jsonb_build_object(
        	        'Person', jsonb_build_object(
        		        'Title', person_name,
        		        'Path', path
        	        ),
        	        'RelationTypeName', relation_type_name,
        	        'DateFrom', lower(date_range),
        	        'DateTo', upper(date_range)
                )) document
            from(
                select
        			n.title person_name,
        			n2.title relation_type_name,
        			por.date_range,
        			'/' || nt.viewer_path || '/' || tn.node_id path
        		from node n
                join node_type nt on nt.id = n.node_type_id
        		join person pe  on pe.id = n.id
        		join person_organization_relation por on por.person_id = pe.Id
        		join node n2 on n2.id = por.person_organization_relation_type_id
        		join tenant_node tn2 on tn2.node_id = por.organization_id
        		join tenant_node tn on tn.node_id = n.id and tn.tenant_id = tn2.tenant_id
        		where tn2.tenant_id = @tenant_id and tn2.node_id = @node_id
                and tn.publication_status_id in 
                (
                    select 
                    publication_status_id 
                    from user_publication_status 
                    where tenant_id = tn.tenant_id 
                    and user_id = @user_id
                    and (
                        subgroup_id = tn.subgroup_id 
                        or subgroup_id is null and tn.subgroup_id is null
                    )
                )
        	) x
        )
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NodeIdParameter, request.NodeId),
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(UserIdParameter, request.UserId),
        };
    }

    protected override UnitedStatesPoliticalParty? Read(NpgsqlDataReader reader)
    {
        return reader.GetFieldValue<UnitedStatesPoliticalParty>(0);
    }
}
