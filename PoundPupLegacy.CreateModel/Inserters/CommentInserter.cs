using System.Collections.Immutable;

namespace PoundPupLegacy.CreateModel.Inserters;

public class CommentInserterFactory : DatabaseInserterFactory<Comment>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    internal static NullableIntegerDatabaseParameter CommentIdParent = new() { Name = "comment_id_parent" };
    internal static NonNullableIntegerDatabaseParameter PublisherId = new() { Name = "publisher_id" };
    internal static NonNullableIntegerDatabaseParameter NodeStatusId = new() { Name = "node_status_id" };
    internal static NonNullableStringDatabaseParameter IPAddress = new() { Name = "ip_address" };
    internal static NonNullableDateTimeDatabaseParameter CreatedDateTime = new() { Name = "created_date_time" };
    internal static NonNullableStringDatabaseParameter Title = new() { Name = "title" };
    internal static NonNullableStringDatabaseParameter Text = new() { Name = "text" };

    public override async Task<IDatabaseInserter<Comment>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var parameterDefinitions = new DatabaseParameter[] {
            NodeId,
            CommentIdParent,
            PublisherId,
            NodeStatusId,
            IPAddress,
            CreatedDateTime,
            Title,
            Text
        };

        var commandWithId = await CreateInsertStatementAsync(
            postgresConnection,
            "comment",
            parameterDefinitions.ToImmutableList().Prepend(Id)
        );
        var commandWithoutId = await CreateAutoGenerateIdentityInsertStatementAsync(
            postgresConnection,
            "comment",
            parameterDefinitions
        );
        return new CommentInserter(commandWithId, commandWithoutId);
    }



}
public class CommentInserter : DatabaseInserter<Comment>
{

    private NpgsqlCommand _identityCommand { get; }
    internal CommentInserter(NpgsqlCommand command, NpgsqlCommand identityCommand) : base(command)
    {
        _identityCommand = identityCommand;
    }

    public override async Task InsertAsync(Comment node)
    {
        if (node.Id is null) {
            Set(CommentInserterFactory.NodeId, node.NodeId, _identityCommand);
            Set(CommentInserterFactory.CommentIdParent, node.CommentIdParent, _identityCommand);
            Set(CommentInserterFactory.PublisherId, node.PublisherId, _identityCommand);
            Set(CommentInserterFactory.NodeStatusId, node.NodeStatusId, _identityCommand);
            Set(CommentInserterFactory.IPAddress, node.IPAddress, _identityCommand);
            Set(CommentInserterFactory.CreatedDateTime, node.CreatedDateTime, _identityCommand);
            Set(CommentInserterFactory.Title, node.Title, _identityCommand);
            Set(CommentInserterFactory.Text, node.Text, _identityCommand);
            node.Id = await _identityCommand.ExecuteScalarAsync() switch {
                int i => i,
                _ => throw new Exception("Insert of node does not return an id.")
            };
        }
        else {
            Set(CommentInserterFactory.Id, node.Id.Value);
            Set(CommentInserterFactory.NodeId, node.NodeId);
            Set(CommentInserterFactory.CommentIdParent, node.CommentIdParent);
            Set(CommentInserterFactory.PublisherId, node.PublisherId);
            Set(CommentInserterFactory.NodeStatusId, node.NodeStatusId);
            Set(CommentInserterFactory.IPAddress, node.IPAddress);
            Set(CommentInserterFactory.CreatedDateTime, node.CreatedDateTime);
            Set(CommentInserterFactory.Title, node.Title);
            Set(CommentInserterFactory.Text, node.Text);
            await _command.ExecuteNonQueryAsync();
        }
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _identityCommand.DisposeAsync();
    }
}
