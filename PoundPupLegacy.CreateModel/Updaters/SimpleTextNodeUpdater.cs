namespace PoundPupLegacy.CreateModel.Updaters;

internal abstract class SimpleTextNodeUpdaterFactory<TRequest> : DatabaseUpdaterFactory<TRequest>
    where TRequest: SimpleTextNodeToUpdate
{
    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableStringDatabaseParameter Text = new() { Name = "text" };
    private static readonly NonNullableStringDatabaseParameter Teaser = new() { Name = "teaser" };
    private static readonly NonNullableStringDatabaseParameter Title = new() { Name = "title" };

    public override string Sql => $"""
        update node set title=@title
        where id = @node_id;
        update simple_text_node set text=@text, teaser=@teaser
        where id = @node_id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(TRequest request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(NodeId, request.Identification.Id),
            ParameterValue.Create(Title, request.NodeDetails.Title),
            ParameterValue.Create(Text, request.SimpleTextNodeDetails.Text),
            ParameterValue.Create(Teaser, request.SimpleTextNodeDetails.Teaser),
        };
    }
}

