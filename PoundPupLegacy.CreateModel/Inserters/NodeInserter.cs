namespace PoundPupLegacy.CreateModel.Inserters;
public class NodeInserterFactory : DatabaseInserterFactory<Node>
{
    internal static NonNullableIntegerDatabaseParameter PublisherId = new() { Name = "publisher_id" };
    internal static NonNullableDateTimeDatabaseParameter CreatedDateTime = new() { Name = "created_date_time" };
    internal static NonNullableDateTimeDatabaseParameter ChangedDateTime = new() { Name = "changed_date_time" };
    internal static NonNullableStringDatabaseParameter Title = new() { Name = "title" };
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };
    internal static NonNullableIntegerDatabaseParameter OwnerId = new() { Name = "owner_id" };

    public override async Task<IDatabaseInserter<Node>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var databaseParameters = new DatabaseParameter[] {
            PublisherId,
            CreatedDateTime,
            ChangedDateTime,
            Title,
            NodeTypeId,
            OwnerId
        };

        var command = await CreateIdentityInsertStatementAsync(
            postgresConnection,
            "node",
            databaseParameters
        );
        return new NodeInserter(command);
    }
}
public class NodeInserter : DatabaseInserter<Node>
{
    internal NodeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Node node)
    {
        if (node.Id is not null) {
            throw new Exception("Node id must be null upon creation");
        }
        Set(NodeInserterFactory.PublisherId,node.PublisherId);
        Set(NodeInserterFactory.CreatedDateTime, node.CreatedDateTime);
        Set(NodeInserterFactory.ChangedDateTime, node.ChangedDateTime);
        Set(NodeInserterFactory.Title, node.Title.Trim());
        Set(NodeInserterFactory.NodeTypeId, node.NodeTypeId);
        Set(NodeInserterFactory.OwnerId, node.OwnerId);
        node.Id = await _command.ExecuteScalarAsync() switch {
            long i => (int)i,
            _ => throw new Exception("Insert of node does not return an id.")
        };
    }
}
