namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class SubgroupInserterFactory : BasicDatabaseInserterFactory<Subgroup, SubgroupInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };

    public override string TableName => "subgroup";
}
internal sealed class SubgroupInserter : BasicDatabaseInserter<Subgroup>
{
    public SubgroupInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(Subgroup item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(SubgroupInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(SubgroupInserterFactory.TenantId, item.TenantId),
        };
    }
}
