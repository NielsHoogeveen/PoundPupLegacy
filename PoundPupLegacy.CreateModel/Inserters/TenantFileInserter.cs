namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = TenantFileInserterFactory;
using Request = TenantFile;
using Inserter = TenantFileInserter;

internal sealed class TenantFileInserterFactory : DatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NullCheckingIntegerDatabaseParameter FileId = new() { Name = "file_id" };
    internal static NullCheckingIntegerDatabaseParameter TenantFileId = new() { Name = "tenant_file_id" };

    public override string TableName => "tenant_file";
}

internal sealed class TenantFileInserter : DatabaseInserter<Request>
{
    public TenantFileInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.TenantId, request.TenantId),
            ParameterValue.Create(Factory.FileId, request.FileId),
            ParameterValue.Create(Factory.TenantFileId, request.TenantFileId),
        };
    }
}
