namespace PoundPupLegacy.Convert;

internal sealed class CountryAndFirstAndSecondLevelSubdivisionMigrator(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderByOwnerAndNameFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, CreateModel.Term> termReaderByNameFactory,
    INameableCreatorFactory<EventuallyIdentifiableCountryAndFirstAndSecondLevelSubdivision> countryAndFirstAndSecondLevelSubdivisionCreatorFactory
) : CountryMigrator(databaseConnections)
{
    protected override string Name => "countries that are both first and second level subdivisions";

    private async IAsyncEnumerable<NewCountryAndFirstAndSecondLevelSubdivision> GetRegionSubdivisionCountries(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReader,
        IMandatorySingleItemDatabaseReader<TermReaderByNameRequest, CreateModel.Term> termReaderByName
    )
    {
        var vocabularyId = await vocabularyIdReader.ReadAsync(new VocabularyIdReaderByOwnerAndNameRequest {
            OwnerId = Constants.OWNER_GEOGRAPHY,
            Name = "Subdivision type"
        });
        var subdivisionTypeId = (await termReaderByName.ReadAsync(new TermReaderByNameRequest {
            VocabularyId = vocabularyId,
            Name = "Overseas collectivity"
        }))!.NameableId;

        yield return new NewCountryAndFirstAndSecondLevelSubdivision {
            Id = null,
            Title = "Saint Barthélemy",
            Name = "Saint Barthélemy",
            OwnerId = Constants.OWNER_GEOGRAPHY,
            AuthoringStatusId = 1,
            TenantNodes = new List<NewTenantNodeForNewNode>
            {
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.PPL,
                    PublicationStatusId = 1,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = Constants.SAINT_BARTH
                },
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.CPCT,
                    PublicationStatusId = 2,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = Constants.SAINT_BARTH
                }
            },
            NodeTypeId = 16,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            PublisherId = 1,
            Description = "",
            VocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = "Saint Barthélemy",
                    ParentNames = new List<string> { "Caribbean" },
                },
            },
            SecondLevelRegionId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = 3808
            }),
            CountryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = 4018
            }),
            ISO3166_1_Code = "BL",
            ISO3166_2_Code = "FR-BL",
            FileIdFlag = null,
            FileIdTileImage = null,
            HagueStatusId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = 41213
            }),
            ResidencyRequirements = null,
            AgeRequirements = null,
            HealthRequirements = null,
            IncomeRequirements = null,
            MarriageRequirements = null,
            OtherRequirements = null,
            SubdivisionTypeId = subdivisionTypeId,
            NodeTermIds = new List<int>(),
        };
        yield return new NewCountryAndFirstAndSecondLevelSubdivision {
            Id = null,
            Title = "Saint Martin",
            Name = "Saint Martin",
            OwnerId = Constants.OWNER_GEOGRAPHY,
            AuthoringStatusId = 1,
            TenantNodes = new List<NewTenantNodeForNewNode>
                {
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.SAINT_MARTIN
                    },
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.SAINT_MARTIN
                    }
                },
            NodeTypeId = 16,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            PublisherId = 1,
            Description = "",
            VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = Constants.OWNER_SYSTEM,
                        Name = Constants.VOCABULARY_TOPICS,
                        TermName = "Saint Martin",
                        ParentNames = new List<string>{ "Caribbean" },
                    },
                },
            SecondLevelRegionId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = 3809
            }),
            CountryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = 4018
            }),
            ISO3166_1_Code = "MF",
            ISO3166_2_Code = "FR-MF",
            FileIdFlag = null,
            FileIdTileImage = null,
            HagueStatusId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = 41213
            }),
            ResidencyRequirements = null,
            AgeRequirements = null,
            HealthRequirements = null,
            IncomeRequirements = null,
            MarriageRequirements = null,
            OtherRequirements = null,
            SubdivisionTypeId = subdivisionTypeId,
            NodeTermIds = new List<int>(),
        };
        yield return new NewCountryAndFirstAndSecondLevelSubdivision {
            Id = null,
            Title = "French Southern Territories",
            Name = "French Southern Territories",
            OwnerId = Constants.OWNER_GEOGRAPHY,
            AuthoringStatusId = 1,
            TenantNodes = new List<NewTenantNodeForNewNode>
                {
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.FRENCH_SOUTHERN_TERRITORIES
                    },
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.FRENCH_SOUTHERN_TERRITORIES
                    }
                },
            NodeTypeId = 15,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            PublisherId = 1,
            Description = "",
            VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = Constants.OWNER_SYSTEM,
                        Name = Constants.VOCABULARY_TOPICS,
                        TermName = "French Southern Territories",
                        ParentNames = new List<string>{ "Southern Africa" },
                    },
                },
            SecondLevelRegionId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = 3828
            }),
            CountryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = 4018
            }),
            ISO3166_1_Code = "TF",
            ISO3166_2_Code = "FR-TF",
            FileIdFlag = null,
            FileIdTileImage = null,
            HagueStatusId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = 41213
            }),
            ResidencyRequirements = null,
            AgeRequirements = null,
            HealthRequirements = null,
            IncomeRequirements = null,
            MarriageRequirements = null,
            OtherRequirements = null,
            SubdivisionTypeId = subdivisionTypeId,
            NodeTermIds = new List<int>(),
        };
    }

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var vocabularyIdReader = await vocabularyIdReaderByOwnerAndNameFactory.CreateAsync(_postgresConnection);
        await using var termReaderByName = await termReaderByNameFactory.CreateAsync(_postgresConnection);

        await using var countryAndFirstAndSecondLevelSubdivisionCreator = await countryAndFirstAndSecondLevelSubdivisionCreatorFactory.CreateAsync(_postgresConnection);
        await countryAndFirstAndSecondLevelSubdivisionCreator.CreateAsync(GetRegionSubdivisionCountries(
            nodeIdReader,
            vocabularyIdReader,
            termReaderByName
        ));
        await countryAndFirstAndSecondLevelSubdivisionCreator.CreateAsync(ReadCountryAndFirstAndSecondLevelSubdivision(
            nodeIdReader,
            vocabularyIdReader,
            termReaderByName
        ));
    }
    private async IAsyncEnumerable<NewCountryAndFirstAndSecondLevelSubdivision> ReadCountryAndFirstAndSecondLevelSubdivision(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReader,
        IMandatorySingleItemDatabaseReader<TermReaderByNameRequest, CreateModel.Term> termReaderByName
        )
    {

        var vocabularyId = await vocabularyIdReader.ReadAsync(new VocabularyIdReaderByOwnerAndNameRequest {
            OwnerId = Constants.OWNER_GEOGRAPHY,
            Name = "Subdivision type"
        });
        var subdivisionTypeId = (await termReaderByName.ReadAsync(new TermReaderByNameRequest {
            Name = "Overseas collectivity",
            VocabularyId = vocabularyId
        })).NameableId;

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
                upper(cou.field_country_code_value) iso_3166_code,
                ua.dst url_path
            FROM node n 
            LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
            JOIN content_type_country_type cou ON cou.nid = n.nid
            JOIN category_hierarchy ch ON ch.cid = n.nid
            JOIN node n2 ON n2.nid = ch.parent
            WHERE n.`type` = 'country_type'
            AND n2.`type` = 'region_facts'
            AND n.nid IN (
                3935,
                3903,
                3908,
                4044,
                4057,
                3887,
                3879,
                4063,
                3878)
            """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();


        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetInt32("id") == 3879 ? "Réunion" :
                        reader.GetString("title");
            var regionName = reader.GetString("second_level_region_name");
            var vocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = name,
                    ParentNames = new List<string>{ regionName },
                },
            };


            yield return new NewCountryAndFirstAndSecondLevelSubdivision {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = name,
                Name = name,
                OwnerId = Constants.OWNER_GEOGRAPHY,
                AuthoringStatusId = 1,
                TenantNodes = new List<NewTenantNodeForNewNode>
                {
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = reader.GetInt32("node_status_id"),
                        UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    },
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id < 33163 ? id : null
                    }
                },
                NodeTypeId = 16,
                Description = "",
                VocabularyNames = vocabularyNames,
                SecondLevelRegionId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = reader.GetInt32("second_level_region_id")
                }),
                ISO3166_1_Code = id == 3847 ? "NE" :
                                 id == 4010 ? "RS" :
                                 id == 4014 ? "XK" :
                                 reader.GetString("iso_3166_code"),
                ISO3166_2_Code = GetISO3166Code2ForCountry(id),
                CountryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = GetSupervisingCountryId(id)
                }),
                FileIdFlag = null,
                FileIdTileImage = null,
                HagueStatusId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = 41213
                }),
                ResidencyRequirements = null,
                AgeRequirements = null,
                HealthRequirements = null,
                IncomeRequirements = null,
                MarriageRequirements = null,
                OtherRequirements = null,
                SubdivisionTypeId = subdivisionTypeId,
                NodeTermIds = new List<int>(),
            };
        }
        await reader.CloseAsync();
    }
}
