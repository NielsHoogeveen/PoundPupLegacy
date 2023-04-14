namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = CommentInserterFactory;
using Request = Comment;
using Inserter = CommentInserter;

public class CommentInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Request, Inserter>
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

}
public class CommentInserter : ConditionalAutoGenerateIdDatabaseInserter<Request>
{

    public CommentInserter(NpgsqlCommand command, NpgsqlCommand autoGenerateIdCommand) : base(command, autoGenerateIdCommand)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.NodeId, item.NodeId),
            ParameterValue.Create(Factory.CommentIdParent, item.CommentIdParent),
            ParameterValue.Create(Factory.PublisherId, item.PublisherId),
            ParameterValue.Create(Factory.NodeStatusId, item.NodeStatusId),
            ParameterValue.Create(Factory.IPAddress, item.IPAddress),
            ParameterValue.Create(Factory.CreatedDateTime, item.CreatedDateTime),
            ParameterValue.Create(Factory.Title, item.Title),
            ParameterValue.Create(Factory.Text, item.Text)
        };
    }
}
