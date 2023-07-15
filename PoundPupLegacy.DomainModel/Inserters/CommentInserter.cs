using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

using Request = Comment;

public class CommentInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NullableIntegerDatabaseParameter CommentIdParent = new() { Name = "comment_id_parent" };
    private static readonly NonNullableIntegerDatabaseParameter PublisherId = new() { Name = "publisher_id" };
    private static readonly NonNullableIntegerDatabaseParameter NodeStatusId = new() { Name = "node_status_id" };
    private static readonly NonNullableStringDatabaseParameter IPAddress = new() { Name = "ip_address" };
    private static readonly NonNullableDateTimeDatabaseParameter CreatedDateTime = new() { Name = "created_date_time" };
    private static readonly NonNullableStringDatabaseParameter Title = new() { Name = "title" };
    private static readonly NonNullableStringDatabaseParameter Text = new() { Name = "text" };

    public override string TableName => "comment";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NodeId, item.NodeId),
            ParameterValue.Create(CommentIdParent, item.CommentIdParent),
            ParameterValue.Create(PublisherId, item.PublisherId),
            ParameterValue.Create(NodeStatusId, item.NodeStatusId),
            ParameterValue.Create(IPAddress, item.IPAddress),
            ParameterValue.Create(CreatedDateTime, item.CreatedDateTime),
            ParameterValue.Create(Title, item.Title),
            ParameterValue.Create(Text, item.Text)
        };
    }
}
