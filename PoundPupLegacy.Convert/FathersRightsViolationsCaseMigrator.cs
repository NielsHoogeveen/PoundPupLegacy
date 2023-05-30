namespace PoundPupLegacy.Convert;

internal sealed class FathersRightsViolationsCaseMigrator(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameRequest, int> termIdReaderFactory,
    IEntityCreatorFactory<FathersRightsViolationCase.ToCreate> fathersRightsViolationCaseCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "father's rights violation cases";

    protected override async Task MigrateImpl()
    {
        await using var fathersRightsViolationCaseCreator = await fathersRightsViolationCaseCreatorFactory.CreateAsync(_postgresConnection);
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termIdReader = await termIdReaderFactory.CreateAsync(_postgresConnection);
        await fathersRightsViolationCaseCreator.CreateAsync(ReadFathersRightsViolationCases(nodeIdReader,termIdReader));
    }
    private async IAsyncEnumerable<FathersRightsViolationCase.ToCreate> ReadFathersRightsViolationCases(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader)
    {

        var sql = $"""
                SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`,
                    32 node_type_id,
                    field_long_description_5_value description,
                    field_reporting_date_1_value `date`,
                    ua.dst url_path
                FROM node n
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                JOIN content_type_fathers_rights_violations c ON c.nid = n.nid AND c.vid = n.vid
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

        var parentTermIds = new List<int> { 
            await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                Name = "Father's rights violations",
                VocabularyId = vocabularyId
            })
        };
        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");
            var vocabularyNames = new List<NewTermForNewNameable> {
                new NewTermForNewNameable {
                    IdentificationForCreate = new Identification.Possible {
                        Id = null,
                    },
                    VocabularyId = vocabularyId,
                    Name = name,
                    ParentTermIds = parentTermIds,
                }
            };

            var country = new FathersRightsViolationCase.ToCreate {
                IdentificationForCreate = new Identification.Possible {
                    Id = null
                },
                NodeDetailsForCreate = new NodeDetails.NodeDetailsForCreate {
                    PublisherId = reader.GetInt32("user_id"),
                    CreatedDateTime = reader.GetDateTime("created"),
                    ChangedDateTime = reader.GetDateTime("changed"),
                    Title = reader.GetString("title"),
                    OwnerId = Constants.OWNER_CASES,
                    AuthoringStatusId = 1,
                    TenantNodes = new List<TenantNode.ToCreateForNewNode>
                    {
                        new TenantNode.ToCreateForNewNode
                        {
                            IdentificationForCreate = new Identification.Possible {
                                Id = null
                            },
                            TenantId = Constants.PPL,
                            PublicationStatusId = reader.GetInt32("status"),
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
                    NodeTypeId = reader.GetInt32("node_type_id"),
                    TermIds = new List<int>(),
                },
                NameableDetailsForCreate = new NameableDetails.NameableDetailsForCreate {
                    Terms = vocabularyNames,
                    Description = reader.GetString("description"),
                    FileIdTileImage = null,
                },
                LocatableDetailsForCreate = new LocatableDetails.LocatableDetailsForCreate {
                    Locations = new List<EventuallyIdentifiableLocation>(),
                },
                CaseDetailsForCreate = new CaseDetails.CaseDetailsForCreate {
                    Date = reader.IsDBNull("date") ? null : StringToDateTimeRange(reader.GetString("date"))?.ToFuzzyDate(),
                    CaseParties = new List<NewCaseNewCaseParties>(),
                },
            };
            yield return country;

        }
        await reader.CloseAsync();
    }
}
