namespace PoundPupLegacy.CreateModel.Updaters;

using Request = ImmediatelyIdentifiableDocument;
internal sealed class DocumentChangerFactory(
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

internal sealed class DocumentUpdaterFactory : DatabaseUpdaterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableStringDatabaseParameter Text = new() { Name = "text" };
    private static readonly NonNullableStringDatabaseParameter Teaser = new() { Name = "teaser" };
    private static readonly NonNullableStringDatabaseParameter Title = new() { Name = "title" };
    private static readonly NullableFuzzyDateDatabaseParameter Published = new() { Name = "published" };
    private static readonly NullableStringDatabaseParameter SourceUrl = new() { Name = "source_url" };
    private static readonly NonNullableIntegerDatabaseParameter DocumentTypeId = new() { Name = "document_type_id" };

    public override string Sql => $"""
        update node set title=@title
        where id = @node_id;
        update simple_text_node set text=@text, teaser=@teaser
        where id = @node_id;
        update document set published=@published, document_type_id=@document_type_id, source_url=@source_url
        where id = @node_id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(Title, request.Title),
            ParameterValue.Create(NodeId, request.Id),
            ParameterValue.Create(Text, request.Text),
            ParameterValue.Create(Teaser, request.Teaser),
            ParameterValue.Create(DocumentTypeId, request.DocumentTypeId),
            ParameterValue.Create(SourceUrl, request.SourceUrl),
            ParameterValue.Create(Published, request.Published),
        };
    }
}

