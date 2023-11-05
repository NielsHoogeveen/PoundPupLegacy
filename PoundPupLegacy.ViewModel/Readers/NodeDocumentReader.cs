namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.ViewModel.Models;
using System.Dynamic;
using Request = NodeDocumentReaderRequest;

internal sealed class NodeDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, Node>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter NodeIdParameter = new() { Name = "node_id" };

    public override string Sql => SQL;
    const string SQL = $"""
            WITH 
            {TAGS_DOCUMENT},
            {COMMENTS_DOCUMENT},
            {FILES_DOCUMENT},
            {DOCUMENT_BREADCRUM_DOCUMENT},
            {DOCUMENT_DOCUMENT},
            {NODE_DOCUMENT}
            SELECT node_type_id, document from node_document
            """
;


    //const string SQL = $"""
    //        WITH 
    //        {TERM_DESCENDANCY},
    //        {SharedSql.ACCESSIBLE_PUBLICATIONS_STATUS},
    //        {NODES_WITH_DESCENDANTS},
    //        {NAMEABLE_CASES_DOCUMENT},
    //        {FILES_DOCUMENT},
    //        {SEE_ALSO_DOCUMENT},
    //        {LOCATIONS_DOCUMENT},
    //        {ORGANIZATION_CASES_DOCUMENT},
    //        {PERSON_CASES_DOCUMENT},
    //        {CASE_CASE_PARTIES_DOCUMENT},
    //        {INTER_ORGANIZATIONAL_RELATION_DOCUMENT},
    //        {INTER_PERSONAL_RELATION_DOCUMENT},
    //        {PARTY_POLITICAL_ENTITY_RELATIONS_DOCUMENT},
    //        {PROFESSIONS_DOCUMENT},
    //        {ORGANIZATION_TYPES_DOCUMENT},
    //        {PERSON_ORGANIZATION_RELATIONS_DOCUMENT},
    //        {ORGANIZATION_PERSON_RELATIONS_DOCUMENT},
    //        {TAGS_DOCUMENT},
    //        {TAGS_DOCUMENT_ABUSE_CASE},
    //        {BILL_ACTIONS_DOCUMENT},
    //        {SUBTOPICS_DOCUMENT},
    //        {SUPERTOPICS_DOCUMENT},
    //        {DOCUMENTS_DOCUMENT},
    //        {COMMENTS_DOCUMENT},
    //        {ORGANIZATIONS_OF_COUNTRY_DOCUMENT},
    //        {ORGANIZATIONS_OF_SUBDIVISION_DOCUMENT},
    //        {COUNTRY_SUBDIVISIONS_DOCUMENT},
    //        {SUBDIVISION_SUBDIVISIONS_DOCUMENT},
    //        {BLOG_POST_BREADCRUM_DOCUMENT},
    //        {DISCUSSION_BREADCRUM_DOCUMENT},
    //        {PAGE_BREADCRUM_DOCUMENT},
    //        {ORGANIZATION_BREADCRUM_DOCUMENT},
    //        {PERSON_BREADCRUM_DOCUMENT},
    //        {DOCUMENT_BREADCRUM_DOCUMENT},
    //        {POLL_BREADCRUM_DOCUMENT},
    //        {POLL_OPTIONS_DOCUMENT},
    //        {POLL_QUESTIONS_DOCUMENT},
    //        {ABUSE_CASE_BREADCRUM_DOCUMENT},
    //        {CHILD_TRAFFICKING_CASE_BREADCRUM_DOCUMENT},
    //        {COERCED_ADOPTION_CASE_BREADCRUM_DOCUMENT},
    //        {DEPORTATION_CASE_BREADCRUM_DOCUMENT},
    //        {DISRUPTED_PLACEMENT_CASE_BREADCRUM_DOCUMENT},
    //        {FATHERS_RIGHTS_VIOLATION_CASE_BREADCRUM_DOCUMENT},
    //        {WRONGFUL_MEDICATION_CASE_BREADCRUM_DOCUMENT},
    //        {WRONGFUL_REMOVAL_CASE_BREADCRUM_DOCUMENT},
    //        {SUBDIVISION_BREADCRUM_DOCUMENT},
    //        {GLOBAL_REGION_BREADCRUM_DOCUMENT},
    //        {COUNTRY_BREADCRUM_DOCUMENT},
    //        {TOPICS_BREADCRUM_DOCUMENT},
    //        {ADOPTION_IMPORTS_DOCUMENT},
    //        {BLOG_POST_DOCUMENT},
    //        {PAGE_DOCUMENT},
    //        {DISCUSSION_DOCUMENT},
    //        {DOCUMENT_DOCUMENT},
    //        {SINGLE_QUESTION_POLL_DOCUMENT},
    //        {MULTIPLE_QUESTION_POLL_DOCUMENT},
    //        {ABUSE_CASE_DOCUMENT},
    //        {CHILD_TRAFFICKING_CASE_DOCUMENT},
    //        {COERCED_ADOPTION_CASE_DOCUMENT},
    //        {DEPORTATION_CASE_DOCUMENT},
    //        {DISRUPTED_PLACEMENT_CASE_DOCUMENT},
    //        {FATHERS_RIGHTS_VIOLATION_CASE_DOCUMENT},
    //        {WRONGFUL_MEDICATION_CASE_DOCUMENT},
    //        {WRONGFUL_REMOVAL_CASE_DOCUMENT},
    //        {BASIC_NAMEABLE_DOCUMENT},
    //        {ORGANIZATION_DOCUMENT},
    //        {PERSON_DOCUMENT},
    //        {INFORMAL_SUBDIVISION_DOCUMENT},
    //        {FORMAL_SUBDIVISION_DOCUMENT},
    //        {BASIC_COUNTRY_DOCUMENT},
    //        {GLOBAL_REGION_DOCUMENT},
    //        {COUNTRY_AND_SUBDIVISION_DOCUMENT},
    //        {BINDING_COUNTRY_DOCUMENT},
    //        {BOUND_COUNTRY_DOCUMENT},
    //        {NODE_DOCUMENT}
    //        SELECT node_type_id, document from node_document
    //        """
    //;

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


    const string DOCUMENT_DOCUMENT = """
        document_document AS (
            SELECT 
                jsonb_build_object(
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
                    'BreadCrumElements', 
                    (SELECT document FROM document_bread_crum_document),
                    'Tags', 
                    (SELECT document FROM tags_document),
                    'CommentListItems', 
                    (SELECT document FROM  comments_document),
                    'Files', 
                    (SELECT document FROM files_document)
            ) document
            FROM(
                SELECT
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
                    '/' || nt.viewer_path || '/' || tn.node_id path
                    from document_type dt
                    join node n on n.id = dt.id
                    join node_type nt on nt.id = n.node_type_id
                    join tenant_node tn on tn.node_id = dt.id and tn.tenant_id = @tenant_id
                ) dt on dt.id = d.document_type_id
                JOIN publisher p on p.id = n.publisher_id
                where tn.tenant_id = @tenant_id and tn.node_id = @node_id
            ) n
        )
        """;


    const string NODE_DOCUMENT = """
        node_document AS (
            SELECT
                n.node_type_id,
                case
                    --when n.node_type_id = 1 then (select document from basic_nameable_document)
                    --when n.node_type_id = 2 then (select document from basic_nameable_document)
                    --when n.node_type_id = 3 then (select document from basic_nameable_document)
                    --when n.node_type_id = 4 then (select document from basic_nameable_document)
                    --when n.node_type_id = 5 then (select document from basic_nameable_document)
                    --when n.node_type_id = 6 then (select document from basic_nameable_document)
                    --when n.node_type_id = 7 then (select document from basic_nameable_document)
                    --when n.node_type_id = 8 then (select document from basic_nameable_document)
                    --when n.node_type_id = 9 then (select document from basic_nameable_document)
                    when n.node_type_id = 10 then (select document from document_document)
                    --when n.node_type_id = 11 then (select document from global_region_document)
                    --when n.node_type_id = 12 then (select document from global_region_document)
                    --when n.node_type_id = 13 then (select document from basic_country_document)
                    --when n.node_type_id = 14 then (select document from bound_country_document)
                    --when n.node_type_id = 15 then (select document from country_and_subdivision_document)
                    --when n.node_type_id = 16 then (select document from country_and_subdivision_document)
                    --when n.node_type_id = 17 then (select document from formal_subdivision_document)
                    --when n.node_type_id = 18 then (select document from informal_subdivision_document)
                    --when n.node_type_id = 19 then (select document from formal_subdivision_document)
                    --when n.node_type_id = 20 then (select document from binding_country_document)
                    --when n.node_type_id = 21 then (select document from country_and_subdivision_document)
                    --when n.node_type_id = 22 then (select document from formal_subdivision_document)
                    --when n.node_type_id = 23 then (select document from organization_document)
                    --when n.node_type_id = 24 then (select document from person_document)
                    --when n.node_type_id = 26 then (select document from abuse_case_document)
                    --when n.node_type_id = 27 then (select document from basic_nameable_document)
                    --when n.node_type_id = 28 then (select document from basic_nameable_document)
                    --when n.node_type_id = 29 then (select document from child_trafficking_case_document)
                    --when n.node_type_id = 30 then (select document from coerced_adoption_case_document)
                    --when n.node_type_id = 31 then (select document from deportation_case_document)
                    --when n.node_type_id = 32 then (select document from fathers_rights_violation_case_document)
                    --when n.node_type_id = 33 then (select document from wrongful_medication_case_document)
                    --when n.node_type_id = 34 then (select document from wrongful_removal_case_document)
                    --when n.node_type_id = 35 then (select document from blog_post_document)
                    --when n.node_type_id = 37 then (select document from discussion_document)
                    --when n.node_type_id = 39 then (select document from basic_nameable_document)
                    --when n.node_type_id = 40 then (select document from basic_nameable_document)
                    --when n.node_type_id = 41 then (select document from basic_nameable_document)
                    --when n.node_type_id = 42 then (select document from page_document)
                    --when n.node_type_id = 44 then (select document from disrupted_placement_case_document)
                    --when n.node_type_id = 50 then (select document from basic_nameable_document)
                    --when n.node_type_id = 51 then (select document from basic_nameable_document)
                    --when n.node_type_id = 52 then (select document from basic_nameable_document)
                    --when n.node_type_id = 53 then (select document from single_question_poll_document)
                    --when n.node_type_id = 54 then (select document from multi_question_poll_document)
                    --when n.node_type_id = 58 then (select document from basic_nameable_document)
                end document
            FROM node n 
            join tenant_node tn on tn.node_id = n.id
            WHERE tn.node_id = @node_id and tn.tenant_id = @tenant_id
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
