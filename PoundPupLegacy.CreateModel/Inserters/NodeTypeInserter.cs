namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class NodeTypeInserterFactory : DatabaseInserterFactory<NodeType, NodeTypeInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    internal static NonNullableBooleanDatabaseParameter AuthorSpecific = new() { Name = "author_specific" };

    public override string TableName => "node_type";

}
internal sealed class NodeTypeInserter : DatabaseInserter<NodeType>
{
    public NodeTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(NodeType item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(NodeTypeInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(NodeTypeInserterFactory.Name, item.Name),
            ParameterValue.Create(NodeTypeInserterFactory.Description, item.Description),
            ParameterValue.Create(NodeTypeInserterFactory.AuthorSpecific, item.AuthorSpecific),
        };
    }
}
