namespace PoundPupLegacy.ViewModel.Readers;

internal class SharedViewerSql
{

    internal const string NODE_DOCUMENT = $"""
        with
        """;
        //{FILES_DOCUMENT},
        //{TAGS_DOCUMENT}

    internal const string SEE_ALSO_DOCUMENT = """
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
                JOIN node_term nt1 on nt1.node_id = n.id
                JOIN node_term nt2 on nt2.term_id = nt1.term_id and nt2.node_id <> nt1.node_id
                JOIN tenant_node tn on tn.node_id = nt2.node_id and tn.tenant_id = @tenant_id and tn.publication_status_id = 1
                JOIN node n2 on n2.id = tn.node_id
                GROUP BY tn.node_id, tn.url_path, tn.url_id, n2.title
                HAVING COUNT(tn.node_id) > 2 
                ORDER BY count(tn.node_id) desc, n2.title
                LIMIT 10
            ) sa
        )
        """;
    internal const string FILES_DOCUMENT = """
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
    internal const string SUBTOPICS_DOCUMENT = """
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

    internal const string SUPERTOPICS_DOCUMENT = """
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

    internal const string CASE_CASE_PARTIES_DOCUMENT = """
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

    internal const string LOCATIONS_DOCUMENT = """
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

    internal const string TAGS_DOCUMENT = """
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
                ) t
            ) t
        )
        """;

    internal const string DOCUMENTS_DOCUMENT = """
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
                    path,
                    title,
                    publication_date_from,
                    publication_date_to,
                    row_number() over(order by sort_date desc nulls last) sort_order
                from(
                    select
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
                    join tenant_node tn2 on tn2.url_id = @url_id and tn2.tenant_id = @tenant_id and tn2.node_id = t.nameable_id
                    join tenant_node tn on tn.node_id = nt.node_id and tn.tenant_id = @tenant_id
                    join node n2 on n2.Id = tn.node_id
                    join "document" d on d.id = n2.id
                    where tn.publication_status_id in 
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

    internal const string COMMENTS_DOCUMENT = """
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


}
