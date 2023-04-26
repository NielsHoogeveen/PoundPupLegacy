namespace PoundPupLegacy.Convert;

internal sealed class DocumentableDocumentMigrator : MigratorPPL
{
    private readonly IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> _nodeIdReaderFactory;
    private readonly IEntityCreator<DocumentableDocument> _documentableDocumentCreator;
    public DocumentableDocumentMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
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

    private async IAsyncEnumerable<DocumentableDocument> ReadArticles(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {

        var sql = $"""
            SELECT
            distinct
            documentable_id,
            document_id
            FROM(
                SELECT
                case 
                	when n2.nid = 11108	then 10996
                	ELSE n2.nid
                end documentable_id,
                n.nid document_id,
                n2.title documentable
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
                n.nid document_id,
                n2.title documentable
                FROM content_field_pers_org cfr 
                JOIN node n ON n.nid = cfr.nid AND n.vid = cfr.vid
                JOIN content_type_adopt_ind_rep r ON r.nid = n.nid AND r.vid = n.vid
                JOIN node n2 ON n2.nid = cfr.field_pers_org_nid
            ) x
            LEFT JOIN (
            	select
            	n.nid,
            	cc.field_related_page_nid
            	FROM node n
            	JOIN category_node cn ON cn.nid = n.nid
            	JOIN node n2 ON n2.nid = cn.cid
            	JOIN content_type_category_cat cc ON cc.nid = n2.nid AND cc.vid = n2.vid
            ) n1 ON n1.nid = x.document_id AND n1.field_related_page_nid = x.documentable_id
            LEFT JOIN (
            	select
            	n.nid,
            	n3.title
            	FROM node n
            	JOIN category_node cn ON cn.nid = n.nid
            	JOIN node n3 ON n3.nid = cn.cid
            ) n2 ON n2.nid = x.document_id AND trim(n2.title) = trim(documentable)
            WHERE n1.nid IS NULL AND n2.nid is NULL
            """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {


            var documentableId = reader.GetInt32("documentable_id");
            var documentId = reader.GetInt32("document_id");
            yield return new DocumentableDocument {
                DocumentableId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = documentableId
                }),
                DocumentId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = documentId
                }),
            };
        }
        await reader.CloseAsync();
    }

}
