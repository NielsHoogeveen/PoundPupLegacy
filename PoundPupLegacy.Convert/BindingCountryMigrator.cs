namespace PoundPupLegacy.Convert;

internal sealed class BindingCountryMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
        IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameRequest, int> termIdReaderFactory,
        IEntityCreatorFactory<BindingCountry.ToCreate> bindingCountryCreatorFactory
    ) : MigratorPPL(databaseConnections)
{
    protected override string Name => "binding countries";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termIdReader = await termIdReaderFactory.CreateAsync(_postgresConnection);
        await using var bindingCountryCreator = await bindingCountryCreatorFactory.CreateAsync(_postgresConnection);
        await bindingCountryCreator.CreateAsync(ReadBindingCountries(nodeIdReader, termIdReader));
    }
    private async IAsyncEnumerable<BindingCountry.ToCreate> ReadBindingCountries(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader)
    {
        var sql = $"""
                SELECT
                    n.nid id,
                    n.uid access_role_id,
                    n.title,
                    n.`status` node_status_id,
                    FROM_UNIXTIME(n.created) created_date_time, 
                    FROM_UNIXTIME(n.changed) changed_date_time, 
                    n2.nid second_level_region_id,
                    n2.title second_level_region_name,
                    upper(cou.field_country_code_value) iso_3166_1_code,
                    ua.dst url_path
                FROM node n 
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                JOIN content_type_country_type cou ON cou.nid = n.nid 
                JOIN category_hierarchy ch ON ch.cid = n.nid 
                JOIN node n2 ON n2.nid = ch.parent 
                WHERE n.`type` = 'country_type' 
                AND n2.`type` = 'region_facts' 
                AND n.nid IN (
                    3992
                )
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

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");
            var regionName = reader.GetString("second_level_region_name");

            var vocabularyNames = new List<NewTermForNewNameable>
                {
                    new NewTermForNewNameable
                    {
                        Identification = new Identification.Possible {
                            Id = null,
                        },
                        VocabularyId = vocabularyId,
                        Name = name,
                        ParentTermIds = new List<int>{
                            await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                                Name = regionName,
                                VocabularyId = vocabularyId
                            })
                        },
                    },
                };

            var country = new BindingCountry.ToCreate {
                Identification = new Identification.Possible {
                    Id = null
                },
                NodeDetails = new NodeDetails.NodeDetailsForCreate {
                    PublisherId = reader.GetInt32("access_role_id"),
                    CreatedDateTime = reader.GetDateTime("created_date_time"),
                    ChangedDateTime = reader.GetDateTime("changed_date_time"),
                    Title = name,
                    OwnerId = Constants.OWNER_GEOGRAPHY,
                    AuthoringStatusId = 1,
                    TenantNodes = new List<TenantNode.ToCreateForNewNode>
                    {
                        new TenantNode.ToCreateForNewNode
                        {
                            Identification = new Identification.Possible {
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
                    NodeTypeId = 20,
                    TermIds = new List<int>(),
                },
                NameableDetails = new NameableDetails.ForCreate {
                    Description = "",
                    Terms = vocabularyNames,
                    FileIdTileImage = null,
                },
                PoliticalEntityDetails = new PoliticalEntityDetails {
                    FileIdFlag = null,
                },
                CountryDetails = new CountryDetails {
                    Name = name,
                    HagueStatusId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                        UrlId = 41215,
                        TenantId = Constants.PPL,
                    }),
                    ResidencyRequirements = null,
                    AgeRequirements = null,
                    HealthRequirements = null,
                    IncomeRequirements = null,
                    MarriageRequirements = null,
                    OtherRequirements = null,
                },
                TopLevelCountryDetails = new TopLevelCountryDetails {
                    SecondLevelRegionId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                        UrlId = reader.GetInt32("second_level_region_id"),
                        TenantId = Constants.PPL,
                    }),
                    ISO3166_1_Code = reader.GetString("iso_3166_1_code"),
                },
            };
            yield return country;
        }
        await reader.CloseAsync();
    }
}
