namespace PoundPupLegacy.CreateModel.Inserters;

using Request = EditNodeAction;

internal sealed class EditNodeActionInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    public override string TableName => "edit_node_action";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NodeTypeId, item.NodeTypeId),
        };
    }
}
