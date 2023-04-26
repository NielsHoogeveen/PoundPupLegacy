namespace PoundPupLegacy.Convert;

internal sealed class BasicSecondLevelSubdivisionMigrator : MigratorPPL
{
    private readonly IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> _nodeIdReaderFactory;
    private readonly IMandatorySingleItemDatabaseReaderFactory<SubdivisionIdReaderByNameRequest, int> _subdivisionIdByNameReaderFactory;
    private readonly IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> _vocabularyIdReaderByOwnerAndNameFactory;
    private readonly ISingleItemDatabaseReaderFactory<TermReaderByNameableIdRequest, CreateModel.Term> _termReaderByNameableIdFactory;
    private readonly IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, CreateModel.Term> _termReaderByNameFactory;
    private readonly IMandatorySingleItemDatabaseReaderFactory<SubdivisionIdReaderByIso3166CodeRequest, int> _subdivisionIdReaderByIso3166CodeFactory;
    private readonly IEntityCreator<BasicSecondLevelSubdivision> _basicSecondLevelSubdivisionCreator;

    protected override string Name => "basic second level subdivisions";

    public BasicSecondLevelSubdivisionMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
        IMandatorySingleItemDatabaseReaderFactory<SubdivisionIdReaderByNameRequest, int> subdivisionIdByNameReaderFactory,
        IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderByOwnerAndNameFactory,
        ISingleItemDatabaseReaderFactory<TermReaderByNameableIdRequest, CreateModel.Term> termReaderByNameableIdFactory,
        IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, CreateModel.Term> termReaderByNameFactory,
        IMandatorySingleItemDatabaseReaderFactory<SubdivisionIdReaderByIso3166CodeRequest, int> subdivisionIdReaderByIso3166CodeFactory,
        IEntityCreator<BasicSecondLevelSubdivision> basicSecondLevelSubdivisionCreator
    ) : base(databaseConnections)
    {
        _nodeIdReaderFactory = nodeIdReaderFactory;
        _subdivisionIdByNameReaderFactory = subdivisionIdByNameReaderFactory;
        _vocabularyIdReaderByOwnerAndNameFactory = vocabularyIdReaderByOwnerAndNameFactory;
        _termReaderByNameableIdFactory = termReaderByNameableIdFactory;
        _termReaderByNameFactory = termReaderByNameFactory;
        _subdivisionIdReaderByIso3166CodeFactory = subdivisionIdReaderByIso3166CodeFactory;
        _basicSecondLevelSubdivisionCreator = basicSecondLevelSubdivisionCreator;
    }

    private async IAsyncEnumerable<BasicSecondLevelSubdivision> ReadBasicSecondLevelSubdivisionsInInformalPrimarySubdivisionCsv(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<SubdivisionIdReaderByNameRequest, int> subdivisionIdByNameReader,
        IMandatorySingleItemDatabaseReader<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReader,
        ISingleItemDatabaseReader<TermReaderByNameableIdRequest, CreateModel.Term> termReaderByNameableId,
        IMandatorySingleItemDatabaseReader<TermReaderByNameRequest, CreateModel.Term> termReaderByName
        )
    {
        var vocabularyId = await vocabularyIdReader.ReadAsync(new VocabularyIdReaderByOwnerAndNameRequest {
            OwnerId = Constants.OWNER_GEOGRAPHY,
            Name = "Subdivision type"
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
            var topicName = (await termReaderByNameableId.ReadAsync(new TermReaderByNameableIdRequest {
                OwnerId = Constants.PPL,
                NameableId = subdivisionId,
                VocabularyName = Constants.VOCABULARY_TOPICS
            }))!.Name;
            yield return new BasicSecondLevelSubdivision {
                Id = null,
                CreatedDateTime = DateTime.Parse(parts[1]),
                ChangedDateTime = DateTime.Parse(parts[2]),
                NodeTypeId = int.Parse(parts[4]),
                OwnerId = Constants.OWNER_GEOGRAPHY,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = int.Parse(parts[5]),
                        UrlPath = null,
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
                PublisherId = int.Parse(parts[6]),
                CountryId = countryId,
                Title = title,
                Name = parts[9],
                Description = "",
                VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = Constants.PPL,
                        Name = Constants.VOCABULARY_TOPICS,
                        TermName = title,
                        ParentNames = new List<string> { topicName },
                    }
                },
                ISO3166_2_Code = parts[10],
                IntermediateLevelSubdivisionId = subdivisionId,
                FileIdFlag = null,
                FileIdTileImage = null,
                SubdivisionTypeId = (await termReaderByName.ReadAsync(new TermReaderByNameRequest {
                    VocabularyId = vocabularyId,
                    Name = parts[12].Trim()
                })).NameableId
            };
        }
    }

    private async IAsyncEnumerable<BasicSecondLevelSubdivision> ReadBasicSecondLevelSubdivisionCsv(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<SubdivisionIdReaderByIso3166CodeRequest, int> subdivisionIdReaderByIso3166Code,
        IMandatorySingleItemDatabaseReader<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReader,
        ISingleItemDatabaseReader<TermReaderByNameableIdRequest, CreateModel.Term> termReaderByNameableId,
        IMandatorySingleItemDatabaseReader<TermReaderByNameRequest, CreateModel.Term> termReaderByName
        )
    {
        var vocabularyId = await vocabularyIdReader.ReadAsync(new VocabularyIdReaderByOwnerAndNameRequest {
            OwnerId = Constants.OWNER_GEOGRAPHY,
            Name = "Subdivision type"
        });
        await foreach (string line in System.IO.File.ReadLinesAsync(@"..\..\..\files\BasicSecondLevelSubdivisions.csv").Skip(1)) {

            var parts = line.Split(new char[] { ';' }).Select(x => x.TrimStart()).ToList();
            int? id = int.Parse(parts[0]) == 0 ? null : int.Parse(parts[0]);
            var title = parts[8];
            var subdivisionId = await subdivisionIdReaderByIso3166Code.ReadAsync(new SubdivisionIdReaderByIso3166CodeRequest {
                Iso3166Code = parts[11]
            });
            var topicName = (await termReaderByNameableId.ReadAsync(new TermReaderByNameableIdRequest {
                OwnerId = Constants.PPL,
                NameableId = subdivisionId,
                VocabularyName = Constants.VOCABULARY_TOPICS
            }))!.Name;
            yield return new BasicSecondLevelSubdivision {
                Id = null,
                CreatedDateTime = DateTime.Parse(parts[1]),
                ChangedDateTime = DateTime.Parse(parts[2]),
                NodeTypeId = int.Parse(parts[4]),
                OwnerId = Constants.OWNER_GEOGRAPHY,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = int.Parse(parts[5]),
                        UrlPath = null,
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
                PublisherId = int.Parse(parts[6]),
                CountryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest() {
                    TenantId = Constants.PPL,
                    UrlId = int.Parse(parts[7])
                }),
                Title = title,
                Name = parts[9],
                Description = "",
                VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = Constants.PPL,
                        Name = Constants.VOCABULARY_TOPICS,
                        TermName = title,
                        ParentNames = new List<string> { topicName },
                    }
                },
                ISO3166_2_Code = parts[10],
                IntermediateLevelSubdivisionId = subdivisionId,
                FileIdFlag = null,
                FileIdTileImage = null,
                SubdivisionTypeId = (await termReaderByName.ReadAsync(new TermReaderByNameRequest {
                    VocabularyId = vocabularyId,
                    Name = parts[12].Trim()
                })).NameableId
            };
        }
    }
    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await _nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var subdivisionIdByNameReader = await _subdivisionIdByNameReaderFactory.CreateAsync(_postgresConnection);
        await using var subdivisionIdReaderByIso3166Code = await _subdivisionIdReaderByIso3166CodeFactory.CreateAsync(_postgresConnection);
        await using var vocabularyIdReader = await _vocabularyIdReaderByOwnerAndNameFactory.CreateAsync(_postgresConnection);
        await using var termReaderByNameableId = await _termReaderByNameableIdFactory.CreateAsync(_postgresConnection);
        await using var termReaderByName = await _termReaderByNameFactory.CreateAsync(_postgresConnection);

        await _basicSecondLevelSubdivisionCreator.CreateAsync(ReadBasicSecondLevelSubdivisionsInInformalPrimarySubdivisionCsv(
            nodeIdReader,
            subdivisionIdByNameReader,
            vocabularyIdReader,
            termReaderByNameableId,
            termReaderByName
        ), _postgresConnection);
        await _basicSecondLevelSubdivisionCreator.CreateAsync(ReadBasicSecondLevelSubdivisionCsv(
            nodeIdReader,
            subdivisionIdReaderByIso3166Code,
            vocabularyIdReader,
            termReaderByNameableId,
            termReaderByName
        ), _postgresConnection);
        await _basicSecondLevelSubdivisionCreator.CreateAsync(ReadBasicSecondLevelSubdivisions(
            nodeIdReader,
            vocabularyIdReader,
            termReaderByName
            ), _postgresConnection);

    }
    private async IAsyncEnumerable<BasicSecondLevelSubdivision> ReadBasicSecondLevelSubdivisions(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReader,
        IMandatorySingleItemDatabaseReader<TermReaderByNameRequest, CreateModel.Term> termReaderByName
        )
    {
        var vocabularyId = await vocabularyIdReader.ReadAsync(new VocabularyIdReaderByOwnerAndNameRequest {
            OwnerId = Constants.OWNER_GEOGRAPHY,
            Name = "Subdivision type"
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
            var vocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.PPL,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = title,
                    ParentNames = new List<string>{ subdivisioName },
                }
            };
            yield return new BasicSecondLevelSubdivision {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = title,
                Name = reader.GetString("title"),
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
                NodeTypeId = 19,
                Description = "",
                VocabularyNames = vocabularyNames,
                IntermediateLevelSubdivisionId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = reader.GetInt32("intermediate_level_subdivision_id")
                }),
                CountryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = reader.GetInt32("country_id")
                }),
                ISO3166_2_Code = reader.GetString("iso_3166_2_code"),
                FileIdFlag = reader.IsDBNull("file_id_flag") ? null : reader.GetInt32("file_id_flag"),
                FileIdTileImage = null,
                SubdivisionTypeId = (await termReaderByName.ReadAsync(new TermReaderByNameRequest {
                    VocabularyId = vocabularyId,
                    Name = "State"
                })).NameableId
            };
        }
        await reader.CloseAsync();
    }

}
