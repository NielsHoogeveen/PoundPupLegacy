namespace PoundPupLegacy.Db.Writers;
public class NodeWriter : DatabaseWriter<Node>, IDatabaseWriter<Node>
{
    private const string PUBLISHER_ID = "publisher_id";
    private const string CREATED_DATE_TIME = "created_date_time";
    private const string CHANGED_DATE_TIME = "changed_date_time";
    private const string TITLE = "title";
    private const string NODE_TYPE_ID = "node_type_id";
    private const string OWNER_ID = "owner_id";

    public static async Task<DatabaseWriter<Node>> CreateAsync(NpgsqlConnection connection)
    {
        var columnDefinitions = new ColumnDefinition[] {
            new ColumnDefinition
            {
                Name = PUBLISHER_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition{
                Name = CREATED_DATE_TIME,
                NpgsqlDbType = NpgsqlDbType.Timestamp
            },
            new ColumnDefinition{
                Name = CHANGED_DATE_TIME,
                NpgsqlDbType = NpgsqlDbType.Timestamp
            },
            new ColumnDefinition{
                Name = TITLE,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new ColumnDefinition{
                Name = NODE_TYPE_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition{
                Name = OWNER_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
        };

        var command = await CreateIdentityInsertStatementAsync(
            connection,
            "node",
            columnDefinitions
        );
        return new NodeWriter(command);
    }


    private NodeWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(Node node)
    {
        if (node.Id is not null) {
            throw new Exception("Node id must be null upon creation");
        }
        WriteValue(node.PublisherId, PUBLISHER_ID);
        WriteValue(node.CreatedDateTime, CREATED_DATE_TIME);
        WriteValue(node.ChangedDateTime, CHANGED_DATE_TIME);
        WriteValue(node.Title.Trim(), TITLE);
        WriteValue(node.NodeTypeId, NODE_TYPE_ID);
        WriteNullableValue(node.OwnerId, OWNER_ID);
        node.Id = await _command.ExecuteScalarAsync() switch {
            long i => (int)i,
            _ => throw new Exception("Insert of node does not return an id.")
        };
    }
}
