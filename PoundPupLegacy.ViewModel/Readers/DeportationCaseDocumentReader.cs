namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.ViewModel.Models;
using Request = NodeDocumentReaderRequest;

internal sealed class DeportationCaseDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, DeportationCase>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter NodeIdParameter = new() { Name = "node_id" };

    public override string Sql => SQL;
    const string SQL = $"""
            {SharedViewerSql.CASE_VIEWER},
            {BREADCRUM_DOCUMENT},
            {DOCUMENT}
            SELECT document from deportation_case_document
            """
            ;

    const string DOCUMENT = """
        deportation_case_document AS (
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
                    'Date', n.fuzzy_date,
                    'SubdivisionFrom', n.subdivision_from,
                    'CountryTo', n.country_to,  
                    'BreadCrumElements', (SELECT document FROM bread_crum_document),
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
                    '/' || nt2.viewer_path || '/' || tn2.node_id path
                    from node n2
                    join node_type nt2 on nt2.id = n2.node_type_id
                    join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = @tenant_id
                ) tn2 on tn2.node_id = ac.country_id_to
                LEFT JOIN (
                    select 
                    n2.id node_id,
                    s.name title,
                    '/' || nt2.viewer_path || '/' || tn2.node_id path
                    from node n2
                    join node_type nt2 on nt2.id = n2.node_type_id
                    join subdivision s on s.id = n2.id
                    join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = @tenant_id
                ) tn4 on tn4.node_id = ac.subdivision_id_from
                WHERE tn.tenant_id = @tenant_id and tn.node_id = @node_id
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

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NodeIdParameter, request.NodeId),
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(UserIdParameter, request.UserId),
        };
    }

    protected override DeportationCase? Read(NpgsqlDataReader reader)
    {
        return reader.GetFieldValue<DeportationCase>(0);
    }
}
