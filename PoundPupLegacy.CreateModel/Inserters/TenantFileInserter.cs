namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class TenantFileInserterFactory : DatabaseInserterFactory<TenantFile, TenantFileInserter>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NonNullableIntegerDatabaseParameter FileId = new() { Name = "file_id" };
    internal static NonNullableIntegerDatabaseParameter TenantFileId = new() { Name = "tenant_file_id" };

    public override string TableName => "tenant_file";
}

internal sealed class TenantFileInserter : DatabaseInserter<TenantFile>
{
    public TenantFileInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(TenantFile item)
    {
        if (item.FileId == null)
            throw new ArgumentNullException(nameof(item.FileId));
        if (item.TenantFileId == null)
            throw new ArgumentNullException(nameof(item.TenantFileId));
        return new ParameterValue[] {
            ParameterValue.Create(TenantFileInserterFactory.TenantId, item.TenantId),
            ParameterValue.Create(TenantFileInserterFactory.FileId, item.FileId.Value),
            ParameterValue.Create(TenantFileInserterFactory.TenantFileId, item.TenantFileId.Value),
        };
    }
}
