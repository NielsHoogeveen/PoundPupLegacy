namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class EditOwnNodeActionInserterFactory : DatabaseInserterFactory<EditOwnNodeAction, EditOwnNodeActionInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    public override string TableName => "edit_own_node_action";
}
internal sealed class EditOwnNodeActionInserter : DatabaseInserter<EditOwnNodeAction>
{
    public EditOwnNodeActionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(EditOwnNodeAction item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(EditOwnNodeActionInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(EditOwnNodeActionInserterFactory.NodeTypeId, item.NodeTypeId),
        };
    }
}
