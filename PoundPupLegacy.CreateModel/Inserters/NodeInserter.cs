namespace PoundPupLegacy.CreateModel.Inserters;
public class NodeInserterFactory : DatabaseInserterFactory<Node>
{
    public override async Task<IDatabaseInserter<Node>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var columnDefinitions = new ColumnDefinition[] {
            new ColumnDefinition
            {
                Name = NodeInserter.PUBLISHER_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition{
                Name = NodeInserter.CREATED_DATE_TIME,
                NpgsqlDbType = NpgsqlDbType.Timestamp
            },
            new ColumnDefinition{
                Name = NodeInserter.CHANGED_DATE_TIME,
                NpgsqlDbType = NpgsqlDbType.Timestamp
            },
            new ColumnDefinition{
                Name = NodeInserter.TITLE,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new ColumnDefinition{
                Name = NodeInserter.NODE_TYPE_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition{
                Name = NodeInserter.OWNER_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
        };

        var command = await CreateIdentityInsertStatementAsync(
            postgresConnection,
            "node",
            columnDefinitions
        );
        return new NodeInserter(command);
    }
}
public class NodeInserter : DatabaseInserter<Node>
{
    internal const string PUBLISHER_ID = "publisher_id";
    internal const string CREATED_DATE_TIME = "created_date_time";
    internal const string CHANGED_DATE_TIME = "changed_date_time";
    internal const string TITLE = "title";
    internal const string NODE_TYPE_ID = "node_type_id";
    internal const string OWNER_ID = "owner_id";

    internal NodeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Node node)
    {
        if (node.Id is not null) {
            throw new Exception("Node id must be null upon creation");
        }
        SetParameter(node.PublisherId, PUBLISHER_ID);
        SetParameter(node.CreatedDateTime, CREATED_DATE_TIME);
        SetParameter(node.ChangedDateTime, CHANGED_DATE_TIME);
        SetParameter(node.Title.Trim(), TITLE);
        SetParameter(node.NodeTypeId, NODE_TYPE_ID);
        SetNullableParameter(node.OwnerId, OWNER_ID);
        node.Id = await _command.ExecuteScalarAsync() switch {
            long i => (int)i,
            _ => throw new Exception("Insert of node does not return an id.")
        };
    }
}
