﻿using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.Convert;

internal sealed class CountryAndFirstLevelSubDivisionMigrator(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameRequest, int> termIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<NameableIdReaderByTermNameRequest, int> termReaderByNameFactory,
    IEntityCreatorFactory<CountryAndFirstAndBottomLevelSubdivision.ToCreate> countryAndFirstAndBottomLevelSubdivisionCreatorFactory
) : CountryMigrator(databaseConnections)
{
    protected override string Name => "countries that are first level subdivisions";
    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termIdReader = await termIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termReader = await termReaderByNameFactory.CreateAsync(_postgresConnection);
        await using var countryAndFirstAndBottomLevelSubdivisionCreator = await countryAndFirstAndBottomLevelSubdivisionCreatorFactory.CreateAsync(_postgresConnection);
        await countryAndFirstAndBottomLevelSubdivisionCreator.CreateAsync(GetCountryAndFirstAndBottomLevelSubdivisions(
            nodeIdReader,
            termIdReader,
            termReader
        ));
        await countryAndFirstAndBottomLevelSubdivisionCreator.CreateAsync(ReadCountryAndFirstAndIntermediateLevelSubdivisions(
            nodeIdReader,
            termIdReader,
            termReader
        ));
    }

    private async IAsyncEnumerable<CountryAndFirstAndBottomLevelSubdivision.ToCreate> GetCountryAndFirstAndBottomLevelSubdivisions(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader,
        IMandatorySingleItemDatabaseReader<NameableIdReaderByTermNameRequest, int> termReader
        )
    {

        var vocabularyIdSubdivisionTypes = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_SUBDIVISION_TYPE,
        });
        var vocabularyIdTopics = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_TOPICS
        });

        yield return new CountryAndFirstAndBottomLevelSubdivision.ToCreate {
            Identification = new Identification.Possible {
                Id = null
            },
            NodeDetails = new NodeDetails.ForCreate {
                Title = "Åland",
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
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.ALAND
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
                        UrlId = Constants.ALAND
                    }
                },
                NodeTypeId = 15,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                PublisherId = 1,
                TermIds = new List<int>(),
            },
            NameableDetails = new NameableDetails.ForCreate {
                Description = "",
                Terms = new List<NewTermForNewNameable>
                {
                    new NewTermForNewNameable
                    {
                        Identification = new Identification.Possible {
                            Id = null,
                        },
                        VocabularyId = vocabularyIdTopics,
                        Name = "Åland",
                        ParentTermIds = new List<int>{
                            await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                                Name = "Northern Europe" ,
                                VocabularyId = vocabularyIdTopics
                            })
                        },
                    },
                },
                FileIdTileImage = null,
            },
            PoliticalEntityDetails = new PoliticalEntityDetails {
                FileIdFlag = null,
            },
            CountryDetails = new CountryDetails {
                Name = "Åland",
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
            },
            SubdivisionDetails = new SubdivisionDetails {
                Name = "Åland",
                CountryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = 3985
                }),
                SubdivisionTypeId = (await termReader.ReadAsync(new NameableIdReaderByTermNameRequest {
                    VocabularyId = vocabularyIdSubdivisionTypes,
                    Name = "Autonomous region"
                })),
            },
            TopLevelCountryDetails = new TopLevelCountryDetails {
                SecondLevelRegionId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = 3813
                }),
                ISO3166_1_Code = "AX",
            },
            ISOCodedSubdivisionDetails = new ISOCodedSubdivisionDetails {
                ISO3166_2_Code = "FI-01",
            },
        };
        yield return new CountryAndFirstAndBottomLevelSubdivision.ToCreate {
            Identification = new Identification.Possible {
                Id = null
            },
            NodeDetails = new NodeDetails.ForCreate {
                Title = "Curaçao",
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
                        PublicationStatusId = 1,
                        UrlPath = "curacao",
                        SubgroupId = null,
                        UrlId = Constants.CURACAO
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
                        UrlId = Constants.CURACAO
                    }
                },
                NodeTypeId = 15,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                PublisherId = 1,
                TermIds = new List<int>(),
            },
            NameableDetails = new NameableDetails.ForCreate {
                Description = "",
                Terms = new List<NewTermForNewNameable>
                {
                    new NewTermForNewNameable
                    {
                        Identification = new Identification.Possible {
                            Id = null,
                        },
                        VocabularyId = vocabularyIdTopics,
                        Name = "Curaçao",
                        ParentTermIds = new List<int>{
                            await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                                Name = "Caribbean",
                                VocabularyId = vocabularyIdTopics
                            })
                        },
                    },
                },
                FileIdTileImage = null,
            },
            PoliticalEntityDetails = new PoliticalEntityDetails {
                FileIdFlag = null,
            },
            CountryDetails = new CountryDetails {
                Name = "Curaçao",
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
            },
            SubdivisionDetails = new SubdivisionDetails {
                Name = "Curaçao",
                CountryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = 4023
                }),
                SubdivisionTypeId = (await termReader.ReadAsync(new NameableIdReaderByTermNameRequest {
                    VocabularyId = vocabularyIdSubdivisionTypes,
                    Name = "Country"
                })),
            },
            TopLevelCountryDetails = new TopLevelCountryDetails {
                ISO3166_1_Code = "CW",
                SecondLevelRegionId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = 3809
                }),
            },
            ISOCodedSubdivisionDetails = new ISOCodedSubdivisionDetails {
                ISO3166_2_Code = "NL-CW",
            },
        };
        yield return new CountryAndFirstAndBottomLevelSubdivision.ToCreate {
            Identification = new Identification.Possible {
                Id = null
            },
            NodeDetails = new NodeDetails.ForCreate {
                Title = "Sint Maarten",
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
                        PublicationStatusId = 1,
                        UrlPath = "sint_maarten",
                        SubgroupId = null,
                        UrlId = Constants.SINT_MAARTEN
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
                        UrlId = Constants.SINT_MAARTEN
                    }
                },
                NodeTypeId = 15,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                PublisherId = 1,
                TermIds = new List<int>(),
            },
            NameableDetails = new NameableDetails.ForCreate {
                Description = "",
                Terms = new List<NewTermForNewNameable>
                {
                    new NewTermForNewNameable
                    {
                        Identification = new Identification.Possible {
                            Id = null,
                        },
                        VocabularyId = vocabularyIdTopics,
                        Name = "Sint Maarten",
                        ParentTermIds = new List<int>{
                            await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                                Name = "Caribbean",
                                VocabularyId = vocabularyIdTopics
                            })
                        },
                    },
                },
                FileIdTileImage = null,
            },
            PoliticalEntityDetails = new PoliticalEntityDetails {
                FileIdFlag = null,
            },
            CountryDetails = new CountryDetails {
                Name = "Sint Maarten",
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
            },
            SubdivisionDetails = new SubdivisionDetails {
                Name = "Sint Maarten",
                CountryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = 4023
                }),
                SubdivisionTypeId = (await termReader.ReadAsync(new NameableIdReaderByTermNameRequest {
                    VocabularyId = vocabularyIdSubdivisionTypes,
                    Name = "Country"
                })),
            },
            TopLevelCountryDetails = new TopLevelCountryDetails {
                SecondLevelRegionId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = 3809
                }),
                ISO3166_1_Code = "SX",
            },
            ISOCodedSubdivisionDetails = new ISOCodedSubdivisionDetails {
                ISO3166_2_Code = "NL-SX",
            },
        };
        yield return new CountryAndFirstAndBottomLevelSubdivision.ToCreate {
            Identification = new Identification.Possible {
                Id = null
            },
            NodeDetails = new NodeDetails.ForCreate {
                Title = "United States Minor Outlying Islands",
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
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.UNITED_STATES_MINOR_OUTLYING_ISLANDS
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
                        UrlId = Constants.UNITED_STATES_MINOR_OUTLYING_ISLANDS
                    }
                },
                NodeTypeId = 15,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                PublisherId = 1,
                TermIds = new List<int>(),
            },
            NameableDetails = new NameableDetails.ForCreate {
                Description = "",
                Terms = new List<NewTermForNewNameable>
                {
                    new NewTermForNewNameable
                    {
                        Identification = new Identification.Possible {
                            Id = null,
                        },
                        VocabularyId = vocabularyIdTopics,
                        Name = "United States Minor Outlying Islands",
                        ParentTermIds = new List<int>{
                            await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                                Name = "Oceania" ,
                                VocabularyId = vocabularyIdTopics
                            })
                        },
                    },
                },
                FileIdTileImage = null,
            },
            PoliticalEntityDetails = new PoliticalEntityDetails {
                FileIdFlag = null,
            },
            CountryDetails =new CountryDetails {
                Name = "United States Minor Outlying Islands",
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
            },
            SubdivisionDetails = new SubdivisionDetails {
                Name = "United States Minor Outlying Islands",
                CountryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = 3805
                }),
                SubdivisionTypeId = (await termReader.ReadAsync(new NameableIdReaderByTermNameRequest {
                    VocabularyId = vocabularyIdSubdivisionTypes,
                    Name = "Outlying area"
                })),
            },
            TopLevelCountryDetails = new TopLevelCountryDetails {
                SecondLevelRegionId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = 3822
                }),
                ISO3166_1_Code = "UM",
            },
            ISOCodedSubdivisionDetails = new ISOCodedSubdivisionDetails {
                ISO3166_2_Code = "US-UM",
            }
        };
    }

    private async IAsyncEnumerable<CountryAndFirstAndBottomLevelSubdivision.ToCreate> ReadCountryAndFirstAndIntermediateLevelSubdivisions(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader,
        IMandatorySingleItemDatabaseReader<NameableIdReaderByTermNameRequest, int> termReader
    )
    {

        var vocabularyIdSubdivisionTypes = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_SUBDIVISION_TYPE,
        });
        var vocabularyIdTopics = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_TOPICS
        });


        var sql = $"""
        SELECT
            n.nid id,
            n.uid access_role_id,
            n.title,
            n.`status` node_status_id,
            case 
            	when n.nid = 3891 then 'Country'
            	when n.nid = 3914 then 'Outlying area'
            	when n.nid = 3920 then 'Outlying area'
            	when n.nid = 4048 then 'Outlying area'
                when n.nid = 4053 then 'Outlying area'
                when n.nid = 4055 then 'Outlying area'
                when n.nid = 3980 then 'Special administrative region'
                when n.nid = 3981 then 'Special administrative region'
            END subdivision_type_name,
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
            3891,
            3914,
            3920,
            3980,
            3981,
            4048,
            4053,
            4055
        )
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
            var vocabularyNames = new List<NewTermForNewNameable>
            {
                new NewTermForNewNameable
                {
                    Identification = new Identification.Possible {
                        Id = null,
                    },
                    VocabularyId = vocabularyIdTopics,
                    Name = name,
                    ParentTermIds = new List<int>{
                        await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                            Name = regionName,
                            VocabularyId = vocabularyIdTopics
                        })
                    },
                },
            };
            yield return new CountryAndFirstAndBottomLevelSubdivision.ToCreate {
                Identification = new Identification.Possible {
                    Id = null
                },
                NodeDetails = new NodeDetails.ForCreate {
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
                    NodeTypeId = 15,
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
                        TenantId = Constants.PPL,
                        UrlId = 41213
                    }),
                    ResidencyRequirements = null,
                    AgeRequirements = null,
                    HealthRequirements = null,
                    IncomeRequirements = null,
                    MarriageRequirements = null,
                    OtherRequirements = null,
                },
                SubdivisionDetails = new SubdivisionDetails {
                    Name = name,
                    CountryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                        TenantId = Constants.PPL,
                        UrlId = GetSupervisingCountryId(reader.GetInt32("id"))
                    }),
                    SubdivisionTypeId = (await termReader.ReadAsync(new NameableIdReaderByTermNameRequest {
                        VocabularyId = vocabularyIdSubdivisionTypes,
                        Name = reader.GetString("subdivision_type_name")
                    })),
                },
                TopLevelCountryDetails = new TopLevelCountryDetails {
                    SecondLevelRegionId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                        TenantId = Constants.PPL,
                        UrlId = reader.GetInt32("second_level_region_id")
                    }),
                    ISO3166_1_Code = reader.GetInt32("id") == 3847 ? "NE" :
                                reader.GetInt32("id") == 4010 ? "RS" :
                                reader.GetInt32("id") == 4014 ? "XK" :
                                reader.GetString("iso_3166_code"),
                },
                ISOCodedSubdivisionDetails = new ISOCodedSubdivisionDetails {
                    ISO3166_2_Code = GetISO3166Code2ForCountry(reader.GetInt32("id")),
                }
            };
        }
        await reader.CloseAsync();
    }
}

