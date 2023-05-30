using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.Convert;

internal sealed class BasicSecondLevelSubdivisionMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
        IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameRequest, int> termIdReaderFactory,
        IMandatorySingleItemDatabaseReaderFactory<SubdivisionIdReaderByNameRequest, int> subdivisionIdByNameReaderFactory,
        IMandatorySingleItemDatabaseReaderFactory<TermNameReaderByNameableIdRequest, string> termReaderByNameableIdFactory,
        IMandatorySingleItemDatabaseReaderFactory<NameableIdReaderByTermNameRequest, int> termReaderByNameFactory,
        IMandatorySingleItemDatabaseReaderFactory<SubdivisionIdReaderByIso3166CodeRequest, int> subdivisionIdReaderByIso3166CodeFactory,
        IEntityCreatorFactory<BasicSecondLevelSubdivision.ToCreate> basicSecondLevelSubdivisionCreatorFactory
    ) : MigratorPPL(databaseConnections)
{
    protected override string Name => "basic second level subdivisions";
    private async IAsyncEnumerable<BasicSecondLevelSubdivision.ToCreate> ReadBasicSecondLevelSubdivisionsInInformalPrimarySubdivisionCsv(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader,
        IMandatorySingleItemDatabaseReader<SubdivisionIdReaderByNameRequest, int> subdivisionIdByNameReader,
        IMandatorySingleItemDatabaseReader<TermNameReaderByNameableIdRequest, string> termReaderByNameableId,
        IMandatorySingleItemDatabaseReader<NameableIdReaderByTermNameRequest, int> termReaderByName
        )
    {
        var vocabularyIdSubdivisionTypes = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_SUBDIVISION_TYPE
        });
        var vocabularyIdTopics = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_TOPICS
        });
        await foreach (string line in System.IO.File.ReadLinesAsync(@"..\..\..\files\BasicSecondLevelSubdivisionsInInformalPrimarySubdivision.csv").Skip(1)) {
            var parts = line.Split(new char[] { ';' }).Select(x => x.TrimStart()).ToList();
            int? id = int.Parse(parts[0]) == 0 ? null : int.Parse(parts[0]);
            var title = parts[8];
            var countryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = int.Parse(parts[7])
            });
            var subdivisionId = await subdivisionIdByNameReader.ReadAsync(new SubdivisionIdReaderByNameRequest {
                CountryId = countryId,
                Name = parts[11]
            });
            var topicName = (await termReaderByNameableId.ReadAsync(new TermNameReaderByNameableIdRequest {
                NameableId = subdivisionId,
                VocabularyId = vocabularyIdTopics
            }));
            yield return new BasicSecondLevelSubdivision.ToCreate {
                Identification = new Identification.Possible {
                    Id = null
                },
                NodeDetails = new NodeDetails.NodeDetailsForCreate {
                    Title = title,
                    CreatedDateTime = DateTime.Parse(parts[1]),
                    ChangedDateTime = DateTime.Parse(parts[2]),
                    NodeTypeId = int.Parse(parts[4]),
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
                            PublicationStatusId = int.Parse(parts[5]),
                            UrlPath = null,
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
                    PublisherId = int.Parse(parts[6]),
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
                            Name = title,
                            ParentTermIds = new List<int> {
                                await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                                    Name = topicName ,
                                    VocabularyId = vocabularyIdTopics
                                })
                            },
                        }
                    },
                    FileIdTileImage = null,
                },
                PoliticalEntityDetails = new PoliticalEntityDetails {
                    FileIdFlag = null,
                },
                SubdivisionDetails = new SubdivisionDetails {
                    CountryId = countryId,
                    Name = parts[9],
                    SubdivisionTypeId = (await termReaderByName.ReadAsync(new NameableIdReaderByTermNameRequest {
                        VocabularyId = vocabularyIdSubdivisionTypes,
                        Name = parts[12].Trim()
                    })),
                },
                ISOCodedSubdivisionDetails = new ISOCodedSubdivisionDetails {
                    ISO3166_2_Code = parts[10],
                },
                IntermediateLevelSubdivisionId = subdivisionId,
            };
        }
    }

    private async IAsyncEnumerable<BasicSecondLevelSubdivision.ToCreate> ReadBasicSecondLevelSubdivisionCsv(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader,
        IMandatorySingleItemDatabaseReader<SubdivisionIdReaderByIso3166CodeRequest, int> subdivisionIdReaderByIso3166Code,
        IMandatorySingleItemDatabaseReader<TermNameReaderByNameableIdRequest, string> termReaderByNameableId,
        IMandatorySingleItemDatabaseReader<NameableIdReaderByTermNameRequest, int> termReaderByName
        )
    {
        var vocabularyIdSubdivisionTypes = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_SUBDIVISION_TYPE
        });
        var vocabularyIdTopics = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_TOPICS
        });
        await foreach (string line in System.IO.File.ReadLinesAsync(@"..\..\..\files\BasicSecondLevelSubdivisions.csv").Skip(1)) {

            var parts = line.Split(new char[] { ';' }).Select(x => x.TrimStart()).ToList();
            int? id = int.Parse(parts[0]) == 0 ? null : int.Parse(parts[0]);
            var title = parts[8];
            var subdivisionId = await subdivisionIdReaderByIso3166Code.ReadAsync(new SubdivisionIdReaderByIso3166CodeRequest {
                Iso3166Code = parts[11]
            });
            var topicName = (await termReaderByNameableId.ReadAsync(new TermNameReaderByNameableIdRequest {
                NameableId = subdivisionId,
                VocabularyId = vocabularyIdTopics
            }));
            yield return new BasicSecondLevelSubdivision.ToCreate {
                Identification = new Identification.Possible {
                    Id = null
                },
                NodeDetails = new NodeDetails.NodeDetailsForCreate {
                    Title = title,
                    CreatedDateTime = DateTime.Parse(parts[1]),
                    ChangedDateTime = DateTime.Parse(parts[2]),
                    NodeTypeId = int.Parse(parts[4]),
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
                            PublicationStatusId = int.Parse(parts[5]),
                            UrlPath = null,
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
                    PublisherId = int.Parse(parts[6]),
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
                            Name = title,
                            ParentTermIds = new List<int> {
                                await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                                    Name = topicName,
                                    VocabularyId = vocabularyIdTopics
                                })
                            },
                        }
                    },
                    FileIdTileImage = null,
                },
                PoliticalEntityDetails = new PoliticalEntityDetails {
                    FileIdFlag = null,
                },
                SubdivisionDetails = new SubdivisionDetails {
                    CountryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest() {
                        TenantId = Constants.PPL,
                        UrlId = int.Parse(parts[7])
                    }),
                    Name = parts[9],
                    SubdivisionTypeId = (await termReaderByName.ReadAsync(new NameableIdReaderByTermNameRequest {
                        VocabularyId = vocabularyIdSubdivisionTypes,
                        Name = parts[12].Trim()
                    })),

                },
                ISOCodedSubdivisionDetails = new ISOCodedSubdivisionDetails {
                    ISO3166_2_Code = parts[10],
                },
                IntermediateLevelSubdivisionId = subdivisionId,
            };
        }
    }
    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termIdReader = await termIdReaderFactory.CreateAsync(_postgresConnection);
        await using var subdivisionIdByNameReader = await subdivisionIdByNameReaderFactory.CreateAsync(_postgresConnection);
        await using var subdivisionIdReaderByIso3166Code = await subdivisionIdReaderByIso3166CodeFactory.CreateAsync(_postgresConnection);
        await using var termReaderByNameableId = await termReaderByNameableIdFactory.CreateAsync(_postgresConnection);
        await using var termReaderByName = await termReaderByNameFactory.CreateAsync(_postgresConnection);

        await using var basicSecondLevelSubdivisionCreator = await basicSecondLevelSubdivisionCreatorFactory.CreateAsync(_postgresConnection);
        await basicSecondLevelSubdivisionCreator.CreateAsync(ReadBasicSecondLevelSubdivisionsInInformalPrimarySubdivisionCsv(
            nodeIdReader,
            termIdReader,
            subdivisionIdByNameReader,
            termReaderByNameableId,
            termReaderByName
        ));
        await basicSecondLevelSubdivisionCreator.CreateAsync(ReadBasicSecondLevelSubdivisionCsv(
            nodeIdReader,
            termIdReader,
            subdivisionIdReaderByIso3166Code,
            termReaderByNameableId,
            termReaderByName
        ));
        await basicSecondLevelSubdivisionCreator.CreateAsync(ReadBasicSecondLevelSubdivisions(
            nodeIdReader,
            termIdReader,
            termReaderByName
        ));

    }
    private async IAsyncEnumerable<BasicSecondLevelSubdivision.ToCreate> ReadBasicSecondLevelSubdivisions(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader,
        IMandatorySingleItemDatabaseReader<NameableIdReaderByTermNameRequest, int> termReaderByName
        )
    {
        var vocabularyIdSubdivisionTypes = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_SUBDIVISION_TYPE
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
                FROM_UNIXTIME(n.created) created_date_time, 
                FROM_UNIXTIME(n.changed) changed_date_time,
                n2.nid intermediate_level_subdivision_id,
                n2.title subdivision_name,
                3805 country_id,
                CONCAT('US-', s.field_statecode_value) 
                iso_3166_2_code,
                s.field_state_flag_fid file_id_flag,
                ua.dst url_path
            FROM node n 
            LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
            JOIN content_type_statefact s ON s.nid = n.nid
            JOIN category_hierarchy ch ON ch.cid = n.nid
            JOIN node n2 ON n2.nid = ch.parent
            WHERE n.`type` = 'statefact'
            AND n2.`type` = 'region_facts'
            AND s.field_statecode_value IS NOT NULL
            ORDER BY s.field_statecode_value
            """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var title = $"{reader.GetString("title").Replace(" (state)", "")} (state of the USA)";
            var subdivisioName = $"{reader.GetString("subdivision_name")} (region of the USA)";
            var vocabularyNames = new List<NewTermForNewNameable>
            {
                new NewTermForNewNameable
                {
                    Identification = new Identification.Possible {
                        Id = null,
                    },
                    VocabularyId = vocabularyIdTopics,
                    Name = title,
                    ParentTermIds = new List<int>{
                        await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                            Name = subdivisioName,
                            VocabularyId = vocabularyIdTopics
                        })
                    },
                }
            };
            yield return new BasicSecondLevelSubdivision.ToCreate {
                Identification = new Identification.Possible {
                    Id = null
                },
                NodeDetails = new NodeDetails.NodeDetailsForCreate {
                    PublisherId = reader.GetInt32("access_role_id"),
                    CreatedDateTime = reader.GetDateTime("created_date_time"),
                    ChangedDateTime = reader.GetDateTime("changed_date_time"),
                    Title = title,
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
                    NodeTypeId = 19,
                    TermIds = new List<int>(),
                },
                NameableDetails = new NameableDetails.ForCreate {
                    Description = "",
                    Terms = vocabularyNames,
                    FileIdTileImage = null,
                },
                PoliticalEntityDetails = new PoliticalEntityDetails {
                    FileIdFlag = reader.IsDBNull("file_id_flag") ? null : reader.GetInt32("file_id_flag"),
                },
                SubdivisionDetails = new SubdivisionDetails {
                    Name = reader.GetString("title"),
                    CountryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                        TenantId = Constants.PPL,
                        UrlId = reader.GetInt32("country_id")
                    }),
                    SubdivisionTypeId = (await termReaderByName.ReadAsync(new NameableIdReaderByTermNameRequest {
                        VocabularyId = vocabularyIdSubdivisionTypes,
                        Name = "State"
                    })),
                },
                ISOCodedSubdivisionDetails = new ISOCodedSubdivisionDetails {
                    ISO3166_2_Code = reader.GetString("iso_3166_2_code"),
                },
                IntermediateLevelSubdivisionId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = reader.GetInt32("intermediate_level_subdivision_id")
                }),
            };
        }
        await reader.CloseAsync();
    }

}
