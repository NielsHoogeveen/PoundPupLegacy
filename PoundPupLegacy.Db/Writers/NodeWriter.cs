using System.Collections.Immutable;

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

    public static async Task<DatabaseWriter<Node>> CreateAsync(NpgsqlConnection connection)
    {
        var columnDefinitions = new ColumnDefinition[] {
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
        };

        var commandWithId = await CreateInsertStatementAsync(
            connection,
            "node",
            columnDefinitions.ToImmutableList().Prepend(
                new ColumnDefinition
                {
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                })
        );
        var commandWithoutId = await CreateIdentityInsertStatementAsync(
            connection,
            "node",
            columnDefinitions
        );
        return new NodeWriter(commandWithId, commandWithoutId);
    }


    private NpgsqlCommand _identityCommand { get; }
    private NodeWriter(NpgsqlCommand command, NpgsqlCommand identityCommand) : base(command)
    {
        _identityCommand = identityCommand;
    }

    internal override async Task WriteAsync(Node node)
    {
        if (node.Id is null)
        {
            WriteValue(node.AccessRoleId, ACCESS_ROLE_ID, _identityCommand);
            WriteValue(node.CreatedDateTime, CREATED_DATE_TIME, _identityCommand);
            WriteValue(node.ChangedDateTime, CHANGED_DATE_TIME, _identityCommand);
            WriteValue(node.Title, TITLE, _identityCommand);
            WriteValue(node.NodeStatusId, NODE_STATUS_ID, _identityCommand);
            WriteValue(node.NodeTypeId, NODE_TYPE_ID, _identityCommand);
            node.Id = await _identityCommand.ExecuteScalarAsync() switch
            {
                int i => i,
                _ => throw new Exception("Insert of node does not return an id.")
            };
        }
        else
        {
            WriteValue(node.Id, ID);
            WriteValue(node.AccessRoleId, ACCESS_ROLE_ID);
            WriteValue(node.CreatedDateTime, CREATED_DATE_TIME);
            WriteValue(node.ChangedDateTime, CHANGED_DATE_TIME);
            WriteValue(node.Title, TITLE);
            WriteValue(node.NodeStatusId, NODE_STATUS_ID);
            WriteValue(node.NodeTypeId, NODE_TYPE_ID);
            await _command.ExecuteNonQueryAsync();
        }
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _identityCommand.DisposeAsync();
    }
}
