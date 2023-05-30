using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.Convert;

internal sealed class InformalIntermediateLevelSubdivisionMigrator(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameRequest, int> termIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderByOwnerAndNameFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermNameReaderByNameableIdRequest, string> termReaderByNameableIdFactory,
    IMandatorySingleItemDatabaseReaderFactory<NameableIdReaderByTermNameRequest, int> termReaderByNameFactory,
    IEntityCreatorFactory<InformalIntermediateLevelSubdivision.ToCreate> informalIntermediateLevelSubdivisionCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "informal intermediate level subdivisions";

    private async IAsyncEnumerable<InformalIntermediateLevelSubdivision.ToCreate> ReadInformalIntermediateLevelSubdivisionCsv(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader,
        IMandatorySingleItemDatabaseReader<TermNameReaderByNameableIdRequest, string> termReaderByNameableId,
        IMandatorySingleItemDatabaseReader<NameableIdReaderByTermNameRequest, int> termReaderByName
        )
    {
        var vocabularyIdSubdivisionType = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_SUBDIVISION_TYPE
        });
        var vocabularyIdTopics = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_TOPICS
        });

        await foreach (string line in System.IO.File.ReadLinesAsync(@"..\..\..\files\InformalIntermediateLevelSubdivisions.csv").Skip(1)) {

            var parts = line.Split(new char[] { ';' }).Select(x => x.TrimStart()).ToList();
            int? id = int.Parse(parts[0]) == 0 ? null : int.Parse(parts[0]);
            var title = parts[8];
            var countryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = int.Parse(parts[7])
            });
            var countryName = (await termReaderByNameableId.ReadAsync(new TermNameReaderByNameableIdRequest {
                NameableId = countryId,
                VocabularyId = vocabularyIdTopics
            }));
            yield return new InformalIntermediateLevelSubdivision.ToCreate {
                Identification = new Identification.Possible {
                    Id = null
                },
                NodeDetails = new NodeDetails.NodeDetailsForCreate {
                    CreatedDateTime = DateTime.Parse(parts[1]),
                    ChangedDateTime = DateTime.Parse(parts[2]),
                    Title = title,
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
                                    Name = countryName ,
                                    VocabularyId = vocabularyIdTopics
                                })
                            },
                        }
                    },
                    Description = "",
                    FileIdTileImage = null,
                },
                SubdivisionDetails = new SubdivisionDetails {
                    CountryId = countryId,
                    Name = parts[9],
                    SubdivisionTypeId = (await termReaderByName.ReadAsync(new NameableIdReaderByTermNameRequest {
                        Name = "Region",
                        VocabularyId = vocabularyIdSubdivisionType
                    })),
                }
                
            };
        }
    }

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termIdReader = await termIdReaderFactory.CreateAsync(_postgresConnection);
        await using var vocabularyIdReader = await vocabularyIdReaderByOwnerAndNameFactory.CreateAsync(_postgresConnection);
        await using var termReaderByName = await termReaderByNameFactory.CreateAsync(_postgresConnection);
        await using var termReaderByNameableId = await termReaderByNameableIdFactory.CreateAsync(_postgresConnection);

        await using var informalIntermediateLevelSubdivisionCreator = await informalIntermediateLevelSubdivisionCreatorFactory.CreateAsync(_postgresConnection);
        await informalIntermediateLevelSubdivisionCreator.CreateAsync(ReadInformalIntermediateLevelSubdivisionCsv(
            nodeIdReader,
            termIdReader,
            termReaderByNameableId,
            termReaderByName
        ));
        await informalIntermediateLevelSubdivisionCreator.CreateAsync(ReadInformalIntermediateLevelSubdivisions(
            nodeIdReader,
            termIdReader,
            termReaderByName
        ));
    }
    private async IAsyncEnumerable<InformalIntermediateLevelSubdivision.ToCreate> ReadInformalIntermediateLevelSubdivisions(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader,
        IMandatorySingleItemDatabaseReader<NameableIdReaderByTermNameRequest, int> termReaderByName
    )
    {

        var vocabularyIdSubdivision = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
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
                n2.nid country_id,
                ua.dst url_path
            FROM node n 
            LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
            JOIN category_hierarchy ch ON ch.cid = n.nid
            JOIN node n2 ON n2.nid = ch.parent
            WHERE n.`type` = 'region_facts'AND n2.`type` = 'country_type'
            """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        var parentTermIds = new List<int> {
            await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                Name = "United States of America",
                VocabularyId = vocabularyIdTopics
            }
        )};
        while (await reader.ReadAsync()) {

            var id = reader.GetInt32("id");
            var title = $"{reader.GetString("title")} (region of the USA)";
            var vocabularyNames = new List<NewTermForNewNameable>
            {
                new NewTermForNewNameable
                {
                    Identification = new Identification.Possible {
                        Id = null,
                    },
                    VocabularyId = vocabularyIdTopics,
                    Name = title,
                    ParentTermIds = parentTermIds
                }
            };

            yield return new InformalIntermediateLevelSubdivision.ToCreate {
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
                            UrlId = id
                        }
                    },
                    NodeTypeId = 18,
                    TermIds = new List<int>(),
                },
                NameableDetails = new NameableDetails.ForCreate {
                    Terms = vocabularyNames,
                    Description = "",
                    FileIdTileImage = null,
                },
                SubdivisionDetails = new SubdivisionDetails {
                    Name = reader.GetString("title"),
                    CountryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                        TenantId = Constants.PPL,
                        UrlId = reader.GetInt32("country_id")
                    }),
                    SubdivisionTypeId = (await termReaderByName.ReadAsync(new NameableIdReaderByTermNameRequest {
                        Name = "Region",
                        VocabularyId = vocabularyIdSubdivision
                    })),
                }
            };
        }
        await reader.CloseAsync();
    }
}
