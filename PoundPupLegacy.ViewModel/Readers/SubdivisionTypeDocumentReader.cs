namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.ViewModel.Models;
using Request = NodeDocumentReaderRequest;

internal sealed class SubdivisionTypeDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, SubdivisionType>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter NodeIdParameter = new() { Name = "node_id" };

    public override string Sql => SQL;
    const string SQL = $"""
            {SharedViewerSql.DOCUMENTABLE_VIEWER},
            {SharedViewerSql.NAMEABLE_CASES_DOCUMENT},
            {BREADCRUM_DOCUMENT},
            {DOCUMENT}
            SELECT document from basic_nameable_document
            """
            ;

    const string DOCUMENT = """
        basic_nameable_document AS (
            SELECT 
                jsonb_build_object(
                    'NodeId', 
                    n.node_id,
                    'NodeTypeId', 
                    n.node_type_id,
                    'Title', n.title, 
                    'Description', 
                    n.description,
                    'Name', 
                    n.title,
                    'Subdivisions',
                    (
                        select 
                        jsonb_agg(
                            jsonb_build_object(
                                'Id',
                                s.id,
                                'Title',
                                n2.title,
                                'Path',
                                '/' || nt.viewer_path || '/' || s.id,
                                'PublicationStatusId',
                                tn2.publication_status_id,
                                'HasBeenPublished',
                                case 
                                    when tn2.publication_status_id = 0 then false
                                    else true
                                end,
                                'Country',
                                    jsonb_build_object(
        		                    'Title', 
                                    n3.title,
        		                    'Path', 
                                    '/' || nt3.viewer_path || '/' || n3.id
        		                )
                            )
                        )
                        from subdivision s
                        join node n2 on n2.id = s.id
                        join node_type nt on nt.id = n2.node_type_id
                        join tenant_node tn2 on tn2.tenant_id = @tenant_id and tn2.node_id = s.id
                        join node n3 on n3.id = s.country_id
                        join node_type nt3 on nt3.id = n3.node_type_id
                        join tenant_node tn3 on tn3.tenant_id = @tenant_id and tn3.node_id = n3.id
                        where s.subdivision_type_id = @node_id
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
                        and tn3.publication_status_id in 
                        (
                            select 
                            publication_status_id  
                            from user_publication_status 
                            where tenant_id = tn3.tenant_id 
                            and user_id = @user_id
                            and (
                                subgroup_id = tn3.subgroup_id 
                                or subgroup_id is null and tn2.subgroup_id is null
                            )
                        )
                    ),
                    'HasBeenPublished', n.has_been_published,
                    'PublicationStatusId', publication_status_id,
                    'Authoring', jsonb_build_object(
                        'Id', n.publisher_id, 
                        'Name', n.publisher_name,
                        'CreatedDateTime', n.created_date_time,
                        'ChangedDateTime', n.changed_date_time
                    ),
                    'BreadCrumElements', (SELECT document FROM bread_crum_document),
                    'Tags', (SELECT document FROM tags_document),
                    'CommentListItems', (SELECT document FROM  comments_document),
                    'SubTopics', (SELECT document from subtopics_document),
                    'SuperTopics', (SELECT document from supertopics_document),
                    'Files', (SELECT document FROM files_document),
                    'Documents', 
                    (SELECT document FROM documents_document),
                    'Cases', 
                    (SELECT document FROM nameable_cases_document)
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
                    tn.publication_status_id
                FROM node n
                join tenant_node tn on tn.node_id = n.id
                join nameable nm on nm.id = n.id 
                join subdivision_type bn on bn.id = n.id
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
                    '/topics', 
                    'topics', 
                    1
                ) bce
                ORDER BY bce."order"
            ) bces
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

    protected override SubdivisionType? Read(NpgsqlDataReader reader)
    {
        return reader.GetFieldValue<SubdivisionType>(0);
    }
}
