namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.ViewModel.Models;
using Request = NodeDocumentReaderRequest;

internal sealed class UnitedStatesCityDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, UnitedStatesCity>
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
            SELECT document from united_states_city_document
            """
            ;

    const string DOCUMENT = """
        united_states_city_document AS (
            SELECT 
                jsonb_build_object(
                    'NodeId', n.node_id,
                    'NodeTypeId', n.node_type_id,
                    'Title', n.title, 
                    'Description', n.description,
                    'County',
                    jsonb_build_object(
                        'Path', 
                        '/' || united_states_county_viewer_path || '/' || n.united_states_county_id,
                        'Title', 
                        n.united_states_county_title
                    ),
                    'Population', 
                    n.population,
                    'Density',
                    n.density,
                    'Latitude',
                    n.latitude,
                    'Longitude',
                    n.longitude,
                    'Timezone',
                    n.timezone,
                    'Incorporated',
                    n.incorporated,
                    'Military',
                    n.military,
                    'HasBeenPublished', 
                    n.has_been_published,
                    'PublicationStatusId', 
                    publication_status_id,
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
                    tn.publication_status_id,
                    n2.id united_states_county_id,
                    n2.title united_states_county_title,
                    nt2.viewer_path united_states_county_viewer_path,
                    bn.population,
                    bn.density,
                    bn.latitude,
                    bn.longitude,
                    bn.timezone,
                    bn.incorporated,
                    bn.military
                FROM node n
                join tenant_node tn on tn.node_id = n.id
                join nameable nm on nm.id = n.id 
                join united_states_city bn on bn.id = n.id
                join node n2 on n2.id = bn.county_id
                join node_type nt2 on nt2.id = n2.node_type_id
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

    protected override UnitedStatesCity? Read(NpgsqlDataReader reader)
    {
        return reader.GetFieldValue<UnitedStatesCity>(0);
    }
}
