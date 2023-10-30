namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.ViewModel.Models;
using System.Dynamic;
using Request = NodeDocumentReaderRequest;

public sealed class NodeDocumentReaderRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int UserId { get; init; }
    public required int UrlId { get; init; }
}

internal sealed class NodeDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, Node>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter UrlIdParameter = new() { Name = "url_id" };

    public override string Sql => SQL;

    const string SQL = $"""
            WITH 
            {TERM_DESCENDANCY},
            {SharedSql.ACCESSIBLE_PUBLICATIONS_STATUS},
            {NODES_WITH_DESCENDANTS},
            {NAMEABLE_CASES_DOCUMENT},
            {FILES_DOCUMENT},
            {SEE_ALSO_DOCUMENT},
            {LOCATIONS_DOCUMENT},
            {ORGANIZATION_CASES_DOCUMENT},
            {PERSON_CASES_DOCUMENT},
            {CASE_CASE_PARTIES_DOCUMENT},
            {INTER_ORGANIZATIONAL_RELATION_DOCUMENT},
            {INTER_PERSONAL_RELATION_DOCUMENT},
            {PARTY_POLITICAL_ENTITY_RELATIONS_DOCUMENT},
            {PROFESSIONS_DOCUMENT},
            {ORGANIZATION_TYPES_DOCUMENT},
            {PERSON_ORGANIZATION_RELATIONS_DOCUMENT},
            {ORGANIZATION_PERSON_RELATIONS_DOCUMENT},
            {TAGS_DOCUMENT},
            {TAGS_DOCUMENT_ABUSE_CASE},
            {BILL_ACTIONS_DOCUMENT},
            {SUBTOPICS_DOCUMENT},
            {SUPERTOPICS_DOCUMENT},
            {DOCUMENTS_DOCUMENT},
            {COMMENTS_DOCUMENT},
            {ORGANIZATIONS_OF_COUNTRY_DOCUMENT},
            {ORGANIZATIONS_OF_SUBDIVISION_DOCUMENT},
            {COUNTRY_SUBDIVISIONS_DOCUMENT},
            {SUBDIVISION_SUBDIVISIONS_DOCUMENT},
            {BLOG_POST_BREADCRUM_DOCUMENT},
            {DISCUSSION_BREADCRUM_DOCUMENT},
            {PAGE_BREADCRUM_DOCUMENT},
            {ORGANIZATION_BREADCRUM_DOCUMENT},
            {PERSON_BREADCRUM_DOCUMENT},
            {DOCUMENT_BREADCRUM_DOCUMENT},
            {POLL_BREADCRUM_DOCUMENT},
            {POLL_OPTIONS_DOCUMENT},
            {POLL_QUESTIONS_DOCUMENT},
            {ABUSE_CASE_BREADCRUM_DOCUMENT},
            {CHILD_TRAFFICKING_CASE_BREADCRUM_DOCUMENT},
            {COERCED_ADOPTION_CASE_BREADCRUM_DOCUMENT},
            {DEPORTATION_CASE_BREADCRUM_DOCUMENT},
            {DISRUPTED_PLACEMENT_CASE_BREADCRUM_DOCUMENT},
            {FATHERS_RIGHTS_VIOLATION_CASE_BREADCRUM_DOCUMENT},
            {WRONGFUL_MEDICATION_CASE_BREADCRUM_DOCUMENT},
            {WRONGFUL_REMOVAL_CASE_BREADCRUM_DOCUMENT},
            {SUBDIVISION_BREADCRUM_DOCUMENT},
            {GLOBAL_REGION_BREADCRUM_DOCUMENT},
            {COUNTRY_BREADCRUM_DOCUMENT},
            {TOPICS_BREADCRUM_DOCUMENT},
            {ADOPTION_IMPORTS_DOCUMENT},
            {BLOG_POST_DOCUMENT},
            {PAGE_DOCUMENT},
            {DISCUSSION_DOCUMENT},
            {DOCUMENT_DOCUMENT},
            {SINGLE_QUESTION_POLL_DOCUMENT},
            {MULTIPLE_QUESTION_POLL_DOCUMENT},
            {ABUSE_CASE_DOCUMENT},
            {CHILD_TRAFFICKING_CASE_DOCUMENT},
            {COERCED_ADOPTION_CASE_DOCUMENT},
            {DEPORTATION_CASE_DOCUMENT},
            {DISRUPTED_PLACEMENT_CASE_DOCUMENT},
            {FATHERS_RIGHTS_VIOLATION_CASE_DOCUMENT},
            {WRONGFUL_MEDICATION_CASE_DOCUMENT},
            {WRONGFUL_REMOVAL_CASE_DOCUMENT},
            {BASIC_NAMEABLE_DOCUMENT},
            {ORGANIZATION_DOCUMENT},
            {PERSON_DOCUMENT},
            {INFORMAL_SUBDIVISION_DOCUMENT},
            {FORMAL_SUBDIVISION_DOCUMENT},
            {BASIC_COUNTRY_DOCUMENT},
            {GLOBAL_REGION_DOCUMENT},
            {COUNTRY_AND_SUBDIVISION_DOCUMENT},
            {BINDING_COUNTRY_DOCUMENT},
            {BOUND_COUNTRY_DOCUMENT},
            {NODE_DOCUMENT}
            SELECT node_type_id, document from node_document
            """
    ;

    const string TERM_DESCENDANCY = """
         RECURSIVE term_descendancy AS(
            select
        	n.id node_id,
        	n.title,
        	t.id term_id
        	from node n
        	join term t on t.nameable_id = n.id
        	join tenant_node tn on tn.node_id = n.id
        	where tn.tenant_id = @tenant_id and tn.url_id = @url_id
            UNION ALL
            select
        	n.id node_id,
        	n.title,
        	t.id term_id
        	from node n
        	join term t on t.nameable_id = n.id
        	join term_hierarchy th on th.term_id_child = t.id
        	join tenant_node tn on tn.node_id = n.id
        	join term_descendancy td on td.term_id = th.term_id_parent 
        )
        """;
    const string NODES_WITH_DESCENDANTS = """
        node_with_descendants as
        (
        	SELECT 
            DISTINCT n.id
        	FROM term_descendancy td
        	join node n on n.id = td.node_id
        )
        """;
    const string FILES_DOCUMENT = """
        files_document as(
            select
            jsonb_agg(
                jsonb_build_object(
                    'Id', f.id,
                    'Name', f.name,
                    'Size', f.size,
                    'MimeType', f.mime_type,
                    'Path', f.path
                )
            ) document
            from node_file nf
            join tenant_node tn on tn.node_id = nf.node_id
            join "file" f on f.id = nf.file_id
            where tn.tenant_id = @tenant_id and tn.url_id = @url_id
        )
        """;

    const string POLL_OPTIONS_DOCUMENT = """
        poll_options_document as(
            select
                jsonb_agg(
        	        jsonb_build_object(
        		        'Text', text,
        		        'NumberOfVotes', number_of_votes,
        		        'Percentage', round(100 * (number_of_votes::numeric  / total), 0),
                        'Delta', delta
        	        )
                ) document
            from(
        	    select 
        	    po.text,
        	    po.number_of_votes,
        	    sum(number_of_votes) over() total,
                po.delta
        	    from poll_option po
                join poll_question pq on po.poll_question_id = pq.id
        	    join tenant_node tn on tn.node_id = pq.id
        	    where tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) x
        )
        """;

    const string POLL_QUESTIONS_DOCUMENT = """
        poll_questions_document as(
            select
        	    jsonb_agg(
        		    jsonb_build_object(
                        'UrlId', 
                        url_id,
                        'NodeId', 
                        node_id,
                        'NodeTypeId', 
                        node_type_id,
                        'Title', 
                        title, 
        			    'Text',  
                        question_text,
        			    'Authoring', 
                        jsonb_build_object(
        				    'Id', publisher_id, 
        				    'Name', publisher_name,
        				    'CreatedDateTime', created_date_time,
                            'ChangedDateTime', changed_date_time
                        ),
                        'HasBeenPublished', 
                        true,
                        'PublicationStatusId',
                        1,
        			    'PollOptions', 
                        poll_options,
                        'BreadCrumElements', 
                        (SELECT document FROM poll_breadcrum_document),
                        'Files', 
                        (SELECT document FROM files_document),
                        'Tags', 
                        (SELECT document FROM tags_document)
        		    )
        	    ) document
            from(
        	    select
        		    node_id,
                    url_id,
        		    question_text,
        		    node_type_id,
                    title,
        		    created_date_time,
        		    changed_date_time,
        		    publisher_id,
        		    publisher_name,
        		    jsonb_agg(
        			    jsonb_build_object(
        				    'Text', option_text,
        				    'NumberOfVotes', number_of_votes,
        				    'Percentage', round(100 * (number_of_votes::numeric  / total), 0),
        				    'Delta', delta
        			    )
        		    ) poll_options
        	    from(
        		    select 
        		    tn.node_id,
                    tn.url_id,
        		    stn.text question_text,
        		    n.node_type_id,
                    n.title,
        		    n.created_date_time,
        		    n.changed_date_time,
        		    p.id publisher_id,
        		    p.name publisher_name,
        		    po.text option_text,
        		    po.number_of_votes,
        		    sum(number_of_votes) over() total,
        		    po.delta
        		    from poll_option po
        		    join poll_question pq on po.poll_question_id = pq.id
        		    join simple_text_node stn on stn.id = pq.id
        		    join node n on n.id = pq.id
        		    join publisher p on p.id = n.publisher_id
        		    join multi_question_poll_poll_question mqppq on mqppq.poll_question_id = pq.id
        		    join tenant_node tn on tn.node_id = mqppq.multi_question_poll_id
        		    where tn.tenant_id = @tenant_id and tn.url_id = @url_id
        	    ) x
        	    group by 
        		    node_id,
                    url_id,
                    title,
        		    question_text,
        		    node_type_id,
        		    created_date_time,
        		    changed_date_time,
        		    publisher_id,
        		    publisher_name
            ) x
        )
        """;

    const string PARTY_POLITICAL_ENTITY_RELATIONS_DOCUMENT = """
        party_political_entity_relations_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Party', jsonb_build_object(
        	                'Title', party_name,
        	                'Path', party_path
                        ),
                        'PoliticalEntity', jsonb_build_object(
        	                'Title', political_entity_name,
        	                'Path', political_entity_path
                        ),
                        'PartyPoliticalEntityRelationType',
                        jsonb_build_object(
        	                'Title', party_political_entity_relation_type_name,
        	                'Path', party_political_entity_relation_type_path
                        ),
                        'DateFrom', lower(date_range),
                        'DateTo', upper(date_range),
                        'ProofDocument', case
        	                when publication_status_id_5 is null then null
        	                else jsonb_build_object(
        		                'Title', document_proof_name,
        		                'Path', document_proof_path
        	                )
                        end
                    )
                ) document
            from(
                select
        		    n2.title party_name,
        		    case
        			    when tn2.url_path is null then '/node/' || tn2.url_id
        			    else tn2.url_path
        		    end party_path,
        		    n3.title political_entity_name,
        		    case
        			    when tn3.url_path is null then '/node/' || tn3.url_id
        			    else tn3.url_path
        		    end political_entity_path,
                    tn5.publication_status_id publication_status_id_5,
        		    n4.title party_political_entity_relation_type_name,
        		    case
        			    when tn4.url_path is null then '/node/' || tn4.url_id
        			    else tn4.url_path
        		    end party_political_entity_relation_type_path,
        		    pper.date_range,
        		    case 
        			    when tn.url_path is null then '/node/' || tn.url_id
        			    else '/' || tn.url_path
        		    end path,
        		    n5.title document_proof_name,
        		    case 
        			    when tn5.id is null then null
        			    when tn5.url_path is null then '/node/' || tn.url_id
        			    else '/' || tn.url_path
        		    end document_proof_path
        	    from  node n
        	    join tenant_node tn on tn.node_id = n.id
        	    join party_political_entity_relation pper on pper.id = n.id 

        	    join node n2 on n2.id = pper.party_id				
        	    join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = tn.tenant_id

        	    join node n3 on n3.id = pper.political_entity_id				
        	    join tenant_node tn3 on tn3.node_id = n3.id and tn3.tenant_id = tn.tenant_id

        	    join node n4 on n4.id = pper.party_political_entity_relation_type_id
        	    join tenant_node tn4 on tn4.node_id = n4.id and tn4.tenant_id = tn.tenant_id

        	    left join node n5 on n5.id = pper.document_id_proof
        	    left join tenant_node tn5 on tn4.node_id = n5.id and tn5.tenant_id = tn.tenant_id

        	    where tn.tenant_id = @tenant_id and tn2.url_id = @url_id
                and tn.publication_status_id in 
                (
                    select 
                    id 
                    from accessible_publication_status 
                    where tenant_id = tn.tenant_id 
                    and (
                        subgroup_id = tn.subgroup_id 
                        or subgroup_id is null and tn.subgroup_id is null
                    )
                )
                and tn3.publication_status_id in 
                (
                    select 
                    id 
                    from accessible_publication_status 
                    where tenant_id = tn3.tenant_id 
                    and (
                        subgroup_id = tn3.subgroup_id 
                        or subgroup_id is null and tn3.subgroup_id is null
                    )
                )
                and tn4.publication_status_id in 
                (
                    select 
                    id 
                    from accessible_publication_status 
                    where tenant_id = tn4.tenant_id 
                    and (
                        subgroup_id = tn4.subgroup_id 
                        or subgroup_id is null and tn4.subgroup_id is null
                    )
                )
        	) x
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
        			    case 
        				    when tn.url_path is null then '/node/' || tn.url_id
        				    else '/' || tn.url_path
        			    end path
        		    from case_parties cp
                    join case_case_parties ccp on ccp.case_parties_id = cp.id 
        		    join node n on n.id = ccp.case_id
        		    join case_party_type cpt on cpt.id= ccp.case_party_type_id
                    join term t on t.nameable_id = cpt.id
                    join tenant_node tn3 on tn3.node_id = t.vocabulary_id and tn3.tenant_id = 1 and tn3.url_id = 156
                    join case_parties_organization o on o.case_parties_id = cp.id
        		    join tenant_node tn2 on tn2.node_id = o.organization_id
        		    join node_type nt on nt.id = n.node_type_id
        		    join tenant_node tn on tn.node_id = n.id and tn.tenant_id = tn2.tenant_id
        		    where tn2.tenant_id = @tenant_id and tn2.url_id = @url_id
                    and tn.publication_status_id in 
                    (
                        select 
                        id 
                        from accessible_publication_status 
                        where tenant_id = tn.tenant_id 
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

    const string NAMEABLE_CASES_DOCUMENT = """
        nameable_cases_document as(
            select
            jsonb_agg(
        	    jsonb_build_object(
        		    'CaseTypeName',
        		    name,
        		    'Cases',
        		    cases
        	    )
        	    order by name
            ) document
            from(
        	    select
        	    name,
        	    jsonb_agg(
        		    jsonb_build_object(
        			    'Path',
        			    case 
        				    when url_path is null then '/node/' || url_id
        				    else '/' || url_path
        			    end,
        			    'Title',
        			    title,
                        'Date',
                        fuzzy_date
        		    )
        		    order by title
        	    ) cases
                from(
                    select
                    distinct 
                    n2.title,
                    nt2.name,
                    tn2.url_path,
                    tn2.url_id,
                    c.fuzzy_date
        	        from node n
        	        join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id
        	        join(		
        		        select
        		        distinct
        		        nameable_id,
                        id,
                        node_type_id,
                        title
        		        from(
        			        select
        			        t.nameable_id,
        			        n2.id,
        			        n2.node_type_id,
        			        n2.title
        			        from term t 
        			        join node_term nt on nt.term_id = t.id
        			        join node n2 on n2.id = nt.node_id
        			        union
        			        select
        			        l.subdivision_id nameable_id,
        			        n2.id,
        			        n2.node_type_id,
        			        n2.title
        			        from "location" l
        			        join location_locatable ll on ll.location_id = l.id
        			        join node n2 on n2.id = ll.locatable_id
        			        union
        			        select
        			        l.country_id nameable_id,
        			        n2.id,
        			        n2.node_type_id,
        			        n2.title
        			        from "location" l
        			        join location_locatable ll on ll.location_id = l.id
        			        join node n2 on n2.id = ll.locatable_id
                            union
                            select
                            t.type_of_abuse_id nameable_id,
                            n2.id,
                            n2.node_type_id,
                            n2.title
                            from abuse_case_type_of_abuse t
                            join node n2 on n2.id = t.abuse_case_id
                            union
                            select
                            t.type_of_abuser_id nameable_id,
                            n2.id,
                            n2.node_type_id,
                            n2.title
                            from abuse_case_type_of_abuser t
                            join node n2 on n2.id = t.abuse_case_id
        
        		        ) n2
        	        ) n2 on n2.nameable_id = n.id
        	        join "case" c on c.id = n2.id
        	        join node_type nt2 on nt2.id = n2.node_type_id
        	        join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = tn.tenant_id
                    where n.id in (select nwd.id from node_with_descendants nwd)
                    and tn.publication_status_id in 
                    (
                        select 
                        id 
                        from accessible_publication_status 
                        where tenant_id = tn.tenant_id 
                        and (
                            subgroup_id = tn.subgroup_id 
                            or subgroup_id is null and tn.subgroup_id is null
                        )
                    )
                    and tn2.publication_status_id in 
                    (
                        select 
                        id 
                        from accessible_publication_status 
                        where tenant_id = tn2.tenant_id 
                        and (
                            subgroup_id = tn2.subgroup_id 
                            or subgroup_id is null and tn2.subgroup_id is null
                        )
                    )
                ) x
                group by name
            ) x
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
        			    case 
        				    when tn.url_path is null then '/node/' || tn.url_id
        				    else '/' || tn.url_path
        			    end path
        		    from case_parties cp
        		    join case_case_parties ccp on ccp.case_parties_id = cp.id 
        		    join node n on n.id = ccp.case_id
        		    join case_party_type cpt on cpt.id= ccp.case_party_type_id
                    join term t on t.nameable_id = cpt.id
                    join tenant_node tn3 on tn3.node_id = t.vocabulary_id and tn3.tenant_id = 1 and tn3.url_id = 156
        		    join case_parties_person o on o.case_parties_id = cp.id
        		    join tenant_node tn2 on tn2.node_id = o.person_id
        		    join node_type nt on nt.id = n.node_type_id
        		    join tenant_node tn on tn.node_id = n.id and tn.tenant_id = tn2.tenant_id
        		    where tn2.tenant_id = @tenant_id and tn2.url_id = @url_id
                    and tn.publication_status_id in 
                    (
                        select 
                        id 
                        from accessible_publication_status 
                        where tenant_id = tn.tenant_id 
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

    const string SUBTOPICS_DOCUMENT = """
        subtopics_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Title', "name", 
                        'Path', url_path
                    )
                ) "document"
            from(
                select
                    n.title "name",
                    case 
                        when tn.url_path is null then '/node/' || tn.url_id
                        else '/' || tn.url_path
                    end url_path
                from tenant_node tn
        		join tenant tt on tt.id = tn.tenant_id
                join node n on n.id = tn.node_id
                join system_group sg on sg.id = 0
        		join term t1 on t1.nameable_id =  n.id and t1.vocabulary_id = sg.vocabulary_id_tagging
        		join term_hierarchy th on th.term_id_child = t1.id
        		join term t2 on t2.id = th.term_id_parent
        		join tenant_node tn2 on tn2.tenant_id = tn.tenant_id and tn2.node_id = t2.nameable_id
                WHERE tn.tenant_id = @tenant_id AND tn2.url_id = @url_id
                and tn.publication_status_id in 
                (
                    select 
                    id 
                    from accessible_publication_status 
                    where tenant_id = tn.tenant_id 
                    and (
                        subgroup_id = tn.subgroup_id 
                        or subgroup_id is null and tn.subgroup_id is null
                    )
                )
            ) an
        )
        """;

    const string SUPERTOPICS_DOCUMENT = """
        supertopics_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Title', "name", 
                        'Path', url_path
                    )
                ) "document"
            from(
                select
                    n.title "name",
                    case 
                        when tn.url_path is null then '/node/' || tn.url_id
                        else '/' || tn.url_path
                    end url_path
                from tenant_node tn
        		join tenant tt on tt.id = tn.tenant_id
                join node n on n.id = tn.node_id
                join system_group sg on sg.id = 0
        		join term t1 on t1.nameable_id =  n.id and t1.vocabulary_id = sg.vocabulary_id_tagging
        		join term_hierarchy th on th.term_id_parent = t1.id
        		join term t2 on t2.id = th.term_id_child
        		join tenant_node tn2 on tn2.tenant_id = tn.tenant_id and tn2.node_id = t2.nameable_id
                WHERE tn.tenant_id = @tenant_id AND tn2.url_id = @url_id
                and tn.publication_status_id in 
                (
                    select 
                    id 
                    from accessible_publication_status 
                    where tenant_id = tn.tenant_id 
                    and (
                        subgroup_id = tn.subgroup_id 
                        or subgroup_id is null and tn.subgroup_id is null
                    )
                )
            ) an
        )
        """;

    const string CASE_CASE_PARTIES_DOCUMENT = """
        case_case_parties_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'PartyTypeName', "name",
                        'OrganizationsText', organizations_text,
                        'PersonsText', persons_text,
                        'Organizations', organizations,
                        'Persons', persons
                    )
                ) document
            from(
                select
                    t.name,
                    cp.organizations organizations_text,
                    cp.persons persons_text,
                    (
                        select
                            jsonb_agg(
                                jsonb_build_object(
                                    'Title', organization_name,
                                    'Path', organization_path
                                )
                            ) organizations
                        from(
                            select
                                case_parties_id,
                                n2.title organization_name,
                                case 
                                    when tn2.url_path is null then '/node/' || tn2.url_id
                                    else tn2.url_path
                                end organization_path
                            from case_parties_organization cpo 
                            join node n2 on n2.id = cpo.organization_id
                            join tenant_node tn2 on tn2.node_id = n2.id
                            where tn2.tenant_id = @tenant_id and cpo.case_parties_id = cp.id
                            and tn2.publication_status_id in 
                            (
                                select 
                                id 
                                from accessible_publication_status 
                                where tenant_id = tn2.tenant_id 
                                and (
                                    subgroup_id = tn2.subgroup_id 
                                    or subgroup_id is null and tn2.subgroup_id is null
                                )
                            )
                        ) x 
                    ) organizations,
                    (
                        select
                            jsonb_agg(jsonb_build_object(
                                'Title', person_name,
                                'Path', person_path
                            )) persons
                        from(
                            select
                                case_parties_id,
                                n3.title person_name,
                                case 
                                    when tn3.url_path is null then '/node/' || tn3.url_id
                                    else tn3.url_path
                                end person_path
                            from case_parties_person cpp 
                            join node n3 on n3.id = cpp.person_id
                            join tenant_node tn3 on tn3.node_id = n3.id
                            where tn3.tenant_id = @tenant_id and cpp.case_parties_id = cp.id
                            and tn3.publication_status_id in 
                            (
                                select 
                                id 
                                from accessible_publication_status 
                                where tenant_id = tn3.tenant_id 
                                and (
                                    subgroup_id = tn3.subgroup_id 
                                    or subgroup_id is null and tn3.subgroup_id is null
                                )
                            )
                        ) x 
                    ) persons
                from node n
                join case_case_parties ccp on ccp.case_id = n.id
                join case_parties cp on cp.id = ccp.case_parties_id
                join case_party_type cpt on cpt.id = ccp.case_party_type_id
                join term t on t.nameable_id = cpt.id
                join tenant_node tn1 on tn1.node_id = t.vocabulary_id and tn1.tenant_id = 1 and tn1.url_id = 156
                join tenant_node tn on tn.node_id = n.id
                where tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) x
        )
        """;

    const string LOCATIONS_DOCUMENT = """
        locations_document as(
            select
                jsonb_agg(jsonb_build_object(
        			'Id', "id",
        			'Street', street,
        			'Additional', additional,
        			'City', city,
        			'PostalCode', postal_code,
        			'Subdivision', subdivision,
        			'Country', country,
                    'Latitude', latitude,
                    'Longitude', longitude
        		)) document
            from(
                select 
                l.id,
                l.street,
                l.additional,
                l.city,
                l.postal_code,
                jsonb_build_object(
                	'Path', case when tn2.url_path is null then '/node/' || tn2.url_id else '/' || tn2.url_path end,
                	'Title', s.name
                ) subdivision,
                jsonb_build_object(
                	'Path', case when tn3.url_path is null then '/node/' || tn3.url_id else '/' || tn3.url_path end,
                	'Title', nc.title
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
                    case 
                        when number_of_children_involved is null then 0 
                        else number_of_children_involved 
                    end number_of_children_involved,
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
                        case 
                            when r.number_of_children_involved is null then 0
                            else r.number_of_children_involved
                        end number_of_children_involved,
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
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn3.tenant_id = @tenant_id
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
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn3.tenant_id = @tenant_id
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
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn3.tenant_id = @tenant_id
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
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn3.tenant_id = @tenant_id
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
        		    case 
        			    when person_path_from is null then '/node/' || person_id_from
        			    else '/' || person_path_from
        		    end person_path_from,
        		    person_name_to,
        		    case 
        			    when person_path_to is null then '/node/' || person_id_to
        			    else '/' || person_path_to
        		    end person_path_to,
        		    inter_personal_relation_type_name,
        		    case 
        			    when inter_personal_relation_type_path is null then '/node/' || inter_personal_relation_type_id
        			    else '/' || inter_personal_relation_type_path
        		    end inter_personal_relation_type_path,
        		    date_range,
        		    direction
        		    from(
        			    select
        			    distinct
        			    tn1.url_id person_id_from,
        			    tn1.url_path person_path_from,
        			    n1.title person_name_from,
        			    tn2.url_id person_id_to,
        			    tn2.url_path person_path_to,
        			    n2.title person_name_to,
        			    tn3.url_id inter_personal_relation_type_id,
        			    tn3.url_path inter_personal_relation_type_path,
        			    n3.title inter_personal_relation_type_name,
        			    r.date_range,
        			    1 direction
        			    from node n
        			    join inter_personal_relation r on r.id = n.id
        			    join inter_personal_relation_type rt on rt.id = r.inter_personal_relation_type_id
        			    join node n3 on n3.id = rt.id
        			    join node n1 on n1.id = r.person_id_from
        			    join person o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.person_id_to
        			    join person o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id and tn1.url_id = @url_id
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn3.tenant_id = @tenant_id
        			    where rt.is_symmetric
        			    union 
        			    select
        			    distinct
        			    tn2.url_id person_id_to,
        			    tn2.url_path person_path_to,
        			    n2.title person_name_to,
        			    tn1.url_id person_id_from,
        			    tn1.url_path person_path_from,
        			    n1.title person_name_from,
        			    tn3.url_id inter_personal_relation_type_id,
        			    tn3.url_path inter_personal_relation_type_path,
        			    n3.title inter_personal_relation_type_name,
        			    r.date_range,
        			    1 direction
        			    from node n
        			    join inter_personal_relation r on r.id = n.id
        			    join inter_personal_relation_type rt on rt.id = r.inter_personal_relation_type_id
        			    join node n3 on n3.id = rt.id
        			    join node n1 on n1.id = r.person_id_from
        			    join person o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.person_id_to
        			    join person o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id and tn2.url_id = @url_id
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn3.tenant_id = @tenant_id
        			    where rt.is_symmetric
        			    union 
        			    select
        			    distinct
        			    tn1.url_id person_id_from,
        			    tn1.url_path person_path_from,
        			    n1.title person_name_from,
        			    tn2.url_id person_id_to,
        			    tn2.url_path person_path_to,
        			    n2.title person_name_to,
        			    tn3.url_id inter_person_relational_type_id,
        			    tn3.url_path inter_personal_relation_type_path,
        			    n3.title inter_personal_relation_type_name,
        			    r.date_range,
        			    2 direction
        			    from node n
        			    join inter_personal_relation r on r.id = n.id
        			    join inter_personal_relation_type rt on rt.id = r.inter_personal_relation_type_id
        			    join node n3 on n3.id = rt.id
        			    join node n1 on n1.id = r.person_id_from
        			    join person o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.person_id_to
        			    join person o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id and tn1.url_id = @url_id
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id 
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn3.tenant_id = @tenant_id
        			    where not rt.is_symmetric
        			    union 
        			    select
        			    distinct
        			    tn1.url_id person_id_from,
        			    tn1.url_path person_path_from,
        			    n1.title person_name_from,
        			    tn2.url_id person_id_to,
        			    tn2.url_path person_path_to,
        			    n2.title person_name_to,
        			    tn3.url_id inter_person_relation_type_id,
        			    tn3.url_path inter_person_relation_type_path,
        			    n3.title inter_person_relation_type_name,
        			    r.date_range,
        			    3 direction
        			    from node n
        			    join inter_personal_relation r on r.id = n.id
        			    join inter_personal_relation_type rt on rt.id = r.inter_personal_relation_type_id
        			    join node n3 on n3.id = rt.id
        			    join node n1 on n1.id = r.person_id_from
        			    join person o1 on o1.id = n1.id
        			    join node n2 on n2.id = r.person_id_to
        			    join person o2 on o2.id = n2.id
        			    join tenant_node tn1 on tn1.node_id = o1.id and tn1.tenant_id = @tenant_id 
        			    join tenant_node tn2 on tn2.node_id = o2.id and tn2.tenant_id = @tenant_id and tn2.url_id = @url_id
        			    join tenant_node tn3 on tn3.node_id = rt.id and tn3.tenant_id = @tenant_id
        			    where not rt.is_symmetric
        		    ) x
        	    ) x
            )x
        )
        """;

    const string SEE_ALSO_DOCUMENT = """
        see_also_document AS(
            SELECT
                jsonb_agg(
                    jsonb_build_object(
                        'Path', sa.path,
                        'Title', sa.title
                    )
                ) document
            FROM (
                SELECT 
                    case 
                        when tn.url_path is null then '/node/' || tn.url_id
                        else '/' || tn.url_path
                    end path,
                    n2.title
                FROM node n
                JOIN tenant_node tn2 on tn2.node_id = n.id 
                JOIN node_term nt1 on nt1.node_id = n.id
                JOIN node_term nt2 on nt2.term_id = nt1.term_id and nt2.node_id <> nt1.node_id
                JOIN tenant_node tn on tn.node_id = nt2.node_id and tn.tenant_id = @tenant_id and tn.publication_status_id = 1
                JOIN node n2 on n2.id = tn.node_id
                where tn2.tenant_id = @tenant_id and tn2.url_id = @url_id
                GROUP BY tn.node_id, tn.url_path, tn.url_id, n2.title
                HAVING COUNT(tn.node_id) > 2 
                ORDER BY count(tn.node_id) desc, n2.title
                LIMIT 10
            ) sa
        )
        """;

    const string TAGS_DOCUMENT = """
        tags_document AS (
            SELECT
                jsonb_agg(
                    jsonb_build_object(
                        'Path',  
                        t.path,
                        'Title', 
                        t.name,
                        'NodeTypeName', 
                        t.node_type_name
                    )
                ) as document
            FROM (
                select
                path,
                name,
                node_type_name
                from
                (
                    select
                        case 
                            when tn.url_path is null then '/node/' || tn.url_id
                            else '/' || tn.url_path
                        end path,
                        t.name,
                        case
                            when nmt.tag_label_name is not null then nmt.tag_label_name
                            else nt2.name
                        end node_type_name
                    FROM node_term nt 
                    JOIN tenant_node tn2 on tn2.node_id = nt.node_id
                    JOIN term t on t.id = nt.term_id
                    join node n on n.id = t.nameable_id
                    join node_type nt2 on nt2.id = n.node_type_id
                    left join nameable_type nmt on nmt.id = n.node_type_id and nmt.tag_label_name is not null
                    JOIN tenant_node tn on tn.node_id = t.nameable_id and tn.tenant_id = @tenant_id
                    WHERE tn2.url_id = @url_id and tn2.tenant_id = @tenant_id
                    and tn.publication_status_id in 
                    (
                        select 
                        id 
                        from accessible_publication_status 
                        where tenant_id = tn.tenant_id 
                        and (
                            subgroup_id = tn.subgroup_id 
                            or subgroup_id is null and tn.subgroup_id is null
                        )
                    )
                ) t
            ) t
        )
        """;

    const string TAGS_DOCUMENT_ABUSE_CASE = """
        tags_document_abuse_case AS (
            SELECT
                jsonb_agg(
                    jsonb_build_object(
                        'Path',  
                        t.path,
                        'Title', 
                        t.name,
                        'NodeTypeName', 
                        t.node_type_name
                    )
                ) as document
            FROM (
                select
                    case 
                        when tn2.url_path is null then '/node/' || tn2.url_id
                        else '/' || tn2.url_path
                    end path,
                    t.name,
                    case
                        when nmt.tag_label_name is not null then nmt.tag_label_name
                        else nt2.name
                    end node_type_name
                FROM (
                    select
                    distinct
                    *
                    from(
                        select
                        actoa.abuse_case_id node_id,
                        t.id term_id
                        from type_of_abuse ta
                        join term t on t.nameable_id = ta.id
                        join vocabulary v on v.id = t.vocabulary_id
                        join abuse_case_type_of_abuser actoa on actoa.type_of_abuser_id = ta.id
                        where v.name = 'Topics'
                        union
                        select
                        actoa.abuse_case_id node_id,
                        t.id term_id
                        from type_of_abuse ta
                        join term t on t.nameable_id = ta.id
                        join vocabulary v on v.id = t.vocabulary_id
                        join abuse_case_type_of_abuse actoa on actoa.type_of_abuse_id = ta.id
                        where v.name = 'Topics'
                        union
                        select
                        node_id,
                        term_id
                        from node_term
                    ) x
                ) nt 
                JOIN tenant_node tn on tn.node_id = nt.node_id
                JOIN term t on t.id = nt.term_id
                join node n on n.id = t.nameable_id
                join node_type nt2 on nt2.id = n.node_type_id
                left join nameable_type nmt on nmt.id = n.node_type_id and nmt.tag_label_name is not null
                JOIN tenant_node tn2 on tn2.node_id = t.nameable_id and tn2.tenant_id = @tenant_id and tn2.publication_status_id = 1
                WHERE tn.url_id = @url_id and tn.tenant_id = @tenant_id and tn.publication_status_id = 1
            ) t
        )
        """;

    const string ORGANIZATIONS_OF_COUNTRY_DOCUMENT = """
        organizations_of_country_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
        	            'OrganizationTypeName', organization_type,
        	            'Organizations', organizations
                    )
                ) document
            from(
                select
        	        organization_type,
        	        jsonb_agg(
        	            jsonb_build_object(
        		            'Title', organization_name,
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
                        where tn2.publication_status_id in 
                        (
                            select 
                            id 
                            from accessible_publication_status 
                            where tenant_id = tn2.tenant_id 
                            and (
                                subgroup_id = tn2.subgroup_id 
                                or subgroup_id is null and tn2.subgroup_id is null
                            )
                        )
        	        ) x
                group by x.organization_type
            ) x
        )
        """;

    const string ORGANIZATIONS_OF_SUBDIVISION_DOCUMENT = """
        organizations_of_subdivision_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
        	            'OrganizationTypeName', organization_type,
        	            'Organizations', organizations
                    )
                ) document
            from(
                select
        	        organization_type,
        	        jsonb_agg(
        	            jsonb_build_object(
        		            'Title', organization_name,
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
                    join tenant_node tn on tn.url_id = @url_id and tn.node_id = l.subdivision_id and tn.tenant_id = @tenant_id
        	        ) x
                group by x.organization_type
            ) x
        )
        """;

    const string DOCUMENTS_DOCUMENT = """
        documents_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Path', path,
                        'Title', title,
                        'PublicationDateFrom', publication_date_from,
                        'PublicationDateTo', publication_date_to,
                        'SortOrder', sort_order
                    )
                    order by sort_order desc nulls last
                ) document
            from(
                select
                    distinct
                    path,
                    title,
                    publication_date_from,
                    publication_date_to,
                    row_number() over(order by sort_date desc nulls last) sort_order
                from(
                    select
                        distinct
                        case 
        	                when tn.url_path is null then '/node/' || tn.url_id
        	                else '/' || tn.url_path
                        end path,
                        n2.title,
        	            lower(d.published) sort_date,
                        lower(d.published) publication_date_from,
                        upper(d.published) publication_date_to
                    from node_term nt
                    join term t on t.id = nt.term_id
                    join node n on n.id = t.nameable_id
                    join tenant_node tn2 on tn2.node_id = n.id
                    join tenant_node tn on tn.node_id = nt.node_id and tn.tenant_id = @tenant_id
                    join node n2 on n2.Id = tn.node_id
                    join "document" d on d.id = n2.id
                    where n.id in (select nwd.id from node_with_descendants nwd)
                    and tn.publication_status_id in 
                    (
                        select 
                        id 
                        from accessible_publication_status 
                        where tenant_id = tn.tenant_id 
                        and (
                            subgroup_id = tn.subgroup_id 
                            or subgroup_id is null and tn.subgroup_id is null
                        )
                    )
                ) x
            ) docs
        )
        """;

    const string BLOG_POST_BREADCRUM_DOCUMENT = """
        blog_post_bread_crum_document AS (
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
                    '/blogs', 
                    'blogs', 
                    1
                UNION
                SELECT 
                    '/blog/' || p.id, 
                    p.name || '''s blog', 
                    2
                FROM node n
                join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id and tn.url_id = @url_id
                JOIN publisher p on p.id = n.publisher_id
                WHERE tn.url_id = @url_id and tn.tenant_id = @tenant_id
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;

    const string DISCUSSION_BREADCRUM_DOCUMENT = """
        discussion_bread_crum_document AS (
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
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;

    const string PAGE_BREADCRUM_DOCUMENT = """
        page_bread_crum_document AS (
            SELECT jsonb_agg(
                jsonb_build_object(
                    'Path', url,
                    'Title', "name"
                ) 
            ) document
            FROM(
                SELECT 
                    '/' url, 
                    'Home' "name", 
                    0 "order"
            ) bces
        )
        """;

    const string DOCUMENT_BREADCRUM_DOCUMENT = """
        document_bread_crum_document AS (
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
                    '/documents', 
                    'documents', 
                    1
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string TOPICS_BREADCRUM_DOCUMENT = """
        topics_bread_crum_document AS (
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
    const string PERSON_BREADCRUM_DOCUMENT = """
        person_bread_crum_document AS (
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

    const string COUNTRY_SUBDIVISIONS_DOCUMENT = """
        country_subdivisions_document as (
            select
        	    jsonb_agg(jsonb_build_object(
        		    'Name', subdivision_type_name,
        		    'Subdivisions', subdivisions
        	    )) document
        	from(
                select
        	        subdivision_type_name,
        	        jsonb_agg(jsonb_build_object(
        		        'Title', subdivision_name,
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
        	        join tenant_node tn on tn.node_id = c.id and tn.tenant_id = @tenant_id and tn.url_id = @url_id
        	        join tenant t on t.id = tn.tenant_id
        	        join subdivision s on s.country_id = c.id
        	        join node n on n.id = s.subdivision_type_id
        	        join tenant_node tn2 on tn2.node_id = s.id and tn2.tenant_id = t.id
                    join system_group sg on sg.id = 0
        	        join term tp on tp.nameable_id = c.id and tp.vocabulary_id = sg.vocabulary_id_tagging
        	        join term tc on tc.nameable_id = s.id and tc.vocabulary_id = sg.vocabulary_id_tagging
        	        join term_hierarchy th on th.term_id_parent = tp.id and th.term_id_child = tc.id
                ) x
                GROUP BY subdivision_type_name
            ) x
        )
        """;
    const string SUBDIVISION_SUBDIVISIONS_DOCUMENT = """
        subdivision_subdivisions_document as (
            select
        	    jsonb_agg(jsonb_build_object(
        		    'Name', subdivision_type_name,
        		    'Subdivisions', subdivisions
        	    )) document
        	from(
                select
        	        subdivision_type_name,
        	        jsonb_agg(jsonb_build_object(
        		        'Title', subdivision_name,
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
        	        from subdivision sd
        	        join tenant_node tn on tn.node_id = sd.id and tn.tenant_id = @tenant_id and tn.url_id = @url_id
        	        join tenant t on t.id = tn.tenant_id
        	        join basic_second_level_subdivision bsls on bsls.intermediate_level_subdivision_id = sd.id
                    join subdivision s on s.id = bsls.id
        	        join node n on n.id = s.subdivision_type_id
        	        join tenant_node tn2 on tn2.node_id = s.id and tn2.tenant_id = @tenant_id
                    join system_group sg on sg.id = 0
        	        join term tp on tp.nameable_id = sd.id and tp.vocabulary_id = sg.vocabulary_id_tagging
        	        join term tc on tc.nameable_id = s.id and tc.vocabulary_id = sg.vocabulary_id_tagging
        	        join term_hierarchy th on th.term_id_parent = tp.id and th.term_id_child = tc.id
                ) x
                GROUP BY subdivision_type_name
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
                    case 
                        when tn2.url_path is null then '/node/' || tn2.url_id
                        else '/' || tn2.url_path
                    end  bill_action_path,
                    n2.title bill_action_name,
                    case 
                        when tn3.url_path is null then '/node/' || tn3.url_id
                        else '/' || tn3.url_path
                    end bill_path,
                    n3.title bill_name,
                    ba.date
                from node n
                join tenant_node tn on tn.node_id = n.id
                join professional_role pr on pr.person_id = n.id
                join representative_house_bill_action ba on ba.representative_id = pr.id
                join node n2 on n2.id = ba.bill_action_type_id
                join node n3 on n3.id = ba.house_bill_id
                join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = tn.tenant_id
                join tenant_node tn3 on tn3.node_id = n3.id and tn3.tenant_id = tn.tenant_id
                where tn.url_id = @url_id and tn.tenant_id = @tenant_id
                union
                select
                    case 
                        when tn2.url_path is null then '/node/' || tn2.url_id
                        else '/' || tn2.url_path
                    end bill_action_path,
                    n2.title bill_action_name,
                    case 
                        when tn3.url_path is null then '/node/' || tn3.url_id
                        else '/' || tn3.url_path
                    end bill_path,
                    n3.title bill_name,
                    ba.date
                from node n
                join tenant_node tn on tn.node_id = n.id
                join professional_role pr on pr.person_id = n.id
                join senator_senate_bill_action ba on ba.senator_id = pr.id
                join node n2 on n2.id = ba.bill_action_type_id
                join node n3 on n3.id = ba.senate_bill_id
                join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = tn.tenant_id
                join tenant_node tn3 on tn3.node_id = n3.id and tn3.tenant_id = tn.tenant_id
                where tn.url_id = @url_id and tn.tenant_id = @tenant_id
            )x
        )
        """;

    const string COUNTRY_BREADCRUM_DOCUMENT = """
        country_bread_crum_document AS (
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
                    '/countries', 
                    'countries', 
                    1
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;

    const string SUBDIVISION_BREADCRUM_DOCUMENT = """
        subdivision_bread_crum_document AS (
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
                    
                    '/countries', 
                    'countries', 
                    1
                UNION
                SELECT 
                    case
                        when tn.url_path is null then '/node/' || tn.url_id
                        else tn.url_path
                    end url,
                    n.title, 
                    2
                    from subdivision s
                    join country c on c.id = s.country_id
                    join node n on n.id = c.id
                    join tenant_node tn on tn.node_id = c.id and tn.tenant_id = 1 and tn.url_id = @url_id
        
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;

    const string GLOBAL_REGION_BREADCRUM_DOCUMENT = """
        global_region_bread_crum_document AS (
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
                ) bce
            ) bces
        )
        """;

    const string ABUSE_CASE_BREADCRUM_DOCUMENT = """
        abuse_case_bread_crum_document AS (
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
    const string COERCED_ADOPTION_CASE_BREADCRUM_DOCUMENT = """
        coerced_adoption_case_bread_crum_document AS (
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
                    '/cases', 
                    'Cases', 
                    1
                UNION
                SELECT 
                    '/coerced_adoption_cases', 
                    'Coerced adoption cases', 
                    2
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string DEPORTATION_CASE_BREADCRUM_DOCUMENT = """
        deportation_case_bread_crum_document AS (
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
                    '/cases', 
                    'Cases', 
                    1
                UNION
                SELECT 
                    '/deportation_cases', 
                    'Deportation cases', 
                    2
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string DISRUPTED_PLACEMENT_CASE_BREADCRUM_DOCUMENT = """
        disrupted_placement_case_bread_crum_document AS (
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
                    '/cases', 
                    'Cases', 
                    1
                UNION
                SELECT 
                    '/disrupted_placement_cases', 
                    'Disrupted placememt cases', 
                    2
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string FATHERS_RIGHTS_VIOLATION_CASE_BREADCRUM_DOCUMENT = """
        fathers_rights_violation_case_bread_crum_document AS (
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
                    '/cases', 
                    'Cases', 
                    1
                UNION
                SELECT 
                    '/fathers_rights_violation_cases', 
                    'Father''s rights violation cases', 
                    2
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string WRONGFUL_MEDICATION_CASE_BREADCRUM_DOCUMENT = """
        wrongful_medication_case_bread_crum_document AS (
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
                    '/cases', 
                    'Cases', 
                    1
                UNION
                SELECT 
                    '/wrongful_medication_cases', 
                    'Wrongful medicatiion cases', 
                    2
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string WRONGFUL_REMOVAL_CASE_BREADCRUM_DOCUMENT = """
        wrongful_removal_case_bread_crum_document AS (
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
                    '/cases', 
                    'Cases', 
                    1
                UNION
                SELECT 
                    '/wrongful_removal_cases', 
                    'Wrongful removal cases', 
                    2
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string POLL_BREADCRUM_DOCUMENT = """
        poll_breadcrum_document AS (
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
                    '/cases', 
                    'Cases', 
                    1
                UNION
                SELECT 
                    '/polls', 
                    'Polls', 
                    2
                ) bce
                ORDER BY bce."order"
            ) bces
        )
        """;
    const string COMMENTS_DOCUMENT = """
        comments_document AS (
            SELECT jsonb_agg(tree) document
            FROM (
                SELECT to_jsonb(sub) AS tree
                FROM (
        	        SELECT 
        		        c.id AS "Id", 
        		        c.node_status_id AS "NodeStatusId",
        		        jsonb_build_object(
        			        'Id', p.id, 
        			        'Name', p.name,
                            'CreatedDateTime', c.created_date_time,
                            'ChangedDateTime', c.created_date_time
                        ) AS "Authoring",
        		        c.title AS "Title", 
        		        c.text AS "Text", 
                        case 
                            when c.comment_id_parent is null then 0 
                            else c.comment_id_parent 
                        end "CommentIdParent"
        	        FROM comment c
        	        JOIN publisher p on p.id = c.publisher_id
                    JOIN node n on n.id = c.node_id
                    join tenant_node tn on tn.node_id = n.id
        	        WHERE tn.url_id = @url_id and tn.tenant_id = @tenant_id
                ) sub
        	) agg        
        )
        """;

    const string ADOPTION_IMPORTS_DOCUMENT = """
        adoption_imports_document as(
            select 
                case 
                    when document is null then jsonb_build_object(
                        'StartYear', 2000,
                        'EndYear', 2000,
                        'Imports', null
                     )
                     else document
                end document
            from
            (
                select (select document from (
        		        select
        			        jsonb_build_object(
        				        'StartYear', start_year,
        				        'EndYear', end_year,
        				        'Imports', jsonb_agg(
        					        jsonb_build_object(
        					           'CountryFrom', name,
        						        'RowType', row_type,
        						        'Values', y
        					        )
        				        )
        			        ) document
        		        from(
        			        select
        				        name,
        				        row_type,
        				        start_year,
        				        end_year,
        				        jsonb_agg(
        					        jsonb_build_object(
        						        'Year', "year",
        						        'NumberOfChildren', number_of_children
        					        )
        				        ) y
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
        								        join tenant_node tn2 on tn2.tenant_id = 1 and tn2.url_id = 144
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
        	        ) x
        	    )
            ) x
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
            case 
        	    when tn2.url_path is null then '/node/' || tn2.url_id
        	    else tn2.url_path
            end path	
            from profession ot
            join node n on n.id = ot.id
            join professional_role oot on oot.profession_id = ot.id
            join person o on o.id = oot.person_id
            join tenant_node tn1 on tn1.node_id = o.id and tn1.tenant_id = @tenant_id and tn1.url_id = @url_id
            join tenant_node tn2 on tn2.node_id = ot.id and tn2.tenant_id = @tenant_id 
            ) x
        )
        """;

    const string INFORMAL_SUBDIVISION_DOCUMENT = """
        informal_subdivision_document AS (
            SELECT 
                jsonb_build_object(
                'UrlId', n.url_id,
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
                'Country', jsonb_build_object(
                    'Path', n.country_path,
                    'Title', n.country_name
                ),
                'HasBeenPublished', n.has_been_published,
                'PublicationStatusId', publication_status_id,
                'BreadCrumElements', (SELECT document FROM country_bread_crum_document),
                'Tags', (SELECT document FROM tags_document),
                'CommentListItems', (SELECT document FROM  comments_document),
                'Documents', (SELECT document from documents_document),
                'OrganizationTypes', (SELECT document FROM organizations_of_subdivision_document),
                'SubdivisionTypes', (SELECT document FROM subdivision_subdivisions_document),
                'SubTopics', (SELECT document from subtopics_document),
                'SuperTopics', (SELECT document from supertopics_document),
                'Files', (SELECT document FROM files_document),
                'Cases', (SELECT document FROM nameable_cases_document)
            ) document
            FROM (
                 SELECT
                    tn.url_id, 
                    tn.node_id,
                    n.node_type_id,
                    n.title, 
                    n.created_date_time, 
                    n.changed_date_time, 
                    nm.description, 
                    n.publisher_id, 
                    p.name publisher_name,
                    case when 
                        tn.publication_status_id = 0 then false
                        else true
                    end has_been_published,                  
                    tn.publication_status_id,
                    case 
                        when tn.url_path is null then '/node/' || tn.url_id
                        else '/' || tn.url_path
                    end country_path,
                    n.title country_name,
                    n2.title subdivision_type_name
                FROM node n
                join tenant_node tn on tn.node_id = n.id 
                join subdivision sd on sd.id = n.id 
                join node n2 on n2.id = sd.subdivision_type_id
                join nameable nm on nm.id = n.id
                JOIN publisher p on p.id = n.publisher_id
                where tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) n
        ) 
        """;

    const string FORMAL_SUBDIVISION_DOCUMENT = """
        formal_subdivision_document AS (
            SELECT 
                jsonb_build_object(
                'UrlId', n.url_id,
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
                'Country', jsonb_build_object(
                    'Path', n.country_path,
                    'Title', n.country_name
                ),
                'ISO3166_2_Code', iso_3166_2_code,
                'HasBeenPublished', n.has_been_published,
                'BreadCrumElements', (SELECT document FROM country_bread_crum_document),
                'Tags', (SELECT document FROM tags_document),
                'CommentListItems', (SELECT document FROM  comments_document),
                'Documents', (SELECT document from documents_document),
                'OrganizationTypes', (SELECT document FROM organizations_of_subdivision_document),
                'SubdivisionTypes', (SELECT document FROM subdivision_subdivisions_document),
                'SubTopics', (SELECT document from subtopics_document),
                'SuperTopics', (SELECT document from supertopics_document),
                'Files', (SELECT document FROM files_document),
                'Cases', (SELECT document FROM nameable_cases_document)
            ) document
            FROM (
                 SELECT
                    tn.url_id,  
                    tn.node_id,
                    n.node_type_id,
                    n.title, 
                    n.created_date_time, 
                    n.changed_date_time, 
                    nm.description, 
                    n.publisher_id, 
                    p.name publisher_name,
                    case when 
                        tn.publication_status_id = 0 then false
                        else true
                    end has_been_published,
                    tn.publication_status_id,
                    ics.iso_3166_2_code,
                    case 
                        when tn.url_path is null then '/node/' || tn.url_id
                        else '/' || tn.url_path
                    end country_path,
                    n.title country_name,
                    n2.title subdivision_type_name
                FROM node n
                join subdivision sd on sd.id = n.id 
                join iso_coded_subdivision ics on ics.id = sd.id
                join tenant_node tn on tn.node_id = n.id 
                join node n2 on n2.id = sd.subdivision_type_id
                join nameable nm on nm.id = n.id
                JOIN publisher p on p.id = n.publisher_id
                where tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) n
        ) 
        """;
    const string GLOBAL_REGION_DOCUMENT = """
        global_region_document AS (
            SELECT 
                jsonb_build_object(
                'UrlId', n.url_id,
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
                'BreadCrumElements', (SELECT document FROM global_region_bread_crum_document),
                'Tags', (SELECT document FROM tags_document),
                'CommentListItems', (SELECT document FROM  comments_document),
                'Documents', (SELECT document from documents_document),
                'SubTopics', (SELECT document from subtopics_document),
                'SuperTopics', (SELECT document from supertopics_document),
                'Files', (SELECT document FROM files_document),
                'Cases', (SELECT document FROM nameable_cases_document)
            ) document
            FROM (
                 SELECT
                    tn.url_id,  
                    tn.node_id,
                    n.node_type_id,
                    n.title, 
                    n.created_date_time, 
                    n.changed_date_time, 
                    nm.description, 
                    n.publisher_id, 
                    p.name publisher_name,
                    case when 
                        tn.publication_status_id = 0 then false
                        else true
                    end has_been_published,
                    tn.publication_status_id
                FROM node n
                join global_region tlc on tlc.id = n.id 
                join tenant_node tn on tn.node_id = n.id 
                join nameable nm on nm.id = n.id
                JOIN publisher p on p.id = n.publisher_id
                where tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) n
        ) 
        """;

    const string BASIC_COUNTRY_DOCUMENT = """
        basic_country_document AS (
            SELECT 
                jsonb_build_object(
                'UrlId', n.url_id,
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
                'GlobalRegion', jsonb_build_object(
                    'Path', n.global_region_path,
                    'Title', n.global_region_name
                ),
                'ISO3166_1_Code', iso_3166_1_code,
                'FlagImage', json_build_object(
                    'FilePath',
                    '/files/flags/' || lower(iso_3166_1_code) || '.svg',
                    'Label',
                    n.title
                ),
                'BreadCrumElements', (SELECT document FROM country_bread_crum_document),
                'Tags', (SELECT document FROM tags_document),
                'CommentListItems', (SELECT document FROM  comments_document),
                'AdoptionImports', (SELECT document FROM adoption_imports_document),
                'Documents', (SELECT document from documents_document),
                'OrganizationTypes', (SELECT document FROM organizations_of_country_document),
                'SubdivisionTypes', (SELECT document FROM country_subdivisions_document),
                'SubTopics', (SELECT document from subtopics_document),
                'SuperTopics', (SELECT document from supertopics_document),
                'Files', (SELECT document FROM files_document),
                'Cases', (SELECT document FROM nameable_cases_document)
            ) document
            FROM (
                 SELECT
                    tn.url_id,  
                    tn.node_id,
                    n.node_type_id,
                    n.title, 
                    n.created_date_time, 
                    n.changed_date_time, 
                    nm.description, 
                    n.publisher_id, 
                    p.name publisher_name,
                    case when 
                        tn.publication_status_id = 0 then false
                        else true
                    end has_been_published,
                    tn.publication_status_id,
                    tlc.iso_3166_1_code,
                    case 
                        when tn.url_path is null then '/node/' || tn.url_id
                        else '/' || tn.url_path
                    end global_region_path,
                    n.title global_region_name
                FROM node n
                join top_level_country tlc on tlc.id = n.id 
                join tenant_node tn on tn.node_id = n.id
                join nameable nm on nm.id = n.id
                JOIN publisher p on p.id = n.publisher_id
                where tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) n
        ) 
        """;

    const string COUNTRY_AND_SUBDIVISION_DOCUMENT = """
        country_and_subdivision_document AS (
            SELECT 
                jsonb_build_object(
                'UrlId', n.url_id,
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
                'GlobalRegion', jsonb_build_object(
                    'Path', n.global_region_path,
                    'Title', n.global_region_name
                ),
                'ISO3166_1_Code', iso_3166_1_code,
                'ISO3166_2_Code', iso_3166_2_code,
                'FlagImage', json_build_object(
                    'FilePath',
                    '/files/flags/' || lower(iso_3166_1_code) || '.svg',
                    'Label',
                    n.title
                ),
                'BreadCrumElements', (SELECT document FROM country_bread_crum_document),
                'Tags', (SELECT document FROM tags_document),
                'CommentListItems', (SELECT document FROM  comments_document),
                'AdoptionImports', (SELECT document FROM adoption_imports_document),
                'Documents', (SELECT document from documents_document),
                'OrganizationTypes', (SELECT document FROM organizations_of_country_document),
                'SubdivisionTypes', (SELECT document FROM country_subdivisions_document),
                'SubTopics', (SELECT document from subtopics_document),
                'SuperTopics', (SELECT document from supertopics_document),
                'Files', (SELECT document FROM files_document),
                'Cases', (SELECT document FROM nameable_cases_document)
            ) document
            FROM (
                 SELECT
                    tn.url_id,  
                    tn.node_id,
                    n.node_type_id,
                    n.title, 
                    n.created_date_time, 
                    n.changed_date_time, 
                    nm.description, 
                    n.publisher_id, 
                    p.name publisher_name,
                    case when 
                        tn.publication_status_id = 0 then false
                        else true
                    end has_been_published,
                    tn.publication_status_id,
                    tlc.iso_3166_1_code,
                    sd.iso_3166_2_code,
                    case 
                        when tn.url_path is null then '/node/' || tn.url_id
                        else '/' || tn.url_path
                    end global_region_path,
                    n.title global_region_name
                FROM node n
                join top_level_country tlc on tlc.id = n.id 
                join iso_coded_subdivision sd on sd.id = n.id 
                join tenant_node tn on tn.node_id = n.id
                join nameable nm on nm.id = n.id
                JOIN publisher p on p.id = n.publisher_id
                where tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) n
        ) 
        """;

    const string BINDING_COUNTRY_DOCUMENT = """
        binding_country_document AS (
            SELECT 
                jsonb_build_object(
                'UrlId', n.url_id,
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
                'GlobalRegion', jsonb_build_object(
                    'Path', n.global_region_path,
                    'Title', n.global_region_name
                ),
                'ISO3166_1_Code', iso_3166_1_code,
                'FlagImage', json_build_object(
                    'FilePath',
                    '/files/flags/gb.svg'
                    'Label',
                    n.title
                ),
                'BreadCrumElements', (SELECT document FROM country_bread_crum_document),
                'Tags', (SELECT document FROM tags_document),
                'CommentListItems', (SELECT document FROM  comments_document),
                'AdoptionImports', (SELECT document FROM adoption_imports_document),
                'Documents', (SELECT document from documents_document),
                'OrganizationTypes', (SELECT document FROM organizations_of_country_document),
                'SubdivisionTypes', (SELECT document FROM country_subdivisions_document),
                'SubTopics', (SELECT document from subtopics_document),
                'SuperTopics', (SELECT document from supertopics_document),
                'Files', (SELECT document FROM files_document),
                'BoundCountries', null,
                'Cases', (SELECT document FROM nameable_cases_document)
            ) document
            FROM (
                 SELECT
                    tn.url_id,  
                    tn.node_id,
                    n.node_type_id,
                    n.title, 
                    n.created_date_time, 
                    n.changed_date_time, 
                    nm.description, 
                    n.publisher_id, 
                    p.name publisher_name,
                    case when 
                        tn.publication_status_id = 0 then false
                        else true
                    end has_been_published,
                    tn.publication_status_id,
                    tlc.iso_3166_1_code,
                    case 
                        when tn.url_path is null then '/node/' || tn.url_id
                        else '/' || tn.url_path
                    end global_region_path,
                    n.title global_region_name
                FROM node n
                join top_level_country tlc on tlc.id = n.id 
                join tenant_node tn on tn.node_id = n.id
                join nameable nm on nm.id = n.id
                JOIN publisher p on p.id = n.publisher_id
                where tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) n
        ) 
        """;

    const string BOUND_COUNTRY_DOCUMENT = """
        bound_country_document AS (
            SELECT 
                jsonb_build_object(
                'UrlId', n.url_id,
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
                'BindingCountry', jsonb_build_object(
                    'Path', n.binding_country_path,
                    'Title', n.binding_country_name
                ),
                'ISO3166_2_Code', iso_3166_2_code,
                'FlagImage', json_build_object(
                    'FilePath',
                    '/files/flags/' || lower(iso_3166_2_code) || '.svg',
                    'Label',
                    n.title
                ),
                'BreadCrumElements', (SELECT document FROM country_bread_crum_document),
                'Tags', (SELECT document FROM tags_document),
                'CommentListItems', (SELECT document FROM  comments_document),
                'AdoptionImports', (SELECT document FROM adoption_imports_document),
                'Documents', (SELECT document from documents_document),
                'OrganizationTypes', (SELECT document FROM organizations_of_country_document),
                'SubdivisionTypes', (SELECT document FROM country_subdivisions_document),
                'SubTopics', (SELECT document from subtopics_document),
                'SuperTopics', (SELECT document from supertopics_document),
                'Files', (SELECT document FROM files_document),
                'Cases', (SELECT document FROM nameable_cases_document)
            ) document
            FROM (
                 SELECT
                    tn.url_id,  
                    tn.node_id,
                    n.node_type_id,
                    n.title, 
                    n.created_date_time, 
                    n.changed_date_time, 
                    nm.description, 
                    n.publisher_id, 
                    p.name publisher_name,
                    case when 
                        tn.publication_status_id = 0 then false
                        else true
                    end has_been_published,
                    tn.publication_status_id,
                    ics.iso_3166_2_code,
                    case 
                        when tn.url_path is null then '/node/' || tn.url_id
                        else '/' || tn.url_path
                    end binding_country_path,
                    n.title binding_country_name
                FROM node n
                join bound_country bc on bc.id = n.id 
                join iso_coded_subdivision ics on ics.id = bc.id
                join tenant_node tn on tn.node_id = n.id
                join nameable nm on nm.id = n.id
                JOIN publisher p on p.id = n.publisher_id
                where tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) n
        ) 
        """;

    const string DOCUMENT_DOCUMENT = """
        document_document AS (
            SELECT 
                jsonb_build_object(
                    'UrlId', n.url_id,
                    'NodeId', n.node_id,
                    'NodeTypeId', n.node_type_id,
                    'Title', n.title,
                    'Text', n.text,
                    'HasBeenPublished', n.has_been_published,
                    'PublicationStatusId', publication_status_id,
                    'Authoring', jsonb_build_object(
                        'Id', n.publisher_id,
                        'Name', n.publisher_name,
                        'CreatedDateTime', n.created_date_time,
                        'ChangedDateTime', n.changed_date_time
                    ),
                    'Published', published,
                    'SourceUrl', source_url,
                    'DocumentType', document_type,
                    'BreadCrumElements', (SELECT document FROM document_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'Files', (SELECT document FROM files_document)
            ) document
            FROM(
                SELECT
                    tn.url_id, 
                    tn.node_id,
                    n.node_type_id,
                    n.title,
                    n.created_date_time,
                    n.changed_date_time,
                    stn.text,
                    n.publisher_id,
                    p.name publisher_name,
                    case 
                        when tn.publication_status_id = 0 then false
                        else true
                    end has_been_published,
                    tn.publication_status_id,
                    d.published,
                    d.source_url,
                    case 
                        when dt.id is null then null
                        else jsonb_build_object(
                            'Id', dt.id,
                            'Title', dt.name,
                            'Path', dt.path
                        )
                    end document_type
                FROM node n
                join tenant_node tn on tn.node_id = n.id 
                join document d on d.id = n.id
                join simple_text_node stn on stn.id = n.id
                left join (
                    select 
                    dt.id,
                    n.title name,
                    case when tn.url_path is null then '/node/' || tn.url_id
        	            else '/' || tn.url_path
                    end path
                    from document_type dt
                    join node n on n.id = dt.id
                    join tenant_node tn on tn.node_id = dt.id and tn.tenant_id = @tenant_id
                ) dt on dt.id = d.document_type_id
                JOIN publisher p on p.id = n.publisher_id
                where tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) n
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
        			case 
        				when tn.url_path is null then '/node/' || tn.url_id
        				else '/' || tn.url_path
        			end path
        		from  node n
        		join person pe  on pe.id = n.id
        		join person_organization_relation por on por.person_id = pe.Id
        		join node n2 on n2.id = por.person_organization_relation_type_id
        		join tenant_node tn2 on tn2.node_id = por.organization_id
        		join tenant_node tn on tn.node_id = n.id and tn.tenant_id = tn2.tenant_id
        		where tn2.tenant_id = @tenant_id and tn2.url_id = @url_id
                and tn.publication_status_id in 
                (
                    select 
                    id 
                    from accessible_publication_status 
                    where tenant_id = tn.tenant_id 
                    and (
                        subgroup_id = tn.subgroup_id 
                        or subgroup_id is null and tn.subgroup_id is null
                    )
                )
        	) x
        )
        """;
    const string ORGANIZATION_PERSON_RELATIONS_DOCUMENT = """
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
        			case 
        				when tn.url_path is null then '/node/' || tn.url_id
        				else '/' || tn.url_path
        			end path
        		from  node n
        		join organization pe  on pe.id = n.id
        		join person_organization_relation por on por.organization_id = pe.Id
        		join node n2 on n2.id = por.person_organization_relation_type_id
        		join tenant_node tn2 on tn2.node_id = por.person_id
        		join tenant_node tn on tn.node_id = n.id and tn.tenant_id = tn2.tenant_id
        		where tn2.tenant_id = @tenant_id and tn2.url_id = @url_id
                and tn.publication_status_id in 
                (
                    select 
                    id 
                    from accessible_publication_status 
                    where tenant_id = tn.tenant_id 
                    and (
                        subgroup_id = tn.subgroup_id 
                        or subgroup_id is null and tn.subgroup_id is null
                    )
                )
        	) x
        )
        """;

    const string ORGANIZATION_DOCUMENT = """
        organization_document AS (
            SELECT 
                jsonb_build_object(
                'UrlId', n.url_id,
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
                'BreadCrumElements', (SELECT document FROM organization_bread_crum_document),
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
                    tn.url_id,  
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
                join nameable nm on nm.id = n.id
                JOIN publisher p on p.id = n.publisher_id
                where tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) n
        ) 
        """;

    const string PERSON_DOCUMENT = """
        person_document AS (
            SELECT 
                jsonb_build_object(
                'UrlId', n.url_id,
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
                'BreadCrumElements', (SELECT document FROM person_bread_crum_document),
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
                'BillActions', (SELECT document from bill_actions_document)
            ) document
            FROM (
                 SELECT
                    tn.url_id,  
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
                where tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) n
        ) 
        """;

    const string BLOG_POST_DOCUMENT = """
        blog_post_document AS (
            SELECT 
                jsonb_build_object(
                'UrlId', 
                url_id,
                'NodeId', 
                node_id,
                'NodeTypeId', 
                node_type_id,
                'Title', 
                title, 
                'Text', 
                text,
                'HasBeenPublished', 
                has_been_published,
                'PublicationStatusId', 
                publication_status_id,
                'Authoring', 
                jsonb_build_object(
                    'Id', publisher_id, 
                    'Name', publisher_name,
                    'CreatedDateTime', created_date_time,
                    'ChangedDateTime', changed_date_time
                ),
                'BreadCrumElements', 
                (SELECT document FROM blog_post_bread_crum_document),
                'Tags', 
                (SELECT document FROM tags_document),
                'SeeAlsoBoxElements', 
                (SELECT document FROM see_also_document),
                'CommentListItems', 
                (SELECT document FROM  comments_document),
                'Files', 
                (SELECT document FROM files_document)
            ) document
            FROM (
                SELECT
                    tn.url_id,  
                    tn.node_id,
                    n.node_type_id,
                    n.title, 
                    n.created_date_time, 
                    n.changed_date_time, 
                    stn.text, 
                    n.publisher_id, 
                    p.name publisher_name,
                    case 
                        when tn.publication_status_id = 0 then false
                        else true
                    end has_been_published,
                    tn.publication_status_id
                FROM node n
                join tenant_node tn on tn.node_id = n.id 
                join simple_text_node stn on stn.id = n.id 
                JOIN publisher p on p.id = n.publisher_id
                where tn.url_id = @url_id and tn.tenant_id = @tenant_id
            ) n
        ) 
        """;

    const string SINGLE_QUESTION_POLL_DOCUMENT = """
        single_question_poll_document AS (
            SELECT 
                jsonb_build_object(
                'UrlId', n.url_id,
                'NodeId', n.node_id,
                'NodeTypeId', n.node_type_id,
                'Title', n.title, 
                'Text', n.text,
                'HasBeenPublished', n.has_been_published,
                'PublicationStatusId', publication_status_id,
                'Authoring', jsonb_build_object(
                    'Id', n.publisher_id, 
                    'Name', n.publisher_name,
                    'CreatedDateTime', n.created_date_time,
                    'ChangedDateTime', n.changed_date_time
                ),
                'Question', question,
                'DateTimeClosure', date_time_closure,
                'PollStatusId', poll_status_id,
                'BreadCrumElements', (SELECT document FROM poll_breadcrum_document),
                'Tags', (SELECT document FROM tags_document),
                'SeeAlsoBoxElements', (SELECT document FROM see_also_document),
                'CommentListItems', (SELECT document FROM  comments_document),
                'Files', (SELECT document FROM files_document),
                'PollOptions', (SELECT document FROM poll_options_document)
            ) document
            FROM (
                SELECT
                    tn.url_id,  
                    tn.node_id,
                    n.node_type_id,
                    n.title, 
                    n.created_date_time, 
                    n.changed_date_time, 
                    stn.text, 
                    n.publisher_id, 
                    p.name publisher_name,
                    case 
                        when tn.publication_status_id = 0 then false
                        else true
                    end has_been_published,
                    tn.publication_status_id,
                    pq.question,
                    pl.date_time_closure,
                    pl.poll_status_id
                FROM node n
                join tenant_node tn on tn.node_id = n.id
                join simple_text_node stn on stn.id = n.id 
                join poll pl on pl.id = n.id 
                join poll_question pq on pq.id = pl.id
                JOIN publisher p on p.id = n.publisher_id
                where tn.url_id = @url_id and tn.tenant_id = @tenant_id
            ) n
        ) 
        """;

    const string MULTIPLE_QUESTION_POLL_DOCUMENT = """
        multi_question_poll_document AS (
            SELECT 
                jsonb_build_object(
                'UrlId', n.url_id,
                'NodeId', n.node_id,
                'NodeTypeId', n.node_type_id,
                'Title', n.title, 
                'Text', n.text,
                'HasBeenPublished', n.has_been_published,
                'PublicationStatusId', publication_status_id,
                'Authoring', jsonb_build_object(
                    'Id', n.publisher_id, 
                    'Name', n.publisher_name,
                    'CreatedDateTime', n.created_date_time,
                    'ChangedDateTime', n.changed_date_time
                ),
                'DateTimeClosure', date_time_closure,
                'PollStatusId', poll_status_id,
                'BreadCrumElements', (SELECT document FROM poll_breadcrum_document),
                'Tags', (SELECT document FROM tags_document),
                'SeeAlsoBoxElements', (SELECT document FROM see_also_document),
                'CommentListItems', (SELECT document FROM  comments_document),
                'Files', (SELECT document FROM files_document),
                'PollQuestions', (SELECT document FROM poll_questions_document)
            ) document
            FROM (
                SELECT
                    tn.url_id,  
                    tn.node_id,
                    n.node_type_id,
                    n.title, 
                    n.created_date_time, 
                    n.changed_date_time, 
                    stn.text, 
                    n.publisher_id, 
                    p.name publisher_name,
                    case 
                        when tn.publication_status_id = 0 then false
                        else true
                    end has_been_published,
                    tn.publication_status_id,
                    pl.date_time_closure,
                    pl.poll_status_id
                FROM node n
                join tenant_node tn on tn.node_id = n.id
                join simple_text_node stn on stn.id = n.id 
                join poll pl on pl.id = n.id 
                JOIN publisher p on p.id = n.publisher_id
                where tn.url_id = @url_id and tn.tenant_id = @tenant_id
            ) n
        ) 
        """;


    const string DISCUSSION_DOCUMENT = """
        discussion_document AS (
            SELECT 
                jsonb_build_object(
                    'UrlId', n.url_id,
                    'NodeId', n.node_id,
                    'NodeTypeId', n.node_type_id,
                    'Title', n.title, 
                    'Text', n.text,
                    'HasBeenPublished', n.has_been_published,
                    'PublicationStatusId', publication_status_id,
                    'Authoring', jsonb_build_object(
                        'Id', n.publisher_id, 
                        'Name', n.publisher_name,
                        'CreatedDateTime', n.created_date_time,
                        'ChangedDateTime', n.changed_date_time
                    ),
                    'BreadCrumElements', (SELECT document FROM discussion_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'SeeAlsoBoxElements', (SELECT document FROM see_also_document),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'Files', (SELECT document FROM files_document)
                ) document
            FROM (
                SELECT
                    tn.url_id,  
                    tn.node_id,
                    n.node_type_id,
                    n.title, 
                    n.created_date_time, 
                    n.changed_date_time, 
                    stn.text, 
                    n.publisher_id, 
                    p.name publisher_name,
                    case 
                        when tn.publication_status_id = 0 then false
                        else true
                    end has_been_published,
                    tn.publication_status_id
                FROM node n
                join simple_text_node stn on stn.id = n.id 
                join tenant_node tn on tn.node_id = n.id
                JOIN publisher p on p.id = n.publisher_id
                where tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) n
        ) 
        """;


    const string PAGE_DOCUMENT = """
        page_document AS (
            SELECT 
                jsonb_build_object(
                    'UrlId', n.url_id,
                    'NodeId', n.node_id,
                    'NodeTypeId', n.node_type_id,
                    'Title', n.title, 
                    'Text', n.text,
                    'HasBeenPublished', n.has_been_published,
                    'PublicationStatusId', publication_status_id,
                    'Authoring', jsonb_build_object(
                        'Id', n.publisher_id, 
                        'Name', n.publisher_name,
                        'CreatedDateTime', n.created_date_time,
                        'ChangedDateTime', n.changed_date_time
                    ),
                    'BreadCrumElements', 
                    (SELECT document FROM page_bread_crum_document),
                    'Tags', 
                    (SELECT document FROM tags_document),
                    'SeeAlsoBoxElements', 
                    (SELECT document FROM see_also_document),
                    'CommentListItems', 
                    (SELECT document FROM  comments_document),
                    'Files', 
                    (SELECT document FROM files_document)
                ) document
            FROM (
                SELECT
                    tn.url_id,  
                    tn.node_id,
                    n.node_type_id,
                    n.title, 
                    n.created_date_time, 
                    n.changed_date_time, 
                    stn.text, 
                    n.publisher_id, 
                    p.name publisher_name,
                    case 
                        when tn.publication_status_id = 0 then false
                        else true
                    end has_been_published,
                    tn.publication_status_id
                FROM node n
                join tenant_node tn on tn.node_id = n.id
                join simple_text_node stn on stn.id = n.id 
                JOIN publisher p on p.id = n.publisher_id
                where tn.tenant_id = @tenant_id and tn.url_id = @url_id
                and tn.publication_status_id in 
                (
                    select 
                    id 
                    from accessible_publication_status 
                    where tenant_id = tn.tenant_id 
                    and (
                        subgroup_id = tn.subgroup_id 
                        or subgroup_id is null and tn.subgroup_id is null
                    )
                )
            ) n
        ) 
        """;

    const string BASIC_NAMEABLE_DOCUMENT = """
        basic_nameable_document AS (
            SELECT 
                jsonb_build_object(
                    'UrlId', n.url_id,
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
                    'BreadCrumElements', (SELECT document FROM topics_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'SubTopics', (SELECT document from subtopics_document),
                    'SuperTopics', (SELECT document from supertopics_document),
                    'Files', (SELECT document FROM files_document),
                    'Documents', (SELECT document FROM documents_document),
                    'Cases', (SELECT document FROM nameable_cases_document)
                ) document
            FROM (
                SELECT
                    tn.url_id,  
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
                    tn.publication_status_id
                FROM node n
                join tenant_node tn on tn.node_id = n.id
                join nameable nm on nm.id = n.id 
                JOIN publisher p on p.id = n.publisher_id
                where tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) n
        ) 
        """;

    const string ABUSE_CASE_DOCUMENT = """
        abuse_case_document AS (
            SELECT 
                jsonb_build_object(
                    'UrlId', n.url_id,
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
                    'Date', n.fuzzy_date,
                    'ChildPlacementType', n.child_placement_type,
                    'FamilySize', n.family_size,
                    'HomeSchoolingInvolved', n.home_schooling_involved,
                    'FundamentalFaithInvolved', n.fundamental_faith_involved,
                    'DisabilitiesInvolved', n.disabilities_involved,
                    'BreadCrumElements', (SELECT document FROM abuse_case_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document_abuse_case),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'Documents', (SELECT document FROM documents_document),
                    'Locations', (SELECT document FROM locations_document),
                    'SubTopics', (SELECT document from subtopics_document),
                    'SuperTopics', (SELECT document from supertopics_document),
                    'CaseParties', (SELECT document from case_case_parties_document),
                    'Files', (SELECT document FROM files_document),
                    'TypesOfAbuse', 
                    (
                        select jsonb_agg(
                            jsonb_build_object(
                                'Path', 
                                case 
                                    when tn.url_path is null then '/node/' || tn.url_id
                                    else tn.url_path
                                end,
                                'Title', 
                                t.name
                            )
                        ) 
                        from abuse_case_type_of_abuse at
                        join type_of_abuse toa on toa.id = at.type_of_abuse_id
                        join term t on t.nameable_id = toa.id
                        join vocabulary v on v.id = t.vocabulary_id
                        join tenant_node tn on tn.node_id = toa.id and tn.tenant_id = @tenant_id
                        where at.abuse_case_id = n.node_id
                        and v.name = 'Type of Abuse'
                    ),
                    'TypesOfAbuser', 
                    (
                        select jsonb_agg(
                            jsonb_build_object(
                                'Path', 
                                case 
                                    when tn.url_path is null then '/node/' || tn.url_id
                                    else tn.url_path
                                end,
                                'Title', 
                                t.name
                            )
                        ) 
                        from abuse_case_type_of_abuser at
                        join type_of_abuser toa on toa.id = at.type_of_abuser_id
                        join term t on t.nameable_id = toa.id
                        join vocabulary v on v.id = t.vocabulary_id
                        join tenant_node tn on tn.node_id = toa.id and tn.tenant_id = @tenant_id
                        where at.abuse_case_id = n.node_id
                        and v.name = 'Type of Abuser'
                    )
        
                ) document
            FROM (
                SELECT
                    tn.url_id,  
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
                    c.fuzzy_date,
                    case 
                        when tn2.node_id is null then null
                        else jsonb_build_object(
                            'Title', tn2.title,
                            'Path', tn2.path
                        )
                    end child_placement_type,
                    case 
                        when tn4.node_id is null then null
                        else jsonb_build_object(
                            'Title', tn4.title,
                            'Path', tn4.path
                        )
                    end family_size,
                    ac.home_schooling_involved,
                    ac.fundamental_faith_involved,
                    ac.disabilities_involved
                FROM node n
                join tenant_node tn on tn.node_id = n.id
                join "case" c on c.id = n.id 
                join abuse_case ac on ac.id = n.id 
                join nameable nm on nm.id = n.id 
                JOIN publisher p on p.id = n.publisher_id
                LEFT JOIN (
                    select 
                    n2.id node_id,
                    t.name title,
                    case
                        when tn2.url_path is null then '/node/' || tn2.url_id
                        else '/' || tn2.url_path
                    end path
                    from nameable n2
                    join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = @tenant_id
                    join term t on t.nameable_id = n2.id
                    join tenant_node tn3 on tn3.node_id = t.vocabulary_id and tn3.tenant_id = 1
                    where tn3.url_id = 115
                ) tn2 on tn2.node_id = ac.child_placement_type_id
                LEFT JOIN (
                    select 
                    n2.id node_id,
                    t.name title,
                    case
                        when tn2.url_path is null then '/node/' || tn2.url_id
                        else '/' || tn2.url_path
                    end path
                    from nameable n2
                    join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = @tenant_id
                    join term t on t.nameable_id = n2.id
                    join tenant_node tn3 on tn3.node_id = t.vocabulary_id and tn3.tenant_id = 1
                    where tn3.url_id = 117
                ) tn4 on tn4.node_id = ac.family_size_id            
                WHERE tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) n
        ) 
        """;

    const string CHILD_TRAFFICKING_CASE_DOCUMENT = """
        child_trafficking_case_document AS (
            SELECT 
                jsonb_build_object(
                    'UrlId', n.url_id,
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
                    'Date', n.fuzzy_date,
                    'CountryFrom', n.country_from,
                    'NumberOfChildrenInvolved', n.number_of_children_involved,
                    'BreadCrumElements', (SELECT document FROM child_trafficking_case_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'Documents', (SELECT document FROM documents_document),
                    'Locations', (SELECT document FROM locations_document),
                    'SubTopics', (SELECT document from subtopics_document),
                    'SuperTopics', (SELECT document from supertopics_document),
                    'CaseParties', (SELECT document from case_case_parties_document),
                    'Files', (SELECT document FROM files_document)
                ) document
            FROM (
                SELECT
                    tn.url_id,  
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
                    c.fuzzy_date,
                    case 
                        when tn2.node_id is null then null
                        else jsonb_build_object(
                            'Title', tn2.title,
                            'Path', tn2.path
                        )
                    end country_from,
                    ac.number_of_children_involved
                FROM node n
                join tenant_node tn on tn.node_id = n.id
                join "case" c on c.id = n.id 
                join child_trafficking_case ac on ac.id = n.id 
                join nameable nm on nm.id = n.id 
                JOIN publisher p on p.id = n.publisher_id
                LEFT JOIN (
                    select 
                    n2.id node_id,
                    n2.title,
                    case
                        when tn2.url_path is null then '/node/' || tn2.url_id
                        else '/' || tn2.url_path
                    end path
                    from node n2
                    join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = @tenant_id
                ) tn2 on tn2.node_id = ac.country_id_from
                WHERE tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) n
        ) 
        """;

    const string COERCED_ADOPTION_CASE_DOCUMENT = """
        coerced_adoption_case_document AS (
            SELECT 
                jsonb_build_object(
                    'UrlId', n.url_id,
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
                    'Date', n.fuzzy_date,
                    'BreadCrumElements', (SELECT document FROM coerced_adoption_case_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'Documents', (SELECT document FROM documents_document),
                    'Locations', (SELECT document FROM locations_document),
                    'SubTopics', (SELECT document from subtopics_document),
                    'SuperTopics', (SELECT document from supertopics_document),
                    'CaseParties', (SELECT document from case_case_parties_document),
                    'Files', (SELECT document FROM files_document)
                ) document
            FROM (
                SELECT
                    tn.url_id,  
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
                    c.fuzzy_date
                FROM node n
                join tenant_node tn on tn.node_id = n.id
                join "case" c on c.id = n.id 
                join coerced_adoption_case ac on ac.id = n.id 
                join nameable nm on nm.id = n.id 
                JOIN publisher p on p.id = n.publisher_id
                WHERE tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) n
        ) 
        """;

    const string DEPORTATION_CASE_DOCUMENT = """
        deportation_case_document AS (
            SELECT 
                jsonb_build_object(
                    'UrlId', n.url_id,
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
                    'Date', n.fuzzy_date,
                    'SubdivisionFrom', n.subdivision_from,
                    'CountryTo', n.country_to,  
                    'BreadCrumElements', (SELECT document FROM deportation_case_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'Documents', (SELECT document FROM documents_document),
                    'Locations', (SELECT document FROM locations_document),
                    'SubTopics', (SELECT document from subtopics_document),
                    'SuperTopics', (SELECT document from supertopics_document),
                    'CaseParties', (SELECT document from case_case_parties_document),
                    'Files', (SELECT document FROM files_document)
                ) document
            FROM (
                SELECT
                    tn.url_id,  
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
                    c.fuzzy_date,
                    case 
                        when tn4.node_id is null then null
                        else jsonb_build_object(
                            'Title', tn4.title,
                            'Path', tn4.path
                        )
                    end subdivision_from,
                    case 
                        when tn2.node_id is null then null
                        else jsonb_build_object(
                            'Title', tn2.title,
                            'Path', tn2.path
                        )
                    end country_to
                FROM node n
                join tenant_node tn on tn.node_id = n.id
                join "case" c on c.id = n.id 
                join deportation_case ac on ac.id = n.id 
                join nameable nm on nm.id = n.id 
                JOIN publisher p on p.id = n.publisher_id
                LEFT JOIN (
                    select 
                    n2.id node_id,
                    n2.title,
                    case
                        when tn2.url_path is null then '/node/' || tn2.url_id
                        else '/' || tn2.url_path
                    end path
                    from node n2
                    join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = @tenant_id
                ) tn2 on tn2.node_id = ac.country_id_to
                LEFT JOIN (
                    select 
                    n2.id node_id,
                    s.name title,
                    case
                        when tn2.url_path is null then '/node/' || tn2.url_id
                        else '/' || tn2.url_path
                    end path
                    from node n2
                    join subdivision s on s.id = n2.id
                    join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = @tenant_id
                ) tn4 on tn4.node_id = ac.subdivision_id_from
                WHERE tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) n
        ) 
        """;

    const string DISRUPTED_PLACEMENT_CASE_DOCUMENT = """
        disrupted_placement_case_document AS (
            SELECT 
                jsonb_build_object(
                    'UrlId', n.url_id,
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
                    'Date', n.fuzzy_date,
                    'BreadCrumElements', (SELECT document FROM disrupted_placement_case_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'Documents', (SELECT document FROM documents_document),
                    'Locations', (SELECT document FROM locations_document),
                    'SubTopics', (SELECT document from subtopics_document),
                    'SuperTopics', (SELECT document from supertopics_document),
                    'CaseParties', (SELECT document from case_case_parties_document),
                    'Files', (SELECT document FROM files_document)
                ) document
            FROM (
                SELECT
                    tn.url_id,  
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
                    c.fuzzy_date
                FROM node n
                join tenant_node tn on tn.node_id = n.id
                join "case" c on c.id = n.id 
                join disrupted_placement_case ac on ac.id = n.id 
                join nameable nm on nm.id = n.id 
                JOIN publisher p on p.id = n.publisher_id
                WHERE tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) n
        ) 
        """;

    const string FATHERS_RIGHTS_VIOLATION_CASE_DOCUMENT = """
        fathers_rights_violation_case_document AS (
            SELECT 
                jsonb_build_object(
                    'UrlId', n.url_id,
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
                    'Date', n.fuzzy_date,
                    'BreadCrumElements', (SELECT document FROM fathers_rights_violation_case_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'Documents', (SELECT document FROM documents_document),
                    'Locations', (SELECT document FROM locations_document),
                    'SubTopics', (SELECT document from subtopics_document),
                    'SuperTopics', (SELECT document from supertopics_document),
                    'CaseParties', (SELECT document from case_case_parties_document),
                    'Files', (SELECT document FROM files_document)
                ) document
            FROM (
                SELECT
                    tn.url_id,  
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
                    c.fuzzy_date
                FROM node n
                join tenant_node tn on tn.node_id = n.id
                join "case" c on c.id = n.id 
                join fathers_rights_violation_case ac on ac.id = n.id 
                join nameable nm on nm.id = n.id 
                JOIN publisher p on p.id = n.publisher_id
                WHERE tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) n
        ) 
        """;
    const string WRONGFUL_MEDICATION_CASE_DOCUMENT = """
        wrongful_medication_case_document AS (
            SELECT 
                jsonb_build_object(
                    'UrlId', n.url_id,
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
                    'Date', n.fuzzy_date,
                    'BreadCrumElements', (SELECT document FROM wrongful_medication_case_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'Documents', (SELECT document FROM documents_document),
                    'Locations', (SELECT document FROM locations_document),
                    'SubTopics', (SELECT document from subtopics_document),
                    'SuperTopics', (SELECT document from supertopics_document),
                    'CaseParties', (SELECT document from case_case_parties_document),
                    'Files', (SELECT document FROM files_document)
                ) document
            FROM (
                SELECT
                    tn.url_id,  
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
                    c.fuzzy_date
                FROM node n
                join tenant_node tn on tn.node_id = n.id
                join "case" c on c.id = n.id 
                join wrongful_medication_case ac on ac.id = n.id 
                join nameable nm on nm.id = n.id 
                JOIN publisher p on p.id = n.publisher_id
                WHERE tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) n
        ) 
        """;
    const string WRONGFUL_REMOVAL_CASE_DOCUMENT = """
        wrongful_removal_case_document AS (
            SELECT 
                jsonb_build_object(
                    'UrlId', n.url_id,
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
                    'Date', n.fuzzy_date,
                    'BreadCrumElements', (SELECT document FROM wrongful_removal_case_bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'Documents', (SELECT document FROM documents_document),
                    'Locations', (SELECT document FROM locations_document),
                    'SubTopics', (SELECT document from subtopics_document),
                    'SuperTopics', (SELECT document from supertopics_document),
                    'CaseParties', (SELECT document from case_case_parties_document),
                    'Files', (SELECT document FROM files_document)
                ) document
            FROM (
                SELECT
                    tn.url_id, 
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
                    c.fuzzy_date
                FROM node n
                join tenant_node tn on tn.node_id = n.id
                join "case" c on c.id = n.id 
                join wrongful_removal_case ac on ac.id = n.id 
                join nameable nm on nm.id = n.id 
                JOIN publisher p on p.id = n.publisher_id
                WHERE tn.tenant_id = @tenant_id and tn.url_id = @url_id
            ) n
        ) 
        """;

    const string NODE_DOCUMENT = """
        node_document AS (
            SELECT
                n.node_type_id,
                case
                    when n.node_type_id = 1 then (select document from basic_nameable_document)
                    when n.node_type_id = 2 then (select document from basic_nameable_document)
                    when n.node_type_id = 3 then (select document from basic_nameable_document)
                    when n.node_type_id = 4 then (select document from basic_nameable_document)
                    when n.node_type_id = 5 then (select document from basic_nameable_document)
                    when n.node_type_id = 6 then (select document from basic_nameable_document)
                    when n.node_type_id = 7 then (select document from basic_nameable_document)
                    when n.node_type_id = 8 then (select document from basic_nameable_document)
                    when n.node_type_id = 9 then (select document from basic_nameable_document)
                    when n.node_type_id = 10 then (select document from document_document)
                    when n.node_type_id = 11 then (select document from global_region_document)
                    when n.node_type_id = 12 then (select document from global_region_document)
                    when n.node_type_id = 13 then (select document from basic_country_document)
                    when n.node_type_id = 14 then (select document from bound_country_document)
                    when n.node_type_id = 15 then (select document from country_and_subdivision_document)
                    when n.node_type_id = 16 then (select document from country_and_subdivision_document)
                    when n.node_type_id = 17 then (select document from formal_subdivision_document)
                    when n.node_type_id = 18 then (select document from informal_subdivision_document)
                    when n.node_type_id = 19 then (select document from formal_subdivision_document)
                    when n.node_type_id = 20 then (select document from binding_country_document)
                    when n.node_type_id = 21 then (select document from country_and_subdivision_document)
                    when n.node_type_id = 22 then (select document from formal_subdivision_document)
                    when n.node_type_id = 23 then (select document from organization_document)
                    when n.node_type_id = 24 then (select document from person_document)
                    when n.node_type_id = 26 then (select document from abuse_case_document)
                    when n.node_type_id = 27 then (select document from basic_nameable_document)
                    when n.node_type_id = 28 then (select document from basic_nameable_document)
                    when n.node_type_id = 29 then (select document from child_trafficking_case_document)
                    when n.node_type_id = 30 then (select document from coerced_adoption_case_document)
                    when n.node_type_id = 31 then (select document from deportation_case_document)
                    when n.node_type_id = 32 then (select document from fathers_rights_violation_case_document)
                    when n.node_type_id = 33 then (select document from wrongful_medication_case_document)
                    when n.node_type_id = 34 then (select document from wrongful_removal_case_document)
                    when n.node_type_id = 35 then (select document from blog_post_document)
                    when n.node_type_id = 37 then (select document from discussion_document)
                    when n.node_type_id = 39 then (select document from basic_nameable_document)
                    when n.node_type_id = 40 then (select document from basic_nameable_document)
                    when n.node_type_id = 41 then (select document from basic_nameable_document)
                    when n.node_type_id = 42 then (select document from page_document)
                    when n.node_type_id = 44 then (select document from disrupted_placement_case_document)
                    when n.node_type_id = 50 then (select document from basic_nameable_document)
                    when n.node_type_id = 51 then (select document from basic_nameable_document)
                    when n.node_type_id = 52 then (select document from basic_nameable_document)
                    when n.node_type_id = 53 then (select document from single_question_poll_document)
                    when n.node_type_id = 54 then (select document from multi_question_poll_document)
                    when n.node_type_id = 58 then (select document from basic_nameable_document)
                end document
            FROM node n 
            join tenant_node tn on tn.node_id = n.id
            WHERE tn.url_id = @url_id and tn.tenant_id = @tenant_id
            and tn.publication_status_id in 
            (
                select 
                id 
                from accessible_publication_status 
                where tenant_id = tn.tenant_id 
                and (
                    subgroup_id = tn.subgroup_id 
                    or subgroup_id is null and tn.subgroup_id is null
                )
            )
        ) 
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(UrlIdParameter, request.UrlId),
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(UserIdParameter, request.UserId),
        };
    }

    protected override Node? Read(NpgsqlDataReader reader)
    {
        var node_type_id = reader.GetInt32(0);
        return node_type_id switch {
            1 => reader.GetFieldValue<OrganizationType>(1),
            2 => reader.GetFieldValue<InterOrganizationalRelationType>(1),
            3 => reader.GetFieldValue<PartyPoliticalEntityRelationType>(1),
            4 => reader.GetFieldValue<PersonOrganizationRelationType>(1),
            5 => reader.GetFieldValue<InterPersonalRelationType>(1),
            6 => reader.GetFieldValue<Profession>(1),
            7 => reader.GetFieldValue<Denomination>(1),
            8 => reader.GetFieldValue<HagueStatus>(1),
            9 => reader.GetFieldValue<DocumentType>(1),
            10 => reader.GetFieldValue<Document>(1),
            11 => reader.GetFieldValue<GlobalRegion>(1),
            12 => reader.GetFieldValue<GlobalRegion>(1),
            13 => reader.GetFieldValue<BasicCountry>(1),
            14 => reader.GetFieldValue<BoundCountry>(1),
            15 => reader.GetFieldValue<CountryAndSubdivision>(1),
            16 => reader.GetFieldValue<CountryAndSubdivision>(1),
            17 => reader.GetFieldValue<FormalSubdivision>(1),
            18 => reader.GetFieldValue<InformalSubdivision>(1),
            19 => reader.GetFieldValue<FormalSubdivision>(1),
            20 => reader.GetFieldValue<BindingCountry>(1),
            21 => reader.GetFieldValue<CountryAndSubdivision>(1),
            22 => reader.GetFieldValue<FormalSubdivision>(1),
            23 => reader.GetFieldValue<Organization>(1),
            24 => reader.GetFieldValue<Person>(1),
            26 => reader.GetFieldValue<AbuseCase>(1),
            27 => reader.GetFieldValue<ChildPlacementType>(1),
            28 => reader.GetFieldValue<BasicNameable>(1),
            29 => reader.GetFieldValue<ChildTraffickingCase>(1),
            30 => reader.GetFieldValue<CoercedAdoptionCase>(1),
            31 => reader.GetFieldValue<DeportationCase>(1),
            32 => reader.GetFieldValue<FathersRightsViolationCase>(1),
            33 => reader.GetFieldValue<WrongfulMedicationCase>(1),
            34 => reader.GetFieldValue<WrongfulRemovalCase>(1),
            35 => reader.GetFieldValue<BlogPost>(1),
            37 => reader.GetFieldValue<Discussion>(1),
            39 => reader.GetFieldValue<TypeOfAbuse>(1),
            40 => reader.GetFieldValue<TypeOfAbuser>(1),
            41 => reader.GetFieldValue<BasicNameable>(1),
            42 => reader.GetFieldValue<Page>(1),
            44 => reader.GetFieldValue<DisruptedPlacementCase>(1),
            50 => reader.GetFieldValue<BasicNameable>(1),
            51 => reader.GetFieldValue<BasicNameable>(1),
            52 => reader.GetFieldValue<BasicNameable>(1),
            53 => reader.GetFieldValue<SingleQuestionPoll>(1),
            54 => reader.GetFieldValue<MultiQuestionPoll>(1),
            58 => reader.GetFieldValue<BasicNameable>(1),
            _ => null
        };
    }
}
