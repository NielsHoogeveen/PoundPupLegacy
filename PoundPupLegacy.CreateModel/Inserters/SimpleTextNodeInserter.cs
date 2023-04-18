namespace PoundPupLegacy.CreateModel.Inserters;

using Request = SimpleTextNode;

internal sealed class SimpleTextNodeInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NonNullableStringDatabaseParameter Text = new() { Name = "text" };
    internal static NonNullableStringDatabaseParameter Teaser = new() { Name = "teaser" };

    public override string TableName => "simple_text_node";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Text, request.Text),
            ParameterValue.Create(Teaser, request.Teaser),
        };
    }
}
