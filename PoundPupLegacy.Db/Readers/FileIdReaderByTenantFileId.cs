using System.Data;

namespace PoundPupLegacy.Db.Readers;

public sealed class FileIdReaderByTenantFileId : DatabaseUpdater<Term>, IDatabaseReader<FileIdReaderByTenantFileId>
{
    public static async Task<FileIdReaderByTenantFileId> CreateAsync(NpgsqlConnection connection)
    {
        var sql = """
            SELECT file_id FROM tenant_file WHERE tenant_id = @tenant_id and tenant_file_id = @tenant_file_id
            """;

        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;

        command.Parameters.Add("tenant_id", NpgsqlDbType.Integer);
        command.Parameters.Add("tenant_file_id", NpgsqlDbType.Integer);
        await command.PrepareAsync();

        return new FileIdReaderByTenantFileId(command);

    }

    internal FileIdReaderByTenantFileId(NpgsqlCommand command) : base(command) { }

    public async Task<int> ReadAsync(int TenantId, int TenantFileId)
    {
        _command.Parameters["tenant_id"].Value = TenantId;
        _command.Parameters["tenant_file_id"].Value = TenantFileId;

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows) {
            await reader.ReadAsync();
            var id = reader.GetInt32("file_id");
            await reader.CloseAsync();
            return id;
        }
        await reader.CloseAsync();
        throw new Exception($"File id cannot be found for tenant {TenantId} and file id {TenantFileId}");
    }
}
