using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.Inserters;
public sealed class FileInserterFactory : IDatabaseInserterFactory<FileInserterFactory.Request>
{
    public record Request
    {
        public required string Name { get; init; }
        public required string MimeType { get; init; }
        public required string Path { get; init; }
        public required int NodeId { get; init; }
        public required long Size { get; init; }
    }

    internal static NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    internal static NonNullableStringDatabaseParameter Path = new() { Name = "path" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableStringDatabaseParameter MimeType = new() { Name = "mime_type" };
    internal static NonNullableLongDatabaseParameter Size = new() { Name = "size" };

    public async Task<IDatabaseInserter<Request>> CreateAsync(IDbConnection connection)
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
        var parameters = new DatabaseParameter[] {
            Name,
            Size,
            MimeType,
            Path,
            NodeId
        };
        foreach (var parameter in parameters) 
        { 
            command.AddParameter(parameter);
        }
        await command.PrepareAsync();
        return new FileInserter(command);
    }

}   

internal sealed class FileInserter : DatabaseWriter, IDatabaseInserter<FileInserterFactory.Request>
{
    internal FileInserter(NpgsqlCommand command) : base(command)
    {
    }

    public async Task InsertAsync(FileInserterFactory.Request request)
    {
        var parameterValues = new ParameterValue[] {
            ParameterValue.Create(FileInserterFactory.Name, request.Name),
            ParameterValue.Create(FileInserterFactory.Size, request.Size),
            ParameterValue.Create(FileInserterFactory.MimeType, request.MimeType),
            ParameterValue.Create(FileInserterFactory.Path, request.Path),
            ParameterValue.Create(FileInserterFactory.NodeId, request.NodeId)
        };
        Set(parameterValues);
        await _command.ExecuteNonQueryAsync();
    }
}
