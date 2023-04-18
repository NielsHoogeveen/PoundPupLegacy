namespace PoundPupLegacy.CreateModel.Inserters;

using Request = TenantFile;

internal sealed class TenantFileInserterFactory : BasicDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NullCheckingIntegerDatabaseParameter FileId = new() { Name = "file_id" };
    internal static NullCheckingIntegerDatabaseParameter TenantFileId = new() { Name = "tenant_file_id" };

    public override string TableName => "tenant_file";
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantId, request.TenantId),
            ParameterValue.Create(FileId, request.FileId),
            ParameterValue.Create(TenantFileId, request.TenantFileId),
        };
    }
}

