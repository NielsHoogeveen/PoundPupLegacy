namespace PoundPupLegacy.DomainModel.Inserters;

using Request = NodeAccess;

internal sealed class NodeAccessInserterFactory : AutoGenerateIdDatabaseInserterFactory<Request>
{
    private static readonly NonNullableDateTimeWithTimeZoneDatabaseParameter DateTime = new() { Name = "date_time" };
    private static readonly NonNullableIntegerDatabaseParameter UserId = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };

    public override string TableName => "node_access";
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(DateTime, request.DateTime),
            ParameterValue.Create(UserId, request.UserId),
            ParameterValue.Create(TenantId, request.TenantId),
            ParameterValue.Create(NodeId, request.NodeId),
        };
    }
}
