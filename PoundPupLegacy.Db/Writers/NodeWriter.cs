using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db.Writers;
public class NodeWriter : DatabaseWriter<Node>, IDatabaseWriter<Node>
{
    private const string ID = "id";
    private const string USER_ID = "user_id";
    private const string CREATED = "created";
    private const string CHANGED = "changed";
    private const string TITLE = "title";
    private const string STATUS = "status";
    private const string NODE_TYPE_ID = "node_type_id";
    private const string IS_TERM = "is_term";

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
                    Name = USER_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = CREATED,
                    NpgsqlDbType = NpgsqlDbType.Timestamp
                },
                new ColumnDefinition{
                    Name = CHANGED,
                    NpgsqlDbType = NpgsqlDbType.Timestamp
                },
                new ColumnDefinition{
                    Name = TITLE,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = STATUS,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = NODE_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = IS_TERM,
                    NpgsqlDbType = NpgsqlDbType.Boolean
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
        WriteValue(node.UserId, USER_ID);
        WriteValue(node.Created, CREATED);
        WriteValue(node.Changed, CHANGED);
        WriteValue(node.Title, TITLE);
        WriteValue(node.Status, STATUS);
        WriteValue(node.NodeTypeId, NODE_TYPE_ID);
        WriteValue(node.IsTerm, IS_TERM);
        _command.ExecuteNonQuery();
    }
}
