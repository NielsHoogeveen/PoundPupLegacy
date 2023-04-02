namespace PoundPupLegacy.Convert;

internal sealed class DocumentableDocumentMigrator : MigratorPPL
{
    private readonly IDatabaseReaderFactory<NodeIdReaderByUrlId> _nodeIdReaderFactory;
    private readonly IEntityCreator<DocumentableDocument> _documentableDocumentCreator;
    public DocumentableDocumentMigrator(
        IDatabaseConnections databaseConnections,
        IDatabaseReaderFactory<NodeIdReaderByUrlId> nodeIdReaderFactory,
        IEntityCreator<DocumentableDocument> documentableDocumentCreator
    ) : base(databaseConnections)
    {
        _nodeIdReaderFactory = nodeIdReaderFactory;
        _documentableDocumentCreator = documentableDocumentCreator;
    }

    protected override string Name => "documentable documents";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await _nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await _documentableDocumentCreator.CreateAsync(ReadArticles(nodeIdReader), _postgresConnection);

    }

    private async IAsyncEnumerable<DocumentableDocument> ReadArticles(NodeIdReaderByUrlId nodeIdReader)
    {

        var sql = $"""
                SELECT
                case 
                	when n2.nid = 11108	then 10996
                	ELSE n2.nid
                end documentable_id,
                n.nid document_id
                FROM content_field_cases cfr 
                JOIN node n ON n.nid = cfr.nid AND n.vid = cfr.vid
                JOIN content_type_case_file r ON r.nid = n.nid AND r.vid = n.vid
                JOIN node n2 ON n2.nid = cfr.field_cases_nid
                UNION
                SELECT
                case 
                	when n2.nid = 11108	then 10996
                	ELSE n2.nid
                end documentable_id,
                n.nid document_id
                FROM content_field_pers_org cfr 
                JOIN node n ON n.nid = cfr.nid AND n.vid = cfr.vid
                JOIN content_type_adopt_ind_rep r ON r.nid = n.nid AND r.vid = n.vid
                JOIN node n2 ON n2.nid = cfr.field_pers_org_nid
                """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {

            yield return new DocumentableDocument {
                DocumentableId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                    TenantId = Constants.PPL,
                    UrlId = reader.GetInt32("documentable_id")
                }),
                DocumentId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                    TenantId = Constants.PPL,
                    UrlId = reader.GetInt32("document_id")
                }),
            };
        }
        await reader.CloseAsync();
    }

}
