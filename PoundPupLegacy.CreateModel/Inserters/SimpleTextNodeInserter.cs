namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = SimpleTextNodeInserterFactory;
using Request = SimpleTextNode;
using Inserter = SimpleTextNodeInserter;

internal sealed class SimpleTextNodeInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableStringDatabaseParameter Text = new() { Name = "text" };
    internal static NonNullableStringDatabaseParameter Teaser = new() { Name = "teaser" };

    public override string TableName => "simple_text_node";
}
internal sealed class SimpleTextNodeInserter : IdentifiableDatabaseInserter<Request>
{

    public SimpleTextNodeInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Text, request.Text),
            ParameterValue.Create(Factory.Teaser, request.Teaser),
        };
    }
}
