namespace PoundPupLegacy.CreateModel.Inserters;
public class NodeInserterFactory : AutoGenerateIdDatabaseInserterFactory<Node, NodeInserter>
{
    internal static NonNullableIntegerDatabaseParameter PublisherId = new() { Name = "publisher_id" };
    internal static NonNullableDateTimeDatabaseParameter CreatedDateTime = new() { Name = "created_date_time" };
    internal static NonNullableDateTimeDatabaseParameter ChangedDateTime = new() { Name = "changed_date_time" };
    internal static NonNullableStringDatabaseParameter Title = new() { Name = "title" };
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };
    internal static NonNullableIntegerDatabaseParameter OwnerId = new() { Name = "owner_id" };

    public override string TableName => "node";

}
public class NodeInserter : AutoGenerateIdDatabaseInserter<Node>
{
    public NodeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(Node node)
    {
        if (node.Id is not null) {
            throw new Exception("Node id must be null upon creation");
        }
        return new ParameterValue[] {
            ParameterValue.Create(NodeInserterFactory.PublisherId,node.PublisherId),
            ParameterValue.Create(NodeInserterFactory.CreatedDateTime, node.CreatedDateTime),
            ParameterValue.Create(NodeInserterFactory.ChangedDateTime, node.ChangedDateTime),
            ParameterValue.Create(NodeInserterFactory.Title, node.Title.Trim()),
            ParameterValue.Create(NodeInserterFactory.NodeTypeId, node.NodeTypeId),
            ParameterValue.Create(NodeInserterFactory.OwnerId, node.OwnerId)
        };
    }
}
