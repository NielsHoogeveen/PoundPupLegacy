namespace PoundPupLegacy.Convert;

internal sealed class WrongfulRemovalCaseMigrator(
    IDatabaseConnections databaseConnections,
    IEntityCreatorFactory<WrongfulRemovalCase.ToCreate> wrongfulRemovalCaseCreatorFactory,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameRequest, int> termIdReaderFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "wrongful removal case";

    protected override async Task MigrateImpl()
    {
        await using var wrongfulRemovalCaseCreator = await wrongfulRemovalCaseCreatorFactory.CreateAsync(_postgresConnection);
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termIdReader = await termIdReaderFactory.CreateAsync(_postgresConnection);
        await wrongfulRemovalCaseCreator.CreateAsync(ReadWrongfulRemovalCases(nodeIdReader, termIdReader));
    }
    private async IAsyncEnumerable<WrongfulRemovalCase.ToCreate> ReadWrongfulRemovalCases(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader
    )
    {
        var sql = $"""
                SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`,
                    34 node_type_id,
                    field_long_description_0_value description,
                    field_removal_date_value `date`,
                    ua.dst url_path
                FROM node n
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                JOIN content_type_wrongful_removal_case c ON c.nid = n.nid AND c.vid = n.vid
                """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        var vocabularyId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_TOPICS
        });

        var parentTermIds = new List<int>{
            await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
            Name = "wrongful removal",
            VocabularyId = vocabularyId
        })};
        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var title = reader.GetString("title");
            var vocabularyNames = new List<Term.ToCreateForNewNameable> {
                new Term.ToCreateForNewNameable {
                    Identification = new Identification.Possible {
                        Id = null,
                    },
                    VocabularyId = vocabularyId,
                    Name = title,
                    ParentTermIds = parentTermIds,
                }
            };

            var country = new WrongfulRemovalCase.ToCreate {
                Identification = new Identification.Possible {
                    Id = null
                },
                NodeDetails = new NodeDetails.ForCreate {
                    PublisherId = reader.GetInt32("user_id"),
                    CreatedDateTime = reader.GetDateTime("created"),
                    ChangedDateTime = reader.GetDateTime("changed"),
                    Title = reader.GetString("title"),
                    OwnerId = Constants.OWNER_CASES,
                    AuthoringStatusId = 1,
                    TenantNodes = new List<TenantNode.ToCreate.ForNewNode>
                    {
                        new TenantNode.ToCreate.ForNewNode
                        {
                            Identification = new Identification.Possible {
                                Id = null
                            },
                            TenantId = Constants.PPL,
                            PublicationStatusId = reader.GetInt32("status"),
                            UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                            SubgroupId = null,
                            UrlId = id
                        },
                        new TenantNode.ToCreate.ForNewNode
                        {
                            Identification = new Identification.Possible {
                                Id = null
                            },
                            TenantId = Constants.CPCT,
                            PublicationStatusId = 2,
                            UrlPath = null,
                            SubgroupId = null,
                            UrlId = id < 33163 ? id : null
                        }
                    },
                    NodeTypeId = reader.GetInt32("node_type_id"),
                    TermIds = new List<int>(),
                },
                NameableDetails = new NameableDetails.ForCreate {
                    Terms = vocabularyNames,
                    Description = reader.GetString("description"),
                    FileIdTileImage = null,
                },
                LocatableDetails = new LocatableDetails.ForCreate { 
                    Locations = new List<Location.ToCreate>(),
                },
                CaseDetails = new CaseDetails.CaseDetailsForCreate {
                    Date = reader.IsDBNull("date") ? null : StringToDateTimeRange(reader.GetString("date"))?.ToFuzzyDate(),
                    CaseCaseParties = new List<CaseCaseParties.ToCreate.ForNewCase>(),
                },
            };
            yield return country;

        }
        await reader.CloseAsync();
    }
}
