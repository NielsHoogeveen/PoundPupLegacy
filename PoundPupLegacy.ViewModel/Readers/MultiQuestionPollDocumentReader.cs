namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.ViewModel.Models;
using Request = NodeDocumentReaderRequest;

internal sealed class MultiQuestionPollDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, MultiQuestionPoll>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter NodeIdParameter = new() { Name = "node_id" };

    public override string Sql => SQL;
    const string SQL = $"""
        {SharedViewerSql.POLL_VIEWER},
        {DOCUMENT}
        SELECT document from multi_question_poll_document
        """
        ;

    const string DOCUMENT = """
        multi_question_poll_document AS (
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
                where tn.node_id = @node_id and tn.tenant_id = @tenant_id
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


    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NodeIdParameter, request.NodeId),
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(UserIdParameter, request.UserId),
        };
    }

    protected override MultiQuestionPoll? Read(NpgsqlDataReader reader)
    {
        return reader.GetFieldValue<MultiQuestionPoll>(0);
    }
}
