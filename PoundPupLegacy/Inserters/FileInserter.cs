using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.Inserters;

internal sealed class FileInserter : DatabaseInserter<FileInserter.Request>, IDatabaseInserter<FileInserter.Request>
{
    public record Request
    {
        public required string Name { get; init; }
        public required string MimeType { get; init; }
        public required string Path { get; init; }
        public required int NodeId { get; init; }
        public required long Size { get; init; }
    }

    public static async Task<DatabaseInserter<Request>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = postgresConnection.CreateCommand();
        var sql = $"""
                insert into file (name, size, mime_type, path) VALUES(@name, @size, @mime_type, @path);
                insert into node_file (node_id, file_id) VALUES(@node_id, lastval());
                """;
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;
        command.Parameters.Add("name", NpgsqlTypes.NpgsqlDbType.Varchar);
        command.Parameters.Add("size", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("mime_type", NpgsqlTypes.NpgsqlDbType.Varchar);
        command.Parameters.Add("path", NpgsqlTypes.NpgsqlDbType.Varchar);
        command.Parameters.Add("node_id", NpgsqlTypes.NpgsqlDbType.Integer);
        await command.PrepareAsync();
        return new FileInserter(command);

    }

    internal FileInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Request request)
    {
        _command.Parameters["name"].Value = request.Name;
        _command.Parameters["size"].Value = request.Size;
        _command.Parameters["mime_type"].Value = request.MimeType;
        _command.Parameters["path"].Value = request.Path;
        _command.Parameters["node_id"].Value = request.NodeId;
        await _command.ExecuteNonQueryAsync();

    }

}
