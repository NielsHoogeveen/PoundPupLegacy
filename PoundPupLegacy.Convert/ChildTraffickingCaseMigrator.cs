namespace PoundPupLegacy.Convert;

internal sealed class ChildTraffickingCaseMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
        IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameRequest, int> termIdReaderFactory,
        IEntityCreatorFactory<ChildTraffickingCase.ToCreate> childTraffickingCaseCreatorFactory
    ) : MigratorPPL(databaseConnections)
{
    protected override string Name => "child trafficking cases";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termIdReader = await termIdReaderFactory.CreateAsync(_postgresConnection);
        await using var childTraffickingCaseCreator = await childTraffickingCaseCreatorFactory.CreateAsync(_postgresConnection);
        await childTraffickingCaseCreator.CreateAsync(ReadChildTraffickingCases(nodeIdReader,termIdReader));
    }
    private async IAsyncEnumerable<ChildTraffickingCase.ToCreate> ReadChildTraffickingCases(
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
                    29 node_type_id,
                    cc.nid IS NOT null is_topic,
                    field_description_7_value description,
                    field_discovery_date_0_value `date`,
                    field_number_of_children_value number_of_children_involved,
                    field_country_from_nid country_id_from,
                    ua.dst url_path
                FROM node n
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                JOIN content_type_child_trafficking_case c ON c.nid = n.nid AND c.vid = n.vid
                LEFT JOIN content_type_category_cat cc ON cc.field_related_page_nid = n.nid 
                LEFT JOIN node n2 ON n2.nid = cc.nid AND n2.vid = cc.vid
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
                Name = "child trafficking",
                VocabularyId = vocabularyId
            })
        };

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");

            var vocabularyNames = new List<NewTermForNewNameable> {
                new NewTermForNewNameable {
                    Identification = new Identification.Possible {
                        Id = null,
                    },
                    VocabularyId = vocabularyId,
                    Name = name,
                    ParentTermIds = parentTermIds,
                }
            };

            var country = new ChildTraffickingCase.ToCreate {
                Identification = new Identification.Possible {
                    Id = null
                },
                NodeDetails = new NodeDetails.ForCreate {
                    PublisherId = reader.GetInt32("user_id"),
                    CreatedDateTime = reader.GetDateTime("created"),
                    ChangedDateTime = reader.GetDateTime("changed"),
                    Title = name,
                    OwnerId = Constants.OWNER_CASES,
                    AuthoringStatusId = 1,
                    TenantNodes = new List<TenantNode.ToCreateForNewNode>
                    {
                        new TenantNode.ToCreateForNewNode
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
                        new TenantNode.ToCreateForNewNode
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
                LocatableDetails = new LocatableDetails.LocatableDetailsForCreate {
                    Locations = new List<EventuallyIdentifiableLocation>(),
                },
                CaseDetails = new CaseDetails.CaseDetailsForCreate {
                    Date = reader.IsDBNull("date") ? null : StringToDateTimeRange(reader.GetString("date"))?.ToFuzzyDate(),
                    CaseCaseParties = new List<CaseCaseParties.ToCreate.ForNewCase>(),
                },
                ChildTraffickingCaseDetails = new ChildTraffickingCaseDetails {
                    NumberOfChildrenInvolved = reader.IsDBNull("number_of_children_involved") ? null : reader.GetInt32("number_of_children_involved"),
                    CountryIdFrom = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                        TenantId = Constants.PPL,
                        UrlId = reader.GetInt32("country_id_from")
                    }),
                },
            };
            yield return country;
        }
        await reader.CloseAsync();
    }
}
