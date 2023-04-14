namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = SubgroupInserterFactory;
using Request = Subgroup;
using Inserter = SubgroupInserter;

internal sealed class SubgroupInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };

    public override string TableName => "subgroup";
}
internal sealed class SubgroupInserter : IdentifiableDatabaseInserter<Request>
{
    public SubgroupInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.TenantId, request.TenantId),
        };
    }
}
