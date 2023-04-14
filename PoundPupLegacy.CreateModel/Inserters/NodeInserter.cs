namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = NodeInserterFactory;
using Request = Node;
using Inserter = NodeInserter;

public class NodeInserterFactory : AutoGenerateIdDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter PublisherId = new() { Name = "publisher_id" };
    internal static NonNullableDateTimeDatabaseParameter CreatedDateTime = new() { Name = "created_date_time" };
    internal static NonNullableDateTimeDatabaseParameter ChangedDateTime = new() { Name = "changed_date_time" };
    internal static TrimmingNonNullableStringDatabaseParameter Title = new() { Name = "title" };
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };
    internal static NonNullableIntegerDatabaseParameter OwnerId = new() { Name = "owner_id" };

    public override string TableName => "node";

}
public class NodeInserter : AutoGenerateIdDatabaseInserter<Request>
{
    public NodeInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.PublisherId,request.PublisherId),
            ParameterValue.Create(Factory.CreatedDateTime, request.CreatedDateTime),
            ParameterValue.Create(Factory.ChangedDateTime, request.ChangedDateTime),
            ParameterValue.Create(Factory.Title, request.Title),
            ParameterValue.Create(Factory.NodeTypeId, request.NodeTypeId),
            ParameterValue.Create(Factory.OwnerId, request.OwnerId)
        };
    }
}
