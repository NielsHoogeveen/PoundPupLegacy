namespace PoundPupLegacy.Convert;

internal sealed class HagueStatusMigrator(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    IEntityCreatorFactory<HagueStatus.ToCreate> hagueStatusCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "Hague statuses";

    protected override async Task MigrateImpl()
    {
        await using var hagueStatusCreator = await hagueStatusCreatorFactory.CreateAsync(_postgresConnection);
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await hagueStatusCreator.CreateAsync(ReadHagueStatuses(nodeIdReader));
    }

    private async IAsyncEnumerable<HagueStatus.ToCreate> ReadHagueStatuses(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {

        var sql = $"""
                    SELECT
                       n.nid id,
                       n.uid access_role_id,
                       n.title,
                       n.`status` node_status_id,
                       FROM_UNIXTIME(n.created) created_date_time, 
                       FROM_UNIXTIME(n.changed) changed_date_time,
                       ua.dst url_path
                    FROM node n
                    LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                    JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                    JOIN category c ON c.cid = n.nid AND c.cnid = 41212
                """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();
        var vocabularyId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_HAGUE_STATUS
        });

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");
            var vocabularyNames = new List<NewTermForNewNameable>
                {
                    new NewTermForNewNameable
                    {
                        IdentificationForCreate = new Identification.Possible {
                            Id = null,
                        },
                        VocabularyId = vocabularyId,
                        Name = name,
                        ParentTermIds = new List<int>(),
                    }
                };

            yield return new HagueStatus.ToCreate {
                IdentificationForCreate = new Identification.Possible {
                    Id = null
                },
                NodeDetailsForCreate = new NodeDetails.NodeDetailsForCreate {
                    PublisherId = reader.GetInt32("access_role_id"),
                    CreatedDateTime = reader.GetDateTime("created_date_time"),
                    ChangedDateTime = reader.GetDateTime("changed_date_time"),
                    Title = name,
                    OwnerId = Constants.OWNER_PARTIES,
                    AuthoringStatusId = 1,
                    TenantNodes = new List<TenantNode.ToCreateForNewNode>
                {
                    new TenantNode.ToCreateForNewNode
                    {
                        IdentificationForCreate = new Identification.Possible {
                            Id = null
                        },
                        TenantId = Constants.PPL,
                        PublicationStatusId = reader.GetInt32("node_status_id"),
                        UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                        SubgroupId = null,
                        UrlId = id
                    },
                    new TenantNode.ToCreateForNewNode
                    {
                        IdentificationForCreate = new Identification.Possible {
                            Id = null
                        },
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = id < 33163 ? id : null
                    }
                },
                    NodeTypeId = 8,
                    TermIds = new List<int>(),
                },
                NameableDetailsForCreate = new NameableDetails.NameableDetailsForCreate {
                    Description = "",
                    FileIdTileImage = null,
                    Terms = vocabularyNames,
                },
            };
        }
        await reader.CloseAsync();
    }
}
