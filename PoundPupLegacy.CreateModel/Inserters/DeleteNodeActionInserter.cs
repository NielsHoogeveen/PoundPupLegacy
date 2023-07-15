namespace PoundPupLegacy.DomainModel.Inserters;

using Request = DeleteNodeAction;

internal sealed class DeleteNodeActionInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    public override string TableName => "delete_node_action";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NodeTypeId, item.NodeTypeId),
        };
    }
}
