namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.ViewModel.Models;
using Request = NodeDocumentReaderRequest;

internal sealed class HouseBillDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, HouseBill>
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
                    'NodeId', n.node_id,
                    'NodeTypeId', n.node_type_id,
                    'Title', n.title, 
                    'Description', n.description,
                    'IntroductionDate', n.introduction_date,
                    'HasBeenPublished', n.has_been_published,
                    'PublicationStatusId', publication_status_id,
                    'Authoring', jsonb_build_object(
                        'Id', 
                        n.publisher_id, 
                        'Name', 
                        n.publisher_name,
                        'CreatedDateTime', 
                        n.created_date_time,
                        'ChangedDateTime', 
                        n.changed_date_time
                    ),
                    'Act',
                    case
                        when act_id is null then null
                        else jsonb_build_object(
                            'Path', 
                            '/' || act_viewer_path || '/' || act_id,
                            'Title', 
                            act_name
                        )
                    end,
                    'BillActions', 
                    (
                        SELECT 
        				jsonb_agg(
                            jsonb_build_object(
                                'Representative', jsonb_build_object(
                                    'Path', 
                                    '/' || representative_viewer_path || '/' || representative_id,
                                    'Title', 
                                    representative_name
                                ),
                                'BillActionType', jsonb_build_object(
                                    'Path', 
                                    '/' || bill_action_type_viewer_path || '/' || bill_action_type_id,
                                    'Title', 
                                    bill_action_type_name
                                ),
                                'Date', date
                            )
                        ) "document"
                        FROM (
                            SELECT 
        						
                                p.id representative_id,
                                n.title representative_name,
                                bat.id bill_action_type_id,
                                n2.title bill_action_type_name,
                                hba.date,
                                nt.viewer_path representative_viewer_path,
                                nt2.viewer_path bill_action_type_viewer_path
                            FROM representative_house_bill_action hba
                            JOIN representative s on s.id = hba.representative_id
        					join professional_role pr on pr.id = s.id
        					join person p on p.id = pr.person_id
        					join node n on n.id  = p.id
        					join node_type nt on nt.id = n.node_type_id
                            JOIN bill_action_type bat on bat.id = hba.bill_action_type_id
        					join node n2 on n2.id = bat.id
        					join node_type nt2 on nt2.id = n2.node_type_id
                            where hba.house_bill_id = @node_id
                        ) bill_actions
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
                    b.introduction_date,
                    a.id act_id,
                    a.title act_name,
                    a.viewer_path act_viewer_path
                FROM node n
                join tenant_node tn on tn.node_id = n.id
                join nameable nm on nm.id = n.id 
                join bill b on b.id = n.id
                    left join (
                    select
                    a.id,
                    n2.title,
                    nt2.viewer_path
                    from act a 
                    join node n2 on n2.id = a.id
                    join node_type nt2 on nt2.id = n2.node_type_id
                    join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = @tenant_id
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
                ) a on a.id = b.act_id
                join house_bill hb on hb.id = n.id
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

    protected override HouseBill? Read(NpgsqlDataReader reader)
    {
        return reader.GetFieldValue<HouseBill>(0);
    }
}
