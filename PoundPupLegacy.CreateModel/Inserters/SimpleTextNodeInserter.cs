namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class SimpleTextNodeInserterFactory : DatabaseInserterFactory<SimpleTextNode, SimpleTextNodeInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Text = new() { Name = "text" };
    internal static NonNullableStringDatabaseParameter Teaser = new() { Name = "teaser" };

    public override string TableName => "simple_text_node";
}
internal sealed class SimpleTextNodeInserter : DatabaseInserter<SimpleTextNode>
{

    public SimpleTextNodeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(SimpleTextNode item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(SimpleTextNodeInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(SimpleTextNodeInserterFactory.Text, item.Text),
            ParameterValue.Create(SimpleTextNodeInserterFactory.Teaser, item.Teaser),
        };
    }
}
