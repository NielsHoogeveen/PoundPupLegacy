namespace PoundPupLegacy.Db.Writers;
public class NodeWriter : DatabaseWriter<Node>, IDatabaseWriter<Node>
{
    private const string ID = "id";
    private const string ACCESS_ROLE_ID = "access_role_id";
    private const string CREATED_DATE_TIME = "created_date_time";
    private const string CHANGED_DATE_TIME = "changed_date_time";
    private const string TITLE = "title";
    private const string NODE_STATUS_ID = "node_status_id";
    private const string NODE_TYPE_ID = "node_type_id";

    public static DatabaseWriter<Node> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
            connection,
            "node",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = ACCESS_ROLE_ID,
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
                    Name = NODE_STATUS_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = NODE_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new NodeWriter(command);
    }


    private NodeWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(Node node)
    {
        WriteValue(node.Id, ID);
        WriteValue(node.AccessRoleId, ACCESS_ROLE_ID);
        WriteValue(node.CreatedDateTime, CREATED_DATE_TIME);
        WriteValue(node.ChangedDateTime, CHANGED_DATE_TIME);
        WriteValue(node.Title, TITLE);
        WriteValue(node.NodeStatusId, NODE_STATUS_ID);
        WriteValue(node.NodeTypeId, NODE_TYPE_ID);
        _command.ExecuteNonQuery();
    }
}
