using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.EditModel;
using System.Data;

namespace PoundPupLegacy.Deleters;
public class FileDeleterFactory : IDatabaseDeleterFactory<FileDeleter>
{
    public async Task<FileDeleter> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = postgresConnection.CreateCommand();

        var sql = $"""
                delete from node_file
                where file_id = @file_id and node_id = @node_id;
                delete from tenant_file
                where file_id in (
                    select 
                    id 
                    from file f
                    left join node_file nf on nf.file_id = f.id
                    where nf.file_id is null
                    and f.id = @file_id
                );
                delete from file
                where id in (
                    select 
                    id 
                    from file f
                    left join node_file nf on nf.file_id = f.id
                    where nf.file_id is null
                    and f.id = @file_id
                );
                """;
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;
        command.Parameters.Add("file_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("node_id", NpgsqlTypes.NpgsqlDbType.Integer);
        await command.PrepareAsync();
        return new FileDeleter(command);
    }
}
public class FileDeleter : DatabaseDeleter<FileDeleter.Request>
{
    public record Request
    {
        public required int NodeId { get; init; }
        public required int FileId { get; init; }
    }
    public FileDeleter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task DeleteAsync(Request request)
    {
        _command.Parameters["node_id"].Value = request.NodeId;
        _command.Parameters["file_id"].Value = request.FileId;
        await _command.ExecuteNonQueryAsync();
    }
}
