namespace PoundPupLegacy.DomainModel.Inserters;

using Request = Subgroup;

internal sealed class SubgroupInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };

    public override string TableName => "subgroup";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantId, request.TenantId),
        };
    }
}
