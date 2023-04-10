namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class DeleteNodeActionInserterFactory : BasicDatabaseInserterFactory<DeleteNodeAction, DeleteNodeActionInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    public override string TableName => "delete_node_action";
}
internal sealed class DeleteNodeActionInserter : BasicDatabaseInserter<DeleteNodeAction>
{

    public DeleteNodeActionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(DeleteNodeAction item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(DeleteNodeActionInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(DeleteNodeActionInserterFactory.NodeTypeId, item.NodeTypeId),
        };
    }
}
