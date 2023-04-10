namespace PoundPupLegacy.CreateModel.Inserters;

public class CommentInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Comment, CommentInserter>
{
    internal static AutoGenerateIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    internal static NullableIntegerDatabaseParameter CommentIdParent = new() { Name = "comment_id_parent" };
    internal static NonNullableIntegerDatabaseParameter PublisherId = new() { Name = "publisher_id" };
    internal static NonNullableIntegerDatabaseParameter NodeStatusId = new() { Name = "node_status_id" };
    internal static NonNullableStringDatabaseParameter IPAddress = new() { Name = "ip_address" };
    internal static NonNullableDateTimeDatabaseParameter CreatedDateTime = new() { Name = "created_date_time" };
    internal static NonNullableStringDatabaseParameter Title = new() { Name = "title" };
    internal static NonNullableStringDatabaseParameter Text = new() { Name = "text" };

    public override string TableName => "comment";

}
public class CommentInserter : ConditionalAutoGenerateIdDatabaseInserter<Comment>
{

    public CommentInserter(NpgsqlCommand command, NpgsqlCommand autoGenerateIdCommand) : base(command, autoGenerateIdCommand)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(Comment item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(CommentInserterFactory.Id, item.Id),
            ParameterValue.Create(CommentInserterFactory.NodeId, item.NodeId),
            ParameterValue.Create(CommentInserterFactory.CommentIdParent, item.CommentIdParent),
            ParameterValue.Create(CommentInserterFactory.PublisherId, item.PublisherId),
            ParameterValue.Create(CommentInserterFactory.NodeStatusId, item.NodeStatusId),
            ParameterValue.Create(CommentInserterFactory.IPAddress, item.IPAddress),
            ParameterValue.Create(CommentInserterFactory.CreatedDateTime, item.CreatedDateTime),
            ParameterValue.Create(CommentInserterFactory.Title, item.Title),
            ParameterValue.Create(CommentInserterFactory.Text, item.Text)
        };
    }
}
