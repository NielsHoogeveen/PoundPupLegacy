namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.ViewModel.Models;
using Request = NodeDocumentReaderRequest;

internal sealed class AbuseCaseDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, AbuseCase>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter NodeIdParameter = new() { Name = "node_id" };

    public override string Sql => SQL;
    const string SQL = $"""
            {SharedViewerSql.CASE_VIEWER},
            {TAGS_DOCUMENT_ABUSE_CASE},
            {BREADCRUM_DOCUMENT},
            {DOCUMENT}
            SELECT document from abuse_case_document
            """
            ;

    const string DOCUMENT = """
        abuse_case_document AS (
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
                    'ChildPlacementType', n.child_placement_type,
                    'FamilySize', n.family_size,
                    'HomeSchoolingInvolved', n.home_schooling_involved,
                    'FundamentalFaithInvolved', n.fundamental_faith_involved,
                    'DisabilitiesInvolved', n.disabilities_involved,
                    'BreadCrumElements', (SELECT document FROM bread_crum_document),
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
                                '/' || nt.viewer_path || '/' || tn.node_id,
                                'Title', 
                                t.name
                            )
                        ) 
                        from abuse_case_type_of_abuse at
                        join type_of_abuse toa on toa.id = at.type_of_abuse_id
                        join term t on t.nameable_id = toa.id
                        join vocabulary v on v.id = t.vocabulary_id
                        join tenant_node tn on tn.node_id = toa.id and tn.tenant_id = @tenant_id
                        join node n on n.id = tn.node_id
                        join node_type nt on nt.id = n.node_type_id
                        where at.abuse_case_id = tn.node_id
                        and v.name = 'Type of Abuse'
                    ),
                    'TypesOfAbuser', 
                    (
                        select jsonb_agg(
                            jsonb_build_object(
                                'Path', 
                                '/' || nt.viewer_path || '/' || tn.node_id,
                                'Title', 
                                t.name
                            )
                        ) 
                        from abuse_case_type_of_abuser at
                        join type_of_abuser toa on toa.id = at.type_of_abuser_id
                        join term t on t.nameable_id = toa.id
                        join vocabulary v on v.id = t.vocabulary_id
                        join tenant_node tn on tn.node_id = toa.id and tn.tenant_id = @tenant_id
                        join node n on n.id = tn.node_id
                        join node_type nt on nt.id = n.node_type_id
                        where at.abuse_case_id = tn.node_id
                        and v.name = 'Type of Abuser'
                    )
        
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
                    '/' || nt2.viewer_path || '/' || tn2.node_id path
                    from nameable nm2
                    join node n2 on n2.id = nm2.id
                    join node_type nt2 on nt2.id = n2.node_type_id
                    join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = @tenant_id
                    join term t on t.nameable_id = n2.id
                    where t.vocabulary_id = 100014
                ) tn2 on tn2.node_id = ac.child_placement_type_id
                LEFT JOIN (
                    select 
                    n2.id node_id,
                    t.name title,
                    '/' || nt2.viewer_path || '/' || tn2.node_id path
                    from nameable nm2
                    join node n2 on n2.id = nm2.id
                    join node_type nt2 on nt2.id = n2.node_type_id
                    join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = @tenant_id
                    join term t on t.nameable_id = n2.id
                    where t.vocabulary_id = 100017
                ) tn4 on tn4.node_id = ac.family_size_id            
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
                    '/abuse_cases', 
                    'Abuse cases', 
                    2
                ) bce
                ORDER BY bce."order"
            ) bces
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
                    '/' || nt2.viewer_path || '/' || tn2.node_id path,
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
                left join nameable_type nmt on nmt.id = n.node_type_id and nmt.tag_label_name is not null
                JOIN tenant_node tn2 on tn2.node_id = t.nameable_id and tn2.tenant_id = @tenant_id and tn2.publication_status_id = 1
                join node_type nt2 on nt2.id = n.node_type_id
                WHERE tn.node_id = @node_id and tn.tenant_id = @tenant_id and tn.publication_status_id = 1
            ) t
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

    protected override AbuseCase? Read(NpgsqlDataReader reader)
    {
        return reader.GetFieldValue<AbuseCase>(0);
    }
}
