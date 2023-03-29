namespace PoundPupLegacy.Convert;

internal sealed class CountryAndFirstLevelSubDivisionMigrator : CountryMigrator
{
    protected override string Name => "countries that are first level subdivisions";

    public CountryAndFirstLevelSubDivisionMigrator(MySqlToPostgresConverter converter) : base(converter) { }

    protected override async Task MigrateImpl()
    {
        await CountryAndFirstAndBottomLevelSubdivisionCreator.CreateAsync(GetCountryAndFirstAndBottomLevelSubdivisions(), _postgresConnection);
        await CountryAndFirstAndBottomLevelSubdivisionCreator.CreateAsync(ReadCountryAndFirstAndIntermediateLevelSubdivisions(), _postgresConnection);

    }

    private async IAsyncEnumerable<CountryAndFirstAndBottomLevelSubdivision> GetCountryAndFirstAndBottomLevelSubdivisions()
    {
        await using var vocabularyReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(_postgresConnection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(_postgresConnection);

        var vocabularyId = await vocabularyReader.ReadAsync(new VocabularyIdReaderByOwnerAndName.Request {
            OwnerId = Constants.OWNER_GEOGRAPHY,
            Name = "Subdivision type"
        });

        yield return new CountryAndFirstAndBottomLevelSubdivision {
            Id = null,
            Title = "Åland",
            Name = "Åland",
            OwnerId = Constants.OWNER_GEOGRAPHY,
            TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.ALAND
                    },
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.ALAND
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
                        OwnerId = Constants.PPL,
                        Name = Constants.VOCABULARY_TOPICS,
                        TermName = "Åland",
                        ParentNames = new List<string>{ "Northern Europe" },
                    }
                },
            SecondLevelRegionId = await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                TenantId = Constants.PPL,
                UrlId = 3813
            }),
            CountryId = await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                TenantId = Constants.PPL,
                UrlId = 3985
            }),
            ISO3166_1_Code = "AX",
            ISO3166_2_Code = "FI-01",
            FileIdFlag = null,
            FileIdTileImage = null,
            HagueStatusId = await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                TenantId = Constants.PPL,
                UrlId = 41213
            }),
            ResidencyRequirements = null,
            AgeRequirements = null,
            HealthRequirements = null,
            IncomeRequirements = null,
            MarriageRequirements = null,
            OtherRequirements = null,
            SubdivisionTypeId = (await termReader.ReadAsync(new TermReaderByName.Request {
                VocabularyId = vocabularyId,
                Name = "Autonomous region"
            }))!.NameableId,
        };
        yield return new CountryAndFirstAndBottomLevelSubdivision {
            Id = null,
            Title = "Curaçao",
            Name = "Curaçao",
            OwnerId = Constants.OWNER_GEOGRAPHY,
            TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = 1,
                        UrlPath = "curacao",
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.CURACAO
                    },
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.CURACAO
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
                        OwnerId = Constants.PPL,
                        Name = Constants.VOCABULARY_TOPICS,
                        TermName = "Curaçao",
                        ParentNames = new List<string>{ "Caribbean" },
                    }
                },
            SecondLevelRegionId = await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                TenantId = Constants.PPL,
                UrlId = 3809
            }),
            CountryId = await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                TenantId = Constants.PPL,
                UrlId = 4023
            }),
            ISO3166_1_Code = "CW",
            ISO3166_2_Code = "NL-CW",
            FileIdFlag = null,
            FileIdTileImage = null,
            HagueStatusId = await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                TenantId = Constants.PPL,
                UrlId = 41213
            }),
            ResidencyRequirements = null,
            AgeRequirements = null,
            HealthRequirements = null,
            IncomeRequirements = null,
            MarriageRequirements = null,
            OtherRequirements = null,
            SubdivisionTypeId = (await termReader.ReadAsync(new TermReaderByName.Request {
                VocabularyId = vocabularyId,
                Name = "Country"
            }))!.NameableId,
        };
        yield return new CountryAndFirstAndBottomLevelSubdivision {
            Id = null,
            Title = "Sint Maarten",
            Name = "Sint Maarten",
            OwnerId = Constants.OWNER_GEOGRAPHY,
            TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = 1,
                        UrlPath = "sint_maarten",
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.SINT_MAARTEN
                    },
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.SINT_MAARTEN
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
                        OwnerId = Constants.PPL,
                        Name = Constants.VOCABULARY_TOPICS,
                        TermName = "Sint Maarten",
                        ParentNames = new List<string>{ "Caribbean" },
                    }
                },
            SecondLevelRegionId = await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                TenantId = Constants.PPL,
                UrlId = 3809
            }),
            CountryId = await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                TenantId = Constants.PPL,
                UrlId = 4023
            }),
            ISO3166_1_Code = "SX",
            ISO3166_2_Code = "NL-SX",
            FileIdFlag = null,
            FileIdTileImage = null,
            HagueStatusId = await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                TenantId = Constants.PPL,
                UrlId = 41213
            }),
            ResidencyRequirements = null,
            AgeRequirements = null,
            HealthRequirements = null,
            IncomeRequirements = null,
            MarriageRequirements = null,
            OtherRequirements = null,
            SubdivisionTypeId = (await termReader.ReadAsync(new TermReaderByName.Request {
                VocabularyId = vocabularyId,
                Name = "Country"
            }))!.NameableId,
        };
        yield return new CountryAndFirstAndBottomLevelSubdivision {
            Id = null,
            Title = "United States Minor Outlying Islands",
            Name = "United States Minor Outlying Islands",
            OwnerId = Constants.OWNER_GEOGRAPHY,
            TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.UNITED_STATES_MINOR_OUTLYING_ISLANDS
                    },
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.UNITED_STATES_MINOR_OUTLYING_ISLANDS
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
                        OwnerId = Constants.PPL,
                        Name = Constants.VOCABULARY_TOPICS,
                        TermName = "United States Minor Outlying Islands",
                        ParentNames = new List<string>{ "Oceania" },
                    }
                },
            SecondLevelRegionId = await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                TenantId = Constants.PPL,
                UrlId = 3822
            }),
            CountryId = await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                TenantId = Constants.PPL,
                UrlId = 3805
            }),
            ISO3166_1_Code = "UM",
            ISO3166_2_Code = "US-UM",
            FileIdFlag = null,
            FileIdTileImage = null,
            HagueStatusId = await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                TenantId = Constants.PPL,
                UrlId = 41213
            }),
            ResidencyRequirements = null,
            AgeRequirements = null,
            HealthRequirements = null,
            IncomeRequirements = null,
            MarriageRequirements = null,
            OtherRequirements = null,
            SubdivisionTypeId = (await termReader.ReadAsync(new TermReaderByName.Request {
                VocabularyId = vocabularyId,
                Name = "Outlying area"
            }))!.NameableId,
        };
    }

    private async IAsyncEnumerable<CountryAndFirstAndBottomLevelSubdivision> ReadCountryAndFirstAndIntermediateLevelSubdivisions()
    {
        await using var vocabularyReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(_postgresConnection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(_postgresConnection);

        var vocabularyId = await vocabularyReader.ReadAsync(new VocabularyIdReaderByOwnerAndName.Request {
            OwnerId = Constants.OWNER_GEOGRAPHY,
            Name = "Subdivision type"

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
        using var readCommand = MysqlConnection.CreateCommand();
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
                    OwnerId = Constants.PPL,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = name,
                    ParentNames = new List<string>{ regionName },
                }
            };


            yield return new CountryAndFirstAndBottomLevelSubdivision {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = name,
                Name = name,
                OwnerId = Constants.OWNER_GEOGRAPHY,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = reader.GetInt32("node_status_id"),
                        UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    },
                    new TenantNode
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
                NodeTypeId = 15,
                Description = "",
                VocabularyNames = vocabularyNames,
                SecondLevelRegionId = await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                    TenantId = Constants.PPL,
                    UrlId = reader.GetInt32("second_level_region_id")
                }),
                ISO3166_1_Code = reader.GetInt32("id") == 3847 ? "NE" :
                                reader.GetInt32("id") == 4010 ? "RS" :
                                reader.GetInt32("id") == 4014 ? "XK" :
                                reader.GetString("iso_3166_code"),
                ISO3166_2_Code = GetISO3166Code2ForCountry(reader.GetInt32("id")),
                CountryId = await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                    TenantId = Constants.PPL,
                    UrlId = GetSupervisingCountryId(reader.GetInt32("id"))
                }),
                FileIdFlag = null,
                FileIdTileImage = null,
                HagueStatusId = await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                    TenantId = Constants.PPL,
                    UrlId = 41213
                }),
                ResidencyRequirements = null,
                AgeRequirements = null,
                HealthRequirements = null,
                IncomeRequirements = null,
                MarriageRequirements = null,
                OtherRequirements = null,
                SubdivisionTypeId = (await termReader.ReadAsync(new TermReaderByName.Request {
                    VocabularyId = vocabularyId,
                    Name = reader.GetString("subdivision_type_name")
                }))!.NameableId,
            };
        }
        await reader.CloseAsync();
    }
}

