namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class TenantFileInserterFactory : DatabaseInserterFactory<TenantFile>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NonNullableIntegerDatabaseParameter FileId = new() { Name = "file_id" };
    internal static NonNullableIntegerDatabaseParameter TenantFileId = new() { Name = "tenant_file_id" };

    public override async Task<IDatabaseInserter<TenantFile>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "tenant_file",
            new DatabaseParameter[] {
                TenantId,
                FileId,
                TenantFileId
            }
        );
        return new TenantFileInserter(command);
    }
}

internal sealed class TenantFileInserter : DatabaseInserter<TenantFile>
{
    internal TenantFileInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(TenantFile tenantFile)
    {
        if (tenantFile.FileId == null)
            throw new ArgumentNullException(nameof(tenantFile.FileId));
        if (tenantFile.TenantFileId == null)
            throw new ArgumentNullException(nameof(tenantFile.TenantFileId));
        Set(TenantFileInserterFactory.TenantId, tenantFile.TenantId);
        Set(TenantFileInserterFactory.FileId, tenantFile.FileId.Value);
        Set(TenantFileInserterFactory.TenantFileId, tenantFile.TenantFileId.Value);
        await _command.ExecuteNonQueryAsync();
    }
}
