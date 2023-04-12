namespace PoundPupLegacy.CreateModel.Readers;
public sealed class FileIdReaderByTenantFileIdFactory : DatabaseReaderFactory<FileIdReaderByTenantFileId>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NonNullableIntegerDatabaseParameter TenantFileId = new() { Name = "tenant_file_id" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT file_id FROM tenant_file WHERE tenant_id = @tenant_id and tenant_file_id = @tenant_file_id
        """;
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
