namespace PoundPupLegacy.CreateModel.Inserters;

using Request = CreateNodeAction;

internal sealed class CreateNodeActionInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    public override string TableName => "create_node_action";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NodeTypeId, request.NodeTypeId),
        };
    }
}
