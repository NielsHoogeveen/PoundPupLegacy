namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class TenantFileInserterFactory : DatabaseInserterFactory<TenantFile>
{
    public override async Task<IDatabaseInserter<TenantFile>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "tenant_file",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = TenantFileInserter.TENANT_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = TenantFileInserter.FILE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = TenantFileInserter.TENANT_FILE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new TenantFileInserter(command);
    }
}

internal sealed class TenantFileInserter : DatabaseInserter<TenantFile>
{
    internal const string TENANT_ID = "tenant_id";
    internal const string FILE_ID = "file_id";
    internal const string TENANT_FILE_ID = "tenant_file_id";

    internal TenantFileInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(TenantFile tenantFile)
    {
        if (tenantFile.FileId == null)
            throw new ArgumentNullException(nameof(tenantFile.FileId));
        SetParameter(tenantFile.TenantId, TENANT_ID);
        SetParameter(tenantFile.FileId, FILE_ID);
        SetParameter(tenantFile.TenantFileId, TENANT_FILE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
