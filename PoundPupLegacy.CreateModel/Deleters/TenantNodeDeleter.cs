namespace PoundPupLegacy.CreateModel.Deleters;

using Request = ImmediatelyIdentifiableTenantNode;

internal sealed class TenantNodeDeleterFactory : DatabaseDeleterFactory<Request>
{

    private static readonly NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };

    public override string Sql => SQL;

    const string SQL = $"""
        delete from tenant_node
        where id = @id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Id, request.Id),
        };
    }
}
