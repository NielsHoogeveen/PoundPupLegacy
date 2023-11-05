namespace PoundPupLegacy.ViewModel.Readers;

internal class SharedViewerSql
{
    public const string NODE_VIEWER = $"""
        WITH 
        {TAGS_DOCUMENT},
        {COMMENTS_DOCUMENT},
        {FILES_DOCUMENT}
        """;

    public const string SIMPLE_TEXT_NODE_WITH_SEE_ALSO_BOX = $"""
        {NODE_VIEWER},
        {SEE_ALSO_DOCUMENT}
        """;


    public const string POLL_VIEWER = $"""
        {NODE_VIEWER},
        {SEE_ALSO_DOCUMENT},
        {POLL_BREADCRUM_DOCUMENT},
        {POLL_OPTIONS_DOCUMENT},
        {POLL_QUESTIONS_DOCUMENT}
        """;

    public const string NAMEABLE_VIEWER = $"""
        {NODE_VIEWER},
        {SUBTOPICS_DOCUMENT},
        {SUPERTOPICS_DOCUMENT}
        """;

    public const string DOCUMENTABLE_VIEWER = $"""
        {NAMEABLE_VIEWER},
        {DOCUMENTS_DOCUMENT}
        """;

    public const string LOCATABLE_VIEWER = $"""
        {DOCUMENTABLE_VIEWER},
        {LOCATIONS_DOCUMENT}
        """;

    public const string CASE_VIEWER = $"""
        {LOCATABLE_VIEWER},
        {CASE_CASE_PARTIES_DOCUMENT}
        """;

    public const string PARTY_VIEWER = $"""
        {LOCATABLE_VIEWER},
        {PARTY_POLITICAL_ENTITY_RELATIONS_DOCUMENT}
        """;

    public const string COUNTRY_VIEWER = $"""
        {LOCATABLE_VIEWER},
        {COUNTRY_BREADCRUM_DOCUMENT},
        {ADOPTION_IMPORTS_DOCUMENT},
        {COUNTRY_SUBDIVISIONS_DOCUMENT},
        {NAMEABLE_CASES_DOCUMENT},
        {ORGANIZATIONS_OF_COUNTRY_DOCUMENT}
        """;

    public const string SUBDIVISIONS_VIEWER = $"""
        {LOCATABLE_VIEWER},
        {SUBDIVISION_BREADCRUM_DOCUMENT},
        {SUBDIVISION_SUBDIVISIONS_DOCUMENT},
        {NAMEABLE_CASES_DOCUMENT},
        {ORGANIZATIONS_OF_SUBDIVISION_DOCUMENT}
        """;

    const string COMMENTS_DOCUMENT = """
        comments_document AS (
            SELECT jsonb_agg(tree) document
            FROM (
                SELECT to_jsonb(sub) AS tree
                FROM (
        	        SELECT 
        		        c.id AS "Id", 
                        c.node_id AS "NodeId",
                        c.comment_id_parent AS "CommentIdParent",
        		        c.node_status_id AS "NodeStatusId",
        		        jsonb_build_object(
        			        'Id', p.id, 
        			        'Name', p.name,
                            'CreatedDateTime', c.created_date_time,
                            'ChangedDateTime', c.created_date_time
                        ) AS "Authoring",
        		        c.title AS "Title", 
        		        c.text AS "Text", 
                        c.comment_id_parent AS "CommentIdParent"
        	        FROM comment c
        	        JOIN publisher p on p.id = c.publisher_id
                    JOIN node n on n.id = c.node_id
                    join tenant_node tn on tn.node_id = n.id
        	        WHERE tn.node_id = @node_id and tn.tenant_id = @tenant_id
                ) sub
        	) agg        
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
            where tn.tenant_id = @tenant_id and tn.node_id = @node_id
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
                        '/' || nt.viewer_path || '/' || tn.node_id path,
                        t.name,
                        case
                            when nmt.tag_label_name is not null then nmt.tag_label_name
                            else nt.name
                        end node_type_name
                    FROM node_term ntm 
                    JOIN tenant_node tn2 on tn2.node_id = ntm.node_id
                    JOIN term t on t.id = ntm.term_id
                    join node n on n.id = t.nameable_id
                    join node_type nt on nt.id = n.node_type_id
                    left join nameable_type nmt on nmt.id = n.node_type_id and nmt.tag_label_name is not null
                    JOIN tenant_node tn on tn.node_id = t.nameable_id and tn.tenant_id = @tenant_id
                    WHERE tn2.node_id = @node_id and tn2.tenant_id = @tenant_id
                    and tn.publication_status_id in 
                    (
                        select 
                        publication_status_id 
                        from user_publication_status
                        where user_id = @user_id
                        and tenant_id = @tenant_id 
                        and (
                            subgroup_id = tn.subgroup_id 
                            or subgroup_id is null and tn.subgroup_id is null
                        )
                    )
                ) t
            ) t
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
        	    where tn.tenant_id = @tenant_id and tn.node_id = @node_id
            ) x
        )
        """;

    const string POLL_QUESTIONS_DOCUMENT = """
        poll_questions_document as(
            select
        	    jsonb_agg(
        		    jsonb_build_object(
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
        		    where tn.tenant_id = @tenant_id and tn.node_id = @node_id
        	    ) x
        	    group by 
        		    node_id,
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
        		    '/' || nt2.viewer_path || '/'  || tn2.node_id party_path,
        		    n3.title political_entity_name,
        		    '/' || nt3.viewer_path || '/' || tn3.node_id political_entity_path,
                    tn5.publication_status_id publication_status_id_5,
        		    n4.title party_political_entity_relation_type_name,
        		    '/' || nt4.viewer_path || '/' || tn4.node_id party_political_entity_relation_type_path,
        		    pper.date_range,
        		    '/' || nt.viewer_path || '/' || tn.node_id path,
        		    n5.title document_proof_name,
                    case 
                        when tn5.node_id is null then null
                        else '/' || nt5.viewer_path || '/' || tn5.node_id 
                    end document_proof_path
        	    from node n
                join node_type nt on nt.id = n.node_type_id
        	    join tenant_node tn on tn.node_id = n.id
        	    join party_political_entity_relation pper on pper.id = n.id 
        	    join node n2 on n2.id = pper.party_id				
                join node_type nt2 on nt2.id = n2.node_type_id
        	    join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = tn.tenant_id

        	    join node n3 on n3.id = pper.political_entity_id
                join node_type nt3 on nt3.id = n3.node_type_id  
        	    join tenant_node tn3 on tn3.node_id = n3.id and tn3.tenant_id = tn.tenant_id

        	    join node n4 on n4.id = pper.party_political_entity_relation_type_id
                join node_type nt4 on nt4.id = n4.node_type_id
        	    join tenant_node tn4 on tn4.node_id = n4.id and tn4.tenant_id = tn.tenant_id

        	    left join node n5 on n5.id = pper.document_id_proof
                left join node_type nt5 on nt5.id = n5.node_type_id
        	    left join tenant_node tn5 on tn4.node_id = n5.id and tn5.tenant_id = tn.tenant_id

        	    where tn.tenant_id = @tenant_id and tn2.node_id = @node_id
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
                and tn3.publication_status_id in 
                (
                    select 
                    publication_status_id 
                    from user_publication_status 
                    where tenant_id = tn3.tenant_id 
                    and user_id = @user_id
                    and (
                        subgroup_id = tn3.subgroup_id 
                        or subgroup_id is null and tn3.subgroup_id is null
                    )
                )
                and tn4.publication_status_id in 
                (
                    select 
                    publication_status_id 
                    from user_publication_status 
                    where tenant_id = tn4.tenant_id 
                    and user_id = @user_id
                    and (
                        subgroup_id = tn4.subgroup_id 
                        or subgroup_id is null and tn4.subgroup_id is null
                    )
                )
        	) x
        )	
        """;


    
    const string SUBTOPICS_DOCUMENT = """
        subtopics_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Title', 
                        "name", 
                        'Path', 
                        path
                    )
                ) "document"
            from(
                select
                    n.title "name",
                    '/' || nt.viewer_path || '/'  || tn.node_id path
                from tenant_node tn
        		join tenant tt on tt.id = tn.tenant_id
                join node n on n.id = tn.node_id
                join node_type nt on nt.id = n.node_type_id
                join system_group sg on sg.id = 0
        		join term t1 on t1.nameable_id =  n.id and t1.vocabulary_id = sg.vocabulary_id_tagging
        		join term_hierarchy th on th.term_id_child = t1.id
        		join term t2 on t2.id = th.term_id_parent
        		join tenant_node tn2 on tn2.tenant_id = tn.tenant_id and tn2.node_id = t2.nameable_id
                WHERE tn.tenant_id = @tenant_id AND tn2.node_id = @node_id
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
            ) an
        )
        """;

    const string SUPERTOPICS_DOCUMENT = """
        supertopics_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
                        'Title', "name", 
                        'Path', path
                    )
                ) "document"
            from(
                select
                    n.title "name",
                    '/' || nt.viewer_path || '/' || tn.node_id path
                from tenant_node tn
        		join tenant tt on tt.id = tn.tenant_id
                join node n on n.id = tn.node_id
                join node_type nt on nt.id = n.node_type_id
                join system_group sg on sg.id = 0
        		join term t1 on t1.nameable_id =  n.id and t1.vocabulary_id = sg.vocabulary_id_tagging
        		join term_hierarchy th on th.term_id_parent = t1.id
        		join term t2 on t2.id = th.term_id_child
        		join tenant_node tn2 on tn2.tenant_id = tn.tenant_id and tn2.node_id = t2.nameable_id
                WHERE tn.tenant_id = @tenant_id AND tn2.node_id = @node_id
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
                                '/' || nt2.viewer_path || '/' || tn2.node_id organization_path
                            from case_parties_organization cpo 
                            join node n2 on n2.id = cpo.organization_id
                            join node_type nt2 on nt2.id = n2.node_type_id
                            join tenant_node tn2 on tn2.node_id = n2.id
                            where tn2.tenant_id = @tenant_id and cpo.case_parties_id = cp.id
                            and tn2.publication_status_id in 
                            (
                                select 
                                publication_status_id 
                                from user_publication_status 
                                where tenant_id = tn2.tenant_id 
                                and user_id = @user_id
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
                                '/' || nt3.viewer_path || '/' || tn3.node_id person_path
                            from case_parties_person cpp 
                            join node n3 on n3.id = cpp.person_id
                            join node_type nt3 on nt3.id = n3.node_type_id
                            join tenant_node tn3 on tn3.node_id = n3.id
                            where tn3.tenant_id = @tenant_id and cpp.case_parties_id = cp.id
                            and tn3.publication_status_id in 
                            (
                                select 
                                publication_status_id 
                                from user_publication_status 
                                where tenant_id = tn3.tenant_id 
                                and user_id = @user_id
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
                join tenant_node tn on tn.node_id = n.id
                where tn.tenant_id = @tenant_id and tn.node_id = @node_id
                and t.vocabulary_id = 100022
            ) x
        )
        """;

    const string LOCATIONS_DOCUMENT = """
        locations_document as(
            select
                jsonb_agg(
                    jsonb_build_object(
        			    'Id', 
                        "id",
        			    'Street', 
                        street,
        			    'Additional', 
                        additional,
        			    'City', 
                        city,
        			    'PostalCode', 
                        postal_code,
        			    'Subdivision', 
                        subdivision,
        			    'Country', 
                        country,
                        'Latitude', 
                        latitude,
                        'Longitude', 
                        longitude
        		    )
                ) document
            from(
                select 
                l.id,
                l.street,
                l.additional,
                l.city,
                l.postal_code,
                jsonb_build_object(
                	'Path', 
                    '/' || nt2.viewer_path || '/' || tn2.node_id,
                	'Title', 
                    s.name
                ) subdivision,
                jsonb_build_object(
                	'Path', 
                    '/' || nt3.viewer_path || '/' || tn3.node_id,
                	'Title', 
                    nc.title
                ) country,
                l.latitude,
                l.longitude
                from "location" l
                join location_locatable ll on ll.location_id = l.id
                join node nc on nc.id = l.country_id
                join node_type nt3 on nt3.id = nc.node_type_id
                join subdivision s on s.id = l.subdivision_id
                join node n2 on n2.id = s.id
                join node_type nt2 on nt2.id = n2.node_type_id
                join tenant_node tn on tn.node_id = ll.locatable_id and tn.tenant_id = @tenant_id and tn.node_id = @node_id
                join tenant_node tn2 on tn2.node_id = s.id and tn2.tenant_id = @tenant_id
                join tenant_node tn3 on tn3.node_id = nc.id and tn3.tenant_id = @tenant_id
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
                    '/' || nt.viewer_path || '/' || tn.node_id path,
                    n2.title
                FROM node n
                JOIN tenant_node tn2 on tn2.node_id = n.id 
                JOIN node_term nt1 on nt1.node_id = n.id
                JOIN node_term nt2 on nt2.term_id = nt1.term_id and nt2.node_id <> nt1.node_id
                JOIN tenant_node tn on tn.node_id = nt2.node_id and tn.tenant_id = @tenant_id and tn.publication_status_id = 1
                JOIN node n2 on n2.id = tn.node_id
                JOIN node_type nt on nt.id = n2.node_type_id
                where tn2.tenant_id = @tenant_id and tn2.node_id = @node_id
                GROUP BY tn.node_id, tn.node_id, n2.title, nt.viewer_path
                HAVING COUNT(tn.node_id) > 2 
                ORDER BY count(tn.node_id) desc, n2.title
                LIMIT 10
            ) sa
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
                            '/' || nt2.viewer_path || '/' || tn2.node_id "path"
                        from node n
                        join tenant_node tn2 on tn2.node_id = n.id and tn2.tenant_id = @tenant_id
                        join organization o on o.id = n.id
                        join organization_organization_type oot on oot.organization_id = o.id
                        join organization_type ot on ot.id = oot.organization_type_id
                        join node n2 on n2.id = ot.id
                        join node_type nt2 on nt2.id = n2.node_type_id
                        join location_locatable ll on ll.locatable_id = n.id
                        join "location" l on l.id = ll.location_id
                        join tenant_node tn on tn.node_id = @node_id and tn.node_id = l.country_id and tn.tenant_id = @tenant_id
                        where tn2.publication_status_id in 
                        (
                            select 
                            publication_status_id 
                            from user_publication_status 
                            where tenant_id = tn2.tenant_id 
                            and user_id = @user_id
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
                        '/' || nt2.viewer_path || '/' || tn2.node_id "path"
                    from node n
                    join tenant_node tn2 on tn2.node_id = n.id and tn2.tenant_id = @tenant_id
                    join organization o on o.id = n.id
                    join organization_organization_type oot on oot.organization_id = o.id
                    join organization_type ot on ot.id = oot.organization_type_id
                    join node n2 on n2.id = ot.id
                    join node_type nt2 on nt2.id = n2.node_type_id
                    join location_locatable ll on ll.locatable_id = n.id
                    join "location" l on l.id = ll.location_id
                    join tenant_node tn on tn.node_id = @node_id and tn.node_id = l.subdivision_id and tn.tenant_id = @tenant_id
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
                        '/' || nt2.viewer_path || '/' || tn2.node_id path,
                        n2.title,
        	            lower(d.published) sort_date,
                        lower(d.published) publication_date_from,
                        upper(d.published) publication_date_to
        	            FROM public.nameable_descendency nd
                        join term t on t.nameable_id = nd.node_id_descendant
        	            join node_term ntm on ntm.term_id = t.id
                        join node n2 on n2.id = ntm.node_id
                        join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = @tenant_id
                        join node_type nt2 on nt2.id = n2.node_type_id
                        join "document" d on d.id = n2.id
                        where nd.node_id = @node_id
                    and tn2.publication_status_id in 
                    (
                        select 
                        publication_status_id 
                        from user_publication_status 
                        where tenant_id = tn2.tenant_id 
                        and user_id = @user_id
                        and (
                            subgroup_id = tn2.subgroup_id 
                            or subgroup_id is null and tn2.subgroup_id is null
                        )
                    )
                ) x
            ) docs
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
        	        jsonb_agg(
                        jsonb_build_object(
        		            'Title', 
                            subdivision_name,
        		            'Path', 
                            '/' || viewer_path || '/' || node_id
        		        )
                    ) "subdivisions"
                FROM(
        	        select
        		        distinct
        		        n.title subdivision_type_name, 
        		        s.name subdivision_name,
        		        tn2.node_id,
                        nt2.viewer_path
        	        from country c
        	        join tenant_node tn on tn.node_id = c.id and tn.tenant_id = @tenant_id and tn.node_id = @node_id
        	        join tenant t on t.id = tn.tenant_id
        	        join subdivision s on s.country_id = c.id
        	        join node n on n.id = s.subdivision_type_id
                    join node n2 on n2.id = s.id
                    join node_type nt2 on nt2.id = n2.node_type_id
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
        	        jsonb_agg(
                        jsonb_build_object(
        		            'Title', 
                            subdivision_name,
        		            'Path', 
                            '/' || viewer_path || '/'  || node_id
        		        )
                    ) "subdivisions"
                FROM(
        	        select
        		        distinct
        		        n.title subdivision_type_name, 
        		        s.name subdivision_name,
        		        tn2.node_id,
                        nt2.viewer_path
        	        from subdivision sd
        	        join tenant_node tn on tn.node_id = sd.id and tn.tenant_id = @tenant_id and tn.node_id = @node_id
        	        join tenant t on t.id = tn.tenant_id
        	        join basic_second_level_subdivision bsls on bsls.intermediate_level_subdivision_id = sd.id
                    join subdivision s on s.id = bsls.id
        	        join node n on n.id = s.subdivision_type_id
                    join node n2 on n2.id = s.id
                    join node_type nt2 on nt2.id = n2.node_type_id
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
                    'Path', path,
                    'Title', "name"
                )
            ) document
            FROM(
            SELECT
        	    path,
        	    "name"
            FROM(
                SELECT 
                    '/' path, 
                    'Home' "name", 
                    0 "order"
                UNION
                SELECT 
                    
                    '/countries', 
                    'countries', 
                    1
                UNION
                SELECT 
                    '/' || nt.viewer_path || '/' || tn.node_id path,
                    n.title, 
                    2
                    from subdivision s
                    join country c on c.id = s.country_id
                    join node n on n.id = c.id
                    join node_type nt on nt.id = n.node_type_id                    
                    join tenant_node tn on tn.node_id = c.id and tn.tenant_id = 1 and tn.node_id = @node_id
        
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
        								        join node n2 on n2.id = 101301
        								        LEFT join inter_country_relation icr on icr.country_id_from = cto.id and cfm.id = icr.country_id_to and icr.date_range = cr.date_range and icr.inter_country_relation_type_id = n2.id
        								        WHERE tn.node_id = @node_id

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

    public const string NAMEABLE_CASES_DOCUMENT = """
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
        			    '/' || viewer_path || '/' || node_id,
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
        				nt2.viewer_path,
        				tn2.node_id, 
        				c.fuzzy_date
        			from node n
        			join
                    (
        				select
                		distinct
                		nameable_id,
                        id,
                        node_type_id,
                        title
                		from(
                			select
                			nd.node_id nameable_id,
                			n2.id,
                			n2.node_type_id,
                			n2.title
                			from nameable_descendency nd
        					join term t on t.nameable_id = nd.node_id_descendant
                			join node_term nt on nt.term_id = t.id
                			join node n2 on n2.id = nt.node_id
                            join tenant_node tn on tn.node_id = nd.node_id_descendant and tn.tenant_id = @tenant_id
                            where tn.publication_status_id in 
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
                	join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = @tenant_id
                    where n.id = @node_id
                    and tn2.publication_status_id in 
                    (
                        select 
                        publication_status_id 
                        from user_publication_status 
                        where tenant_id = tn2.tenant_id 
                        and user_id = @user_id
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
}
