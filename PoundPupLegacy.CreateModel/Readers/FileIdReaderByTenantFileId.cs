namespace PoundPupLegacy.CreateModel.Readers;

using Factory = FileIdReaderByTenantFileIdFactory;
using Reader = FileIdReaderByTenantFileId;
public sealed class FileIdReaderByTenantFileIdFactory : DatabaseReaderFactory<Reader>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NonNullableIntegerDatabaseParameter TenantFileId = new() { Name = "tenant_file_id" };

    internal static IntValueReader IdReader = new() { Name = "id" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT file_id id FROM tenant_file WHERE tenant_id = @tenant_id and tenant_file_id = @tenant_file_id
        """;
}

public sealed class FileIdReaderByTenantFileId : IntDatabaseReader<Reader.Request>
{
    public record Request
    {
        public required int TenantId { get; init; }
        public required int TenantFileId { get; init; }
    }

    internal FileIdReaderByTenantFileId(NpgsqlCommand command) : base(command) { }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.TenantId, request.TenantId),
            ParameterValue.Create(Factory.TenantFileId, request.TenantFileId)
        };
    }

    protected override IntValueReader IntValueReader => Factory.IdReader;

    protected override string GetErrorMessage(Request request)
    {
        return $"File id cannot be found for tenant {request.TenantId} and file id {request.TenantFileId}";
    }
}
