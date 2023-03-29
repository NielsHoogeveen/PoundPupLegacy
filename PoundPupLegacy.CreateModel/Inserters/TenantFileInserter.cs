using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PoundPupLegacy.Db.Test")]
namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class TenantFileInserter : DatabaseInserter<TenantFile>, IDatabaseInserter<TenantFile>
{
    private const string TENANT_ID = "tenant_id";
    private const string FILE_ID = "file_id";
    private const string TENANT_FILE_ID = "tenant_file_id";
    public static async Task<DatabaseInserter<TenantFile>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "tenant_file",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = TENANT_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = FILE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = TENANT_FILE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new TenantFileInserter(command);

    }

    internal TenantFileInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(TenantFile tenantFile)
    {
        if (tenantFile.FileId == null)
            throw new ArgumentNullException(nameof(tenantFile.FileId));
        WriteValue(tenantFile.TenantId, TENANT_ID);
        WriteValue(tenantFile.FileId, FILE_ID);
        WriteValue(tenantFile.TenantFileId, TENANT_FILE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
