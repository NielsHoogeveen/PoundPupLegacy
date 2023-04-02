using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.Updaters;

public class SimpleTextNodeUpdaterFactory : IDatabaseUpdaterFactory<SimpleTextNodeUpdater>
{
    public async Task<SimpleTextNodeUpdater> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;
        var command = postgresConnection.CreateCommand();

        var sql = $"""
                    update node set title=@title
                    where id = @node_id;
                    update simple_text_node set text=@text, teaser=@teaser
                    where id = @node_id;
                    """;
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;
        command.Parameters.Add("text", NpgsqlTypes.NpgsqlDbType.Varchar);
        command.Parameters.Add("teaser", NpgsqlTypes.NpgsqlDbType.Varchar);
        command.Parameters.Add("title", NpgsqlTypes.NpgsqlDbType.Varchar);
        command.Parameters.Add("node_id", NpgsqlTypes.NpgsqlDbType.Integer);
        await command.PrepareAsync();
        return new SimpleTextNodeUpdater(command);
    }
}

public class SimpleTextNodeUpdater: DatabaseUpdater<SimpleTextNodeUpdater.Request>
{
    public SimpleTextNodeUpdater(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task UpdateAsync(Request request)
    {
        _command.Parameters["title"].Value = request.Title;
        _command.Parameters["text"].Value = request.Text;
        _command.Parameters["teaser"].Value = request.Text;
        _command.Parameters["node_id"].Value = request.NodeId;
        var u = await _command.ExecuteNonQueryAsync();

    }

    public record Request
    {
        public required int NodeId { get; init; }
        public required string Title { get; init; }
        public required string Text { get; init; }
        public required string Teaser { get; init; }
   }
}
