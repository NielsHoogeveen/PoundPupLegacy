using System.Collections.Immutable;

namespace PoundPupLegacy.CreateModel.Inserters;

public class CommentInserterFactory : DatabaseInserterFactory<Comment>
{
    public override async Task<IDatabaseInserter<Comment>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var columnDefinitions = new ColumnDefinition[] {
            new ColumnDefinition{
                Name = CommentInserter.PUBLISHER_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition{
                Name = CommentInserter.NODE_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition{
                Name = CommentInserter.COMMENT_ID_PARENT,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition{
                Name = CommentInserter.NODE_STATUS_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition{
                Name = CommentInserter.IP_ADDRESS,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new ColumnDefinition{
                Name = CommentInserter.CREATED_DATE_TIME,
                NpgsqlDbType = NpgsqlDbType.Timestamp
            },
            new ColumnDefinition{
                Name = CommentInserter.TITLE,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new ColumnDefinition{
                Name = CommentInserter.TEXT,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
        };

        var commandWithId = await CreateInsertStatementAsync(
            postgresConnection,
            "comment",
            columnDefinitions.ToImmutableList().Prepend(
                new ColumnDefinition {
                    Name = CommentInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                })
        );
        var commandWithoutId = await CreateIdentityInsertStatementAsync(
            postgresConnection,
            "comment",
            columnDefinitions
        );
        return new CommentInserter(commandWithId, commandWithoutId);
    }



}
public class CommentInserter : DatabaseInserter<Comment>
{
    internal const string ID = "id";
    internal const string NODE_ID = "node_id";
    internal const string COMMENT_ID_PARENT = "comment_id_parent";
    internal const string PUBLISHER_ID = "publisher_id";
    internal const string NODE_STATUS_ID = "node_status_id";
    internal const string IP_ADDRESS = "ip_address";
    internal const string CREATED_DATE_TIME = "created_date_time";
    internal const string TITLE = "title";
    internal const string TEXT = "text";



    private NpgsqlCommand _identityCommand { get; }
    internal CommentInserter(NpgsqlCommand command, NpgsqlCommand identityCommand) : base(command)
    {
        _identityCommand = identityCommand;
    }

    public override async Task InsertAsync(Comment node)
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
