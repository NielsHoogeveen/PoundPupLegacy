namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CreateNodeActionInserterFactory : DatabaseInserterFactory<CreateNodeAction, CreateNodeActionInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    public override string TableName => "create_node_action";

}
internal sealed class CreateNodeActionInserter : DatabaseInserter<CreateNodeAction>
{
    public CreateNodeActionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(CreateNodeAction item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(CreateNodeActionInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(CreateNodeActionInserterFactory.NodeTypeId, item.NodeTypeId),
        };
    }
}
