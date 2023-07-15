using PoundPupLegacy.DomainModel;
using PoundPupLegacy.DomainModel.Creators;
using PoundPupLegacy.DomainModel.Readers;

namespace PoundPupLegacy.Convert;

internal sealed class DocumentableDocumentMigrator(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameableIdRequest, int> termReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    IEntityCreatorFactory<ResolvedNodeTermToAdd> nodeTermCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "documentable documents";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termReader = await termReaderFactory.CreateAsync(_postgresConnection);
        await using var nodeTermCreator = await nodeTermCreatorFactory.CreateAsync(_postgresConnection);
        await nodeTermCreator.CreateAsync(ReadArticles(termReader,nodeIdReader));

    }

    private async IAsyncEnumerable<ResolvedNodeTermToAdd> ReadArticles(
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameableIdRequest, int> termReader,
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

        var vocabularyIdTopics = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_TOPICS
        });

        while (await reader.ReadAsync()) {


            var documentableId = reader.GetInt32("documentable_id");
            var documentId = reader.GetInt32("document_id");
            var nodeId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = documentId
            });
            var nameableId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = documentableId
            });
            var termId = await termReader.ReadAsync(new TermIdReaderByNameableIdRequest {
                NameableId = nameableId,
                VocabularyId = vocabularyIdTopics,
            });
            yield return new ResolvedNodeTermToAdd { NodeId = nodeId, TermId = termId };
        }
        await reader.CloseAsync();
    }

}
