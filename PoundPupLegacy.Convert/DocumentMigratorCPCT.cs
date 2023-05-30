using PoundPupLegacy.CreateModel.Creators;
using System.Xml.Linq;

namespace PoundPupLegacy.Convert;

internal sealed class DocumentMigratorCPCT(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameableIdRequest, int> termReaderFactory,
    ISingleItemDatabaseReaderFactory<TenantNodeReaderByUrlIdRequest, TenantNode.ToCreateForExistingNode> tenantNodeReaderByUrlIdFactory,
    IEntityCreatorFactory<Document.ToCreate> documentCreatorFactory
) : MigratorCPCT(
    databaseConnections, 
    nodeIdReaderFactory, 
    tenantNodeReaderByUrlIdFactory
)
{
    protected override string Name => "documents cpct";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termReader = await termReaderFactory.CreateAsync(_postgresConnection);
        await using var tenantNodeReader = await tenantNodeReaderByUrlIdFactory.CreateAsync(_postgresConnection);
        await using var documentCreator = await documentCreatorFactory.CreateAsync(_postgresConnection);
        await documentCreator.CreateAsync(ReadDocuments(nodeIdReader, termReader, tenantNodeReader));
    }

    private async IAsyncEnumerable<(int, int)> GetDocumentablesWithStatus(
        IEnumerable<int> documentableIds,
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        ISingleItemDatabaseReader<TenantNodeReaderByUrlIdRequest, TenantNode.ToCreateForExistingNode> tenantNodeReader)
    {
        foreach (var urlId in documentableIds) {
            yield return await GetNodeId(urlId, nodeIdReader, tenantNodeReader);
        }
    }

    private async IAsyncEnumerable<Document.ToCreate> ReadDocuments(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameableIdRequest, int> termReader,
        ISingleItemDatabaseReader<TenantNodeReaderByUrlIdRequest, TenantNode.ToCreateForExistingNode> tenantNodeReader)
    {


        var sql = $"""
            SELECT
                n.nid id,
                n.uid user_id,
                n.title,
                n.`status`,
                FROM_UNIXTIME(n.created) created, 
                FROM_UNIXTIME(n.changed) `changed`,
                10 node_type_id,
                r.field_report_date_value publication_date,
                case when r.field_web_address_url = '' then null else r.field_web_address_url end source_url,
                nr.body `text`,
                c.document_type_id,
                GROUP_CONCAT(dd.documentable_id) documentable_ids
            FROM node n
            JOIN content_type_adopt_ind_rep r ON r.nid = n.nid AND r.vid = n.vid
            JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
            LEFT JOIN (
            select 
                c.nid,
                n2.nid document_type_id
            FROM category_node c 
            JOIN node n2 ON n2.nid = c.cid
            JOIN category cat ON cat.cid = c.cid AND cat.cnid = 42416 
            JOIN node n3 ON n3.nid = cat.cnid 
            ) c ON c.nid = n.nid 
            LEFT JOIN (
                SELECT
                n2.nid documentable_id,
                n.nid document_id
                FROM content_field_pers_org cfr 
                JOIN node n ON n.nid = cfr.nid AND n.vid = cfr.vid
                JOIN content_type_adopt_ind_rep r ON r.nid = n.nid AND r.vid = n.vid
                JOIN node n2 ON n2.nid = cfr.field_pers_org_nid
            ) dd ON dd.document_id = n.nid
            WHERE n.`type` = 'adopt_ind_rep'
            AND n.nid > 33162
            GROUP BY
                n.nid,
                n.uid,
                n.title,
                n.`status`,
                n.created, 
                n.`changed`,
                r.field_report_date_value,
                r.field_web_address_url,
                nr.body,
                c.document_type_id
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
            var publicationDate = StringToDateTimeRange(reader.IsDBNull("publication_date")
                ? null
                : reader.GetString("publication_date"))?.ToFuzzyDate();
            var id = reader.GetInt32("id");
            var text = reader.GetString("text");

            var documentableIds = reader.IsDBNull("documentable_ids") ?
                new List<int>() :
                reader
                .GetString("documentable_ids")
                .Split(',')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => int.Parse(x))
                .Distinct()
                .ToList();

            var documentable = await GetDocumentablesWithStatus(documentableIds, nodeIdReader, tenantNodeReader).ToListAsync();

            var tenantNodes = new List<TenantNode.ToCreateForNewNode>
                {
                    new TenantNode.ToCreateForNewNode
                    {
                        Identification = new Identification.Possible {
                            Id = null
                        },
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                };

            if (documentable.All(x => x.Item2 == 1) && !text.ToLower().Contains("arun dohle") && !text.ToLower().Contains("roelie post") && !text.ToLower().Contains("againstchildtrafficking.org")) {
                tenantNodes.Add(new TenantNode.ToCreateForNewNode {
                    Identification = new Identification.Possible {
                        Id = null
                    },
                    TenantId = Constants.PPL,
                    PublicationStatusId = 1,
                    UrlPath = null,
                    SubgroupId = null,
                    UrlId = null
                });
            }
            List<int> termIds = new();
            foreach (var nameableId in documentable.Select(x => x.Item1)) {
                var termId = await termReader.ReadAsync(new TermIdReaderByNameableIdRequest {
                    NameableId = nameableId,
                    VocabularyId = vocabularyIdTopics,
                });
                termIds.Add(termId);
            }
            yield return new Document.ToCreate {
                Identification = new Identification.Possible {
                    Id = null
                },
                NodeDetails = new NodeDetails.ForCreate {
                    PublisherId = reader.GetInt32("user_id"),
                    CreatedDateTime = reader.GetDateTime("created"),
                    ChangedDateTime = reader.GetDateTime("changed"),
                    Title = reader.GetString("title"),
                    OwnerId = Constants.OWNER_DOCUMENTATION,
                    AuthoringStatusId = 1,
                    TenantNodes = tenantNodes,
                    NodeTypeId = reader.GetInt16("node_type_id"),
                    TermIds = termIds,
                },
                SimpleTextNodeDetails = new SimpleTextNodeDetails {
                    Text = TextToHtml(text),
                    Teaser = TextToTeaser(text),
                },
                DocumentDetails = new DocumentDetails {
                    Published = publicationDate,
                    SourceUrl = reader.IsDBNull("source_url") ? null : reader.GetString("source_url"),
                    DocumentTypeId = reader.IsDBNull("document_type_id")
                    ? null
                    : await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                        TenantId = Constants.CPCT,
                        UrlId = reader.GetInt32("document_type_id")
                    }),
                },
            };
        }
        await reader.CloseAsync();
    }
}
