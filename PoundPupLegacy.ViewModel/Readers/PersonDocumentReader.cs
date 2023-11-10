namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.ViewModel.Models;
using Request = NodeDocumentReaderRequest;

internal sealed class PersonDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, Person>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter NodeIdParameter = new() { Name = "node_id" };

    public override string Sql => SQL;
    const string SQL = $"""
            {SharedViewerSql.PARTY_VIEWER},
            {ORGANIZATION_PERSON_RELATIONS_DOCUMENT},
            {BILL_ACTIONS_DOCUMENT},
            {PROFESSIONS_DOCUMENT},
            {PERSON_CASES_DOCUMENT},
            {INTER_PERSONAL_RELATION_DOCUMENT},
            {MEMBER_OF_CONGRESS},
            {BREADCRUM_DOCUMENT},
            {DOCUMENT}
            SELECT document from person_document
            """
            ;

    const string DOCUMENT = """
        person_document AS (
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
                'DateOfBirth', date_of_birth,
                'DateOfDeath', date_of_death,
                'FirstName', first_name,
                'LastName', last_name,
                'FullName', full_name,
                'Suffix', suffix,
                'NickName', nick_name,
                'MiddleName', middle_name,
                'Portrait', jsonb_build_object(
                    'FilePath',
                    portrait_file_path,
                    'Label',
                    full_name
                ),
                'BreadCrumElements', (SELECT document FROM bread_crum_document),
                'Tags', (SELECT document FROM tags_document),
                'CommentListItems', (SELECT document FROM  comments_document),
                'Documents', (SELECT document FROM documents_document),
                'Professions', (SELECT document FROM professions_document),
                'Locations', (SELECT document FROM locations_document),
                'InterPersonalRelations', (SELECT document FROM inter_personal_relation_document),
                'SubTopics', (SELECT document from subtopics_document),
                'SuperTopics', (SELECT document from supertopics_document),
                'PartyCaseTypes', (SELECT document from person_cases_document),
                'OrganizationPersonRelations', (SELECT document from organization_person_relations_document),
                'PartyPoliticalEntityRelations', (SELECT document from party_political_entity_relations_document),
                'Files', (SELECT document FROM files_document),
                'CongressTerms', (SELECT document FROM member_of_congress),
                'BillActions', (SELECT document from bill_actions_document)
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
                    o.date_of_birth,
                    o.date_of_death,
                    o.first_name,
                    o.middle_name,
                    o.last_name,
                    o.full_name,
                    o.suffix,
                    o.nick_name,
                    '/attachment/' || f.id portrait_file_path
                FROM node n
                join tenant_node tn on tn.node_id = n.id
                join person o on o.id = n.id 
                left join file f on f.id = o.file_id_portrait
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
                    '/persons', 
                    'persons', 
                    1
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;

    const string PERSON_CASES_DOCUMENT = """
        person_cases_document as (
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
        		    join case_parties_person o on o.case_parties_id = cp.id
        		    join tenant_node tn2 on tn2.node_id = o.person_id
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

    const string INTER_PERSONAL_RELATION_DOCUMENT = """
        inter_personal_relation_document as(
            select
                jsonb_agg(jsonb_build_object(
        	        'PersonFrom', person_to,
        	        'PersonTo', person_from,
        	        'InterPersonalRelationType', inter_personal_relation_type,
        	        'DateFrom', lower(date_range),
                    'DateTo', upper(date_range),
        	        'Direction', direction
                )) document
            from(
        	    select
        	    jsonb_build_object(
        		    'Title', person_name_from,
        		    'Path', person_path_from
        	    ) person_from,
        	    jsonb_build_object(
        		    'Title', person_name_to,
        		    'Path', person_path_to
        	    ) person_to,
        	    jsonb_build_object(
        		    'Title', inter_personal_relation_type_name,
        		    'Path', inter_personal_relation_type_path
        	    ) inter_personal_relation_type,
        	    date_range,
        	    direction
        	    from(
        		    select
        		    person_name_from,
        		    '/' || viewer_path_person_from || '/' || person_id_from person_path_from,
        		    person_name_to,
        		    '/' || viewer_path_person_to || '/' || person_id_to person_path_to,
        		    inter_personal_relation_type_name,
        		    '/' || viewer_path_inter_personal_relation_type || '/' || inter_personal_relation_type_id inter_personal_relation_type_path,
        		    date_range,
        		    direction
        		    from(
        			    select
        			    distinct
        			    tn1.node_id person_id_from,
                        nt1.viewer_path viewer_path_person_from,
        			    n1.title person_name_from,
        			    tn2.node_id person_id_to,
                        nt2.viewer_path viewer_path_person_to,
        			    n2.title person_name_to,
        			    tn3.node_id inter_personal_relation_type_id,
                        nt3.viewer_path viewer_path_inter_personal_relation_type,
        			    n3.title inter_personal_relation_type_name,
        			    r.date_range,
        			    1 direction
        			    from node n
        			    join inter_personal_relation r on r.id = n.id
        			    join inter_personal_relation_type rt on rt.id = r.inter_personal_relation_type_id
        			    join node n3 on n3.id = rt.id
                        join node_type nt3 on nt3.id = n3.node_type_id
        			    join node n1 on n1.id = r.person_id_from
                        join node_type nt1 on nt1.id = n1.node_type_id
        			    join person o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.person_id_to
                        join node_type nt2 on nt2.id = n2.node_type_id
        			    join person o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id and tn1.node_id = @node_id
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn3.tenant_id = @tenant_id
        			    where rt.is_symmetric
        			    union 
        			    select
        			    distinct
        			    tn2.node_id person_id_to,
                        nt2.viewer_path viewer_path_person_to,
        			    n2.title person_name_to,
        			    tn1.node_id person_id_from,
                        nt1.viewer_path viewer_path_person_from,
        			    n1.title person_name_from,
        			    tn3.node_id inter_personal_relation_type_id,
                        nt3.viewer_path viewer_path_inter_personal_relation_type,
        			    n3.title inter_personal_relation_type_name,
        			    r.date_range,
        			    1 direction
        			    from node n
        			    join inter_personal_relation r on r.id = n.id
        			    join inter_personal_relation_type rt on rt.id = r.inter_personal_relation_type_id
        			    join node n3 on n3.id = rt.id
                        join node_type nt3 on nt3.id = n3.node_type_id
        			    join node n1 on n1.id = r.person_id_from
                        join node_type nt1 on nt1.id = n1.node_type_id
        			    join person o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.person_id_to
                        join node_type nt2 on nt2.id = n2.node_type_id
        			    join person o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id and tn2.node_id = @node_id
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn3.tenant_id = @tenant_id
        			    where rt.is_symmetric
        			    union 
        			    select
        			    distinct
        			    tn1.node_id person_id_from,
                        nt1.viewer_path viewer_path_person_from,
        			    n1.title person_name_from,
        			    tn2.node_id person_id_to,
                        nt2.viewer_path viewer_path_person_to,
        			    n2.title person_name_to,
        			    tn3.node_id inter_person_relational_type_id,
                        nt3.viewer_path viewer_path_inter_personal_relation_type,
        			    n3.title inter_personal_relation_type_name,
        			    r.date_range,
        			    2 direction
        			    from node n
        			    join inter_personal_relation r on r.id = n.id
        			    join inter_personal_relation_type rt on rt.id = r.inter_personal_relation_type_id
        			    join node n3 on n3.id = rt.id
                        join node_type nt3 on nt3.id = n3.node_type_id
        			    join node n1 on n1.id = r.person_id_from
                        join node_type nt1 on nt1.id = n1.node_type_id
        			    join person o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.person_id_to
                        join node_type nt2 on nt2.id = n2.node_type_id
        			    join person o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id and tn1.node_id = @node_id
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id 
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn3.tenant_id = @tenant_id
        			    where not rt.is_symmetric
        			    union 
        			    select
        			    distinct
        			    tn1.node_id person_id_from,
                        nt1.viewer_path viewer_path_person_from,
        			    n1.title person_name_from,
        			    tn2.node_id person_id_to,
                        nt2.viewer_path viewer_path_person_to,
        			    n2.title person_name_to,
        			    tn3.node_id inter_person_relation_type_id,
                        nt3.viewer_path viewer_path_inter_personal_relation_type,
        			    n3.title inter_person_relation_type_name,
        			    r.date_range,
        			    3 direction
        			    from node n
        			    join inter_personal_relation r on r.id = n.id
        			    join inter_personal_relation_type rt on rt.id = r.inter_personal_relation_type_id
        			    join node n3 on n3.id = rt.id
                        join node_type nt3 on nt3.id = n3.node_type_id
        			    join node n1 on n1.id = r.person_id_from
                        join node_type nt1 on nt1.id = n1.node_type_id
        			    join person o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.person_id_to
                        join node_type nt2 on nt2.id = n2.node_type_id
        			    join person o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id 
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id and tn2.node_id = @node_id
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn3.tenant_id = @tenant_id
        			    where not rt.is_symmetric
        		    ) x
        	    ) x
            )x
        )
        """;

    const string PROFESSIONS_DOCUMENT = """
        professions_document AS (
            select
                jsonb_agg(jsonb_build_object(
        	        'Title', "name",
        	        'Path', "path"
                )) "document"
            from(
            select
            n.title "name",
            '/' || nt.viewer_path || '/' || tn2.node_id path	
            from profession ot
            join node n on n.id = ot.id
            join node_type nt on nt.id = n.node_type_id
            join professional_role oot on oot.profession_id = ot.id
            join person o on o.id = oot.person_id
            join tenant_node tn1 on tn1.node_id = o.id and tn1.tenant_id = @tenant_id and tn1.node_id = @node_id
            join tenant_node tn2 on tn2.node_id = ot.id and tn2.tenant_id = @tenant_id 
            ) x
        )
        """;

    const string BILL_ACTIONS_DOCUMENT = """
        bill_actions_document as (
            select
                jsonb_agg(
                    jsonb_build_object(
                        'BillActionType', 
                        jsonb_build_object(
                            'Title', 
                            bill_action_name, 
                            'Path', 
                            bill_action_path
                        ),
                        'Bill', 
                        jsonb_build_object(
                            'Title', 
                            bill_name, 
                            'Path', 
                            bill_path
                        ),
                        'Date',
                        date
                    )
                ) document
            from(
                select
                    '/' || nt2.viewer_path || '/' || tn2.node_id bill_action_path,
                    n2.title bill_action_name,
                    '/' || nt3.viewer_path || '/' || tn3.node_id bill_path,
                    n3.title bill_name,
                    ba.date
                from node n
                join tenant_node tn on tn.node_id = n.id
                join professional_role pr on pr.person_id = n.id
                join representative_house_bill_action ba on ba.representative_id = pr.id
                join node n2 on n2.id = ba.bill_action_type_id
                join node_type nt2 on nt2.id = n2.node_type_id
                join node n3 on n3.id = ba.house_bill_id
                join node_type nt3 on nt3.id = n3.node_type_id
                join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = tn.tenant_id
                join tenant_node tn3 on tn3.node_id = n3.id and tn3.tenant_id = tn.tenant_id
                where tn.node_id = @node_id and tn.tenant_id = @tenant_id
                union
                select
                    '/' || nt2.viewer_path || '/' || tn2.node_id bill_action_path,
                    n2.title bill_action_name,
                    '/' || nt3.viewer_path || '/' || tn3.node_id bill_path,
                    n3.title bill_name,
                    ba.date
                from node n
                join tenant_node tn on tn.node_id = n.id
                join professional_role pr on pr.person_id = n.id
                join senator_senate_bill_action ba on ba.senator_id = pr.id
                join node n2 on n2.id = ba.bill_action_type_id
                join node_type nt2 on nt2.id = n2.node_type_id
                join node n3 on n3.id = ba.senate_bill_id
                join node_type nt3 on nt3.id = n3.node_type_id
                join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = tn.tenant_id
                join tenant_node tn3 on tn3.node_id = n3.id and tn3.tenant_id = tn.tenant_id
                where tn.node_id = @node_id and tn.tenant_id = @tenant_id
            )x
        )
        """;

    public const string ORGANIZATION_PERSON_RELATIONS_DOCUMENT = """
        organization_person_relations_document as(
            select
                jsonb_agg(jsonb_build_object(
        	        'Organization', jsonb_build_object(
        		        'Title', organization_name,
        		        'Path', path
        	        ),
        	        'RelationTypeName', relation_type_name,
        	        'DateFrom', lower(date_range),
        	        'DateTo', upper(date_range)
                )) document
            from(
                select
        			n.title organization_name,
        			n2.title relation_type_name,
        			por.date_range,
        			'/' || nt.viewer_path || '/' || tn.node_id path
        		from  node n
                join node_type nt on nt.id = n.node_type_id
        		join organization pe  on pe.id = n.id
        		join person_organization_relation por on por.organization_id = pe.Id
        		join node n2 on n2.id = por.person_organization_relation_type_id
        		join tenant_node tn2 on tn2.node_id = por.person_id
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

    const string MEMBER_OF_CONGRESS = """
        member_of_congress as(
            select
            x.node_id,
            jsonb_agg(
        	    jsonb_build_object(
        		    'State',
        		    jsonb_build_object(
        			    'Path',
        			    '/' || subdivision_viewer_path || '/' || subdivision_id,
        			    'Title',
        			    subdivision_name
        		    ),
        		    'Roles',
        		    roles
        	    )
            ) document
            from(
        	    select
        	    x.node_id,
        	    x.subdivision_name,
        	    x.subdivision_id,
        	    x.subdivision_viewer_path,
        	    jsonb_agg(
        		    jsonb_build_object(
        			    'Name',
        			    x.role_name,
        			    'Terms',
        			    terms
        		    )
        	    ) roles
        	    from(
        		    select
        		    x.node_id,
        		    x.subdivision_name,
        		    x.subdivision_id,
        		    x.subdivision_viewer_path,
        		    x.role_name,
        		    jsonb_agg(
        			    jsonb_build_object(
        				    'From',
        				    lower(x.date_range_term),
        				    'To',
        				    upper(x.date_range_term),
        				    'PartyAffiliations',
        				    party_affiliations		
        			    )
        		    ) terms
        		    from(
        			    select
        			    x.node_id,
        			    x.subdivision_name,
        			    x.subdivision_id,
        			    x.subdivision_viewer_path,
        			    x.role_name,
        			    x.date_range_term,
        			    jsonb_agg(
        				    jsonb_build_object(
        					    'PartyName',
        					    x.party_name,
        					    'From',
        					    lower(date_range_party_affiliation),
        					    'To',
        					    upper(date_range_party_affiliation)
        				    )
        			    ) party_affiliations
        			    from (
        			    select
        			    p.id node_id,
        			    n2.title role_name,
        			    st.date_range date_range_term,
        			    sd.name subdivision_name,
        			    sd.id subdivision_id,
        			    nt3.viewer_path subdivision_viewer_path,
        			    n4.title party_name,
        			    tpa.date_range date_range_party_affiliation
        			    from person p 
        			    join professional_role pr on pr.person_id = p.id
        			    join node n2 on n2.id = pr.profession_id
        			    join member_of_congress moc on moc.id = pr.id
        			    join senator s on s.id = moc.id
        			    join senate_term st on st.senator_id = s.id
        			    join subdivision sd on sd.id = st.subdivision_id
        			    join node n3 on n3.id = sd.id
        			    join node_type nt3 on nt3.id = n3.node_type_id
        			    join congressional_term_political_party_affiliation tpa on tpa.congressional_term_id = st.id
        			    join united_states_political_party_affiliation pa on pa.id = tpa.united_states_political_party_affiliation_id
        			    join node n4 on n4.id = pa.id
        			    union
        			    select
        			    p.id node_id,
        			    n2.title role_name,
        			    ht.date_range date_range_term,
        			    sd.name subdivision_name,
        			    sd.id subdivision_id,
        			    nt3.viewer_path subdivision_viewer_path,
        			    n4.title party_name,
        			    tpa.date_range date_range_party_affiliation
        			    from person p 
        			    join professional_role pr on pr.person_id = p.id
        			    join node n2 on n2.id = pr.profession_id
        			    join member_of_congress moc on moc.id = pr.id
        			    join representative r on r.id = moc.id
        			    join house_term ht on ht.representative_id = r.id
        			    join subdivision sd on sd.id = ht.subdivision_id
        			    join node n3 on n3.id = sd.id
        			    join node_type nt3 on nt3.id = n3.node_type_id
        			    join congressional_term_political_party_affiliation tpa on tpa.congressional_term_id = ht.id
        			    join united_states_political_party_affiliation pa on pa.id = tpa.united_states_political_party_affiliation_id
        			    join node n4 on n4.id = pa.id
        			    ) x
        			    group by 
        			    x.node_id,
        			    x.subdivision_name,
        			    x.subdivision_id,
        			    x.subdivision_viewer_path,
        			    x.role_name,
        			    x.date_range_term
        		    ) x
        		    GROUP BY
        		    x.node_id,
        		    x.subdivision_name,
        		    x.subdivision_id,
        		    x.subdivision_viewer_path,
        		    x.role_name
        	    ) x
        	    GROUP BY
        	    x.node_id,
        	    x.subdivision_name,
        	    x.subdivision_id,
        	    x.subdivision_viewer_path
            ) x
            where x.node_id = @node_id
            group by x.node_id
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

    protected override Person? Read(NpgsqlDataReader reader)
    {
        return reader.GetFieldValue<Person>(0);
    }
}
