namespace PoundPupLegacy.CreateModel.Inserters;

using Request = ViewNodeTypeListAction;
internal sealed class ViewNodeTypeListActionInserterFactory : BasicDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter BasicActionId = new() { Name = "basic_action_id" };
    private static readonly NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };
    public override string TableName => "view_node_type_list_action";

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(BasicActionId, request.BasicActionId),
            ParameterValue.Create(NodeTypeId, request.NodeTypeId),
        };
    }
}
