namespace PoundPupLegacy.CreateModel.Updaters;

using Request = SimpleTextNodeToUpdate;
internal sealed class SimpleTextNodeChangerFactory(
    IDatabaseUpdaterFactory<Request> databaseUpdaterFactory,
    NodeDetailsChangerFactory nodeDetailsChangerFactory) : IEntityChangerFactory<Request>
{
    public async Task<IEntityChanger<Request>> CreateAsync(IDbConnection connection)
    {
        return new NodeChanger<Request>(
            await databaseUpdaterFactory.CreateAsync(connection),
            await nodeDetailsChangerFactory.CreateAsync(connection)
        );
    }
}

internal sealed class SimpleTextNodeUpdaterFactory : DatabaseUpdaterFactory<Request>
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
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(NodeId, request.Identification.Id),
            ParameterValue.Create(Title, request.NodeDetails.Title),
            ParameterValue.Create(Text, request.SimpleTextNodeDetails.Text),
            ParameterValue.Create(Teaser, request.SimpleTextNodeDetails.Teaser),
        };
    }
}

