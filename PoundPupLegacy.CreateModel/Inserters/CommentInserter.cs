using System.Collections.Immutable;

namespace PoundPupLegacy.CreateModel.Inserters;
public class CommentInserter : DatabaseInserter<Comment>, IDatabaseInserter<Comment>
{
    private const string ID = "id";
    private const string NODE_ID = "node_id";
    private const string COMMENT_ID_PARENT = "comment_id_parent";
    private const string PUBLISHER_ID = "publisher_id";
    private const string NODE_STATUS_ID = "node_status_id";
    private const string IP_ADDRESS = "ip_address";
    private const string CREATED_DATE_TIME = "created_date_time";
    private const string TITLE = "title";
    private const string TEXT = "text";



    public static async Task<DatabaseInserter<Comment>> CreateAsync(NpgsqlConnection connection)
    {
        var columnDefinitions = new ColumnDefinition[] {
            new ColumnDefinition{
                Name = PUBLISHER_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition{
                Name = NODE_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition{
                Name = COMMENT_ID_PARENT,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition{
                Name = NODE_STATUS_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition{
                Name = IP_ADDRESS,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new ColumnDefinition{
                Name = CREATED_DATE_TIME,
                NpgsqlDbType = NpgsqlDbType.Timestamp
            },
            new ColumnDefinition{
                Name = TITLE,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new ColumnDefinition{
                Name = TEXT,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
        };

        var commandWithId = await CreateInsertStatementAsync(
            connection,
            "comment",
            columnDefinitions.ToImmutableList().Prepend(
                new ColumnDefinition {
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                })
        );
        var commandWithoutId = await CreateIdentityInsertStatementAsync(
            connection,
            "comment",
            columnDefinitions
        );
        return new CommentInserter(commandWithId, commandWithoutId);
    }


    private NpgsqlCommand _identityCommand { get; }
    private CommentInserter(NpgsqlCommand command, NpgsqlCommand identityCommand) : base(command)
    {
        _identityCommand = identityCommand;
    }

    internal override async Task WriteAsync(Comment node)
    {
        if (node.Id is null) {
            WriteValue(node.NodeId, NODE_ID, _identityCommand);
            WriteNullableValue(node.CommentIdParent, COMMENT_ID_PARENT, _identityCommand);
            WriteValue(node.PublisherId, PUBLISHER_ID, _identityCommand);
            WriteValue(node.NodeStatusId, NODE_STATUS_ID, _identityCommand);
            WriteValue(node.IPAddress, IP_ADDRESS, _identityCommand);
            WriteValue(node.CreatedDateTime, CREATED_DATE_TIME, _identityCommand);
            WriteValue(node.Title, TITLE, _identityCommand);
            WriteValue(node.Text, TEXT, _identityCommand);
            node.Id = await _identityCommand.ExecuteScalarAsync() switch {
                int i => i,
                _ => throw new Exception("Insert of node does not return an id.")
            };
        }
        else {
            WriteValue(node.Id, ID);
            WriteValue(node.NodeId, NODE_ID);
            WriteNullableValue(node.CommentIdParent, COMMENT_ID_PARENT);
            WriteValue(node.PublisherId, PUBLISHER_ID);
            WriteValue(node.NodeStatusId, NODE_STATUS_ID);
            WriteValue(node.IPAddress, IP_ADDRESS);
            WriteValue(node.CreatedDateTime, CREATED_DATE_TIME);
            WriteValue(node.Title, TITLE);
            WriteValue(node.Text, TEXT);
            await _command.ExecuteNonQueryAsync();
        }
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _identityCommand.DisposeAsync();
    }
}
