namespace PoundPupLegacy.Convert;

internal sealed class NodeTermMigrator(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameableIdRequest, int> termIdReaderByNameableIdFactory,
    IEntityCreatorFactory<ResolvedNodeTermToAdd> nodeTermCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "node terms";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termReaderByNameableId = await termIdReaderByNameableIdFactory.CreateAsync(_postgresConnection);

        await using var nodeTermCreator = await nodeTermCreatorFactory.CreateAsync(_postgresConnection);
        await nodeTermCreator.CreateAsync(ReadNodeTerms(nodeIdReader, termReaderByNameableId));
    }
    private async IAsyncEnumerable<ResolvedNodeTermToAdd> ReadNodeTerms(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameableIdRequest, int> termIdReaderByNameableId)
    {
        var vocabularyIdTopics = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_TOPICS
        });

        var sql = $"""
            SELECT
            *
            FROM
            (
                SELECT
                    DISTINCT
                    n.nid node_id,
                    case
                        when n3.node_id IS NULL then n4.node_id
                        ELSE n3.node_id
                    end nameable_id,
                    case 
                        when n3.`type` IS NULL then n4.`type`
                        else n3.`type`
                        END nameable_type
                FROM node n
                JOIN category_node cn ON cn.nid = n.nid
                JOIN category c ON c.cid = cn.cid AND c.cnid = 4126
                LEFT JOIN(
                    SELECT 
                    cc.nid,
                    n.nid node_id,
                    n.`type`
                    FROM content_type_category_cat cc 
                    JOIN node n3 ON n3.nid = cc.nid AND n3.vid = cc.vid
                    JOIN node n ON n.nid = cc.field_related_page_nid
                    WHERE n.`type` NOT IN ('group') 
                ) n3 ON n3.nid = c.cid
                LEFT JOIN(
                    SELECT 
                    case 
                        when n2.nid is NULL then n.nid 
                        ELSE n2.nid
                    end node_id,
                    n.nid,
                    n2.`type`
                    FROM node n 
                    LEFT JOIN node n2 ON n2.title = n.title and n2.`type` in ('adopt_person','country_type', 'adopt_orgs', 'case', 'region_facts', 'coerced_adoption_cases', 'child_trafficking', 'child_trafficking_case', 'statefact')
                ) n4 ON n4.nid = c.cid
                WHERE  n.nid NOT IN (
                    22589,
                    54123
                )
                AND (n3.nid IS NOT NULL OR n4.nid IS NOT NULL)
                AND n.`type` NOT IN ('amazon_node', 'poll', 'video', 'amazon', 'website', 'image', 'book_page', 'panel', 'viewnode')
                AND n.uid <> 0
            ) x
            WHERE nameable_id <> 45656
            """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {
            var nodeId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = reader.GetInt32("node_id"),
            });
            var (nameableUrlId, tenantId) = GetUrlIdAndTenant(reader.GetInt32("nameable_id"));
            var nameableId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = tenantId,
                UrlId = nameableUrlId,
            });

            var termId = await termIdReaderByNameableId.ReadAsync(new TermIdReaderByNameableIdRequest {
                NameableId = nameableId,
                VocabularyId = vocabularyIdTopics,
            });
            yield return new ResolvedNodeTermToAdd {
                NodeId = nodeId,
                TermId = termId,
            };

        }
        await reader.CloseAsync();
    }
}
