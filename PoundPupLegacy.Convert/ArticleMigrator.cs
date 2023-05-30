namespace PoundPupLegacy.Convert;

internal sealed class ArticleMigrator(
    IDatabaseConnections databaseConnections,
    IEntityCreatorFactory<Document.ToCreate> documentCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "documents (articles)";

    protected override async Task MigrateImpl()
    {
        await using var documentCreator = await documentCreatorFactory.CreateAsync(_postgresConnection);
        await documentCreator.CreateAsync(ReadArticles());
    }
    private async IAsyncEnumerable<Document.ToCreate> ReadArticles()
    {

        var sql = $"""
                SELECT
                     n.nid id,
                     n.uid user_id,
                     n.title,
                     n.`status`,
                     FROM_UNIXTIME(n.created) created, 
                     FROM_UNIXTIME(n.changed) `changed`,
                     nr.body `text`
                FROM node n
                JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                WHERE n.`type` = 'article' AND n.uid <> 0
                """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {
            yield return new Document.ToCreate {
                Identification = new Identification.Possible {
                    Id = null
                },
                NodeDetails = new NodeDetails.NodeDetailsForCreate {
                    PublisherId = reader.GetInt32("user_id"),
                    CreatedDateTime = reader.GetDateTime("created"),
                    ChangedDateTime = reader.GetDateTime("changed"),
                    Title = reader.GetString("title"),
                    OwnerId = Constants.PPL,
                    AuthoringStatusId = 1,
                    TenantNodes = new List<TenantNode.ToCreateForNewNode>
                    {
                        new TenantNode.ToCreateForNewNode
                        {
                            Identification = new Identification.Possible {
                                Id = null
                            },
                            TenantId = 1,
                            PublicationStatusId = reader.GetInt32("status"),
                            UrlPath = null,
                            SubgroupId = null,
                            UrlId = reader.GetInt32("id")
                        }
                    },
                    NodeTypeId = 10,
                    TermIds = new List<int>(),
                },
                SimpleTextNodeDetails = new SimpleTextNodeDetails {
                    Text = TextToHtml(reader.GetString("text")),
                    Teaser = TextToTeaser(reader.GetString("text")),
                },
                DocumentDetails = new DocumentDetails {
                    Published = null,
                    SourceUrl = null,
                    DocumentTypeId = null,
                },
            };
        }
        await reader.CloseAsync();
    }

}
