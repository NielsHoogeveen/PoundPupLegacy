namespace PoundPupLegacy.CreateModel.Inserters;

using Request = Comment;

public class CommentInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    internal static NullableIntegerDatabaseParameter CommentIdParent = new() { Name = "comment_id_parent" };
    internal static NonNullableIntegerDatabaseParameter PublisherId = new() { Name = "publisher_id" };
    internal static NonNullableIntegerDatabaseParameter NodeStatusId = new() { Name = "node_status_id" };
    internal static NonNullableStringDatabaseParameter IPAddress = new() { Name = "ip_address" };
    internal static NonNullableDateTimeDatabaseParameter CreatedDateTime = new() { Name = "created_date_time" };
    internal static NonNullableStringDatabaseParameter Title = new() { Name = "title" };
    internal static NonNullableStringDatabaseParameter Text = new() { Name = "text" };

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
