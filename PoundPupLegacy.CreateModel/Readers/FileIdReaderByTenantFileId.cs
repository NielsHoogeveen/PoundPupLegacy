namespace PoundPupLegacy.CreateModel.Readers;
public sealed class FileIdReaderByTenantFileIdFactory : IDatabaseReaderFactory<FileIdReaderByTenantFileId>
{
    public async Task<FileIdReaderByTenantFileId> CreateAsync(IDbConnection connection)
    {
        var sql = """
            SELECT file_id FROM tenant_file WHERE tenant_id = @tenant_id and tenant_file_id = @tenant_file_id
            """;

        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;
        var command = postgresConnection.CreateCommand();

        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;

        command.Parameters.Add("tenant_id", NpgsqlDbType.Integer);
        command.Parameters.Add("tenant_file_id", NpgsqlDbType.Integer);
        await command.PrepareAsync();

        return new FileIdReaderByTenantFileId(command);

    }
}

public sealed class FileIdReaderByTenantFileId : SingleItemDatabaseReader<FileIdReaderByTenantFileId.Request, int>
{
    public record Request
    {
        public required int TenantId { get; init; }
        public required int TenantFileId { get; init; }
    }

    internal FileIdReaderByTenantFileId(NpgsqlCommand command) : base(command) { }

    public override async Task<int> ReadAsync(Request request)
    {
        _command.Parameters["tenant_id"].Value = request.TenantId;
        _command.Parameters["tenant_file_id"].Value = request.TenantFileId;

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows) {
            await reader.ReadAsync();
            var id = reader.GetInt32("file_id");
            await reader.CloseAsync();
            return id;
        }
        await reader.CloseAsync();
        throw new Exception($"File id cannot be found for tenant {request.TenantId} and file id {request.TenantFileId}");
    }
}
