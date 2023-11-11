namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using Request = NodeDocumentReaderRequest;

internal sealed class UnitedStatesCongresssDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, UnitedStatesCongress>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter NodeIdParameter = new() { Name = "node_id" };

    private static readonly FieldValueReader<UnitedStatesCongress> DocumentReader = new() { Name = "document" };

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
            select jsonb_build_object(
                'NodeId', 
                n.node_id,
                'NodeTypeId', 
                n.node_type_id,
                'Title', 
                n.title, 
                'Description', 
                n.description,
                'HasBeenPublished', 
                n.has_been_published,
                'PublicationStatusId', 
                publication_status_id,
                'Authoring', 
                jsonb_build_object(
                    'Id', n.publisher_id, 
                    'Name', n.publisher_name,
                    'CreatedDateTime', n.created_date_time,
                    'ChangedDateTime', n.changed_date_time
                ),
                'BreadCrumElements', 
                (SELECT document FROM bread_crum_document),
                'Tags', 
                (SELECT document FROM tags_document),
                'CommentListItems', 
                (SELECT document FROM  comments_document),
                'SubTopics', 
                (SELECT document from subtopics_document),
                'SuperTopics', 
                (SELECT document from supertopics_document),
                'Files', 
                (SELECT document FROM files_document),
                'Documents', 
                (SELECT document FROM documents_document),
                'Senate',
        	    ( 
                    select jsonb_build_object(
        			    'ImagePath',
        			    'files/userimages/Image/Seal_of_the_United_States_Senate-250.png',
        			    'Meetings',
        			    (
                            select jsonb_agg(
        				        jsonb_build_object(
        					        'Title',
        					        n.title,
        					        'Path',
        					        '/united_states_senate/' || m.number,
        					        'DateFrom',
        					        lower(m.date_range),
        					        'DateTo',
        					        upper(m.date_range)
        				        )
        			        )
        		            from united_states_congressional_meeting m
        		            join node n on n.id = m.id
                        )
        		    )
                ),
        	    'House',
                ( 
                    select jsonb_build_object(
        			    'ImagePath',
        			    'files/userimages/Image/Seal_of_the_United_States_House_of_Representatives-250.png',
        			    'Meetings',
        			    (
                            select jsonb_agg(
        				        jsonb_build_object(
        					        'Title',
        					        n.title,
        					        'Path',
        					        '/united_states_house_of_representatives/' || m.number,
        					        'DateFrom',
        					        lower(m.date_range),
        					        'DateTo',
        					        upper(m.date_range)
        				        )
        			        )
        		            from united_states_congressional_meeting m
        		            join node n on n.id = m.id
                        )
                    )
        	    )
            )  document
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
                JOIN publisher p on p.id = n.publisher_id
                where tn.tenant_id = @tenant_id 
                and n.id = 100237
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
    protected override UnitedStatesCongress Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
