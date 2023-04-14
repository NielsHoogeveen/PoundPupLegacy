namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class EditNodeActionInserterFactory : DatabaseInserterFactory<EditNodeAction, EditNodeActionInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    public override string TableName => "edit_node_action";
}
internal sealed class EditNodeActionInserter : DatabaseInserter<EditNodeAction>
{
    public EditNodeActionInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(EditNodeAction item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(EditNodeActionInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(EditNodeActionInserterFactory.NodeTypeId, item.NodeTypeId),
        };
    }
}
