namespace PoundPupLegacy.Convert;

internal sealed class InformalIntermediateLevelSubdivisionMigrator : MigratorPPL
{
    protected override string Name => "informal intermediate level subdivisions";

    private readonly IDatabaseReaderFactory<NodeIdReaderByUrlId> _nodeIdReaderFactory;
    private readonly IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> _vocabularyIdReaderByOwnerAndNameFactory;
    private readonly IDatabaseReaderFactory<TermReaderByName> _termReaderByNameFactory;
    private readonly IDatabaseReaderFactory<TermReaderByNameableId> _termReaderByNameableIdFactory;
    private readonly IEntityCreator<InformalIntermediateLevelSubdivision> _informalIntermediateLevelSubdivisionCreator;

    public InformalIntermediateLevelSubdivisionMigrator(
        IDatabaseConnections databaseConnections,
        IDatabaseReaderFactory<NodeIdReaderByUrlId> nodeIdReaderFactory,
        IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> vocabularyIdReaderByOwnerAndNameFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderByNameFactory,
        IDatabaseReaderFactory<TermReaderByNameableId> termReaderByNameableIdFactory,
        IEntityCreator<InformalIntermediateLevelSubdivision> informalIntermediateLevelSubdivisionCreator
    ) : base(databaseConnections)
    {
        _nodeIdReaderFactory = nodeIdReaderFactory;
        _vocabularyIdReaderByOwnerAndNameFactory = vocabularyIdReaderByOwnerAndNameFactory;
        _termReaderByNameFactory = termReaderByNameFactory;
        _termReaderByNameableIdFactory = termReaderByNameableIdFactory;
        _informalIntermediateLevelSubdivisionCreator = informalIntermediateLevelSubdivisionCreator;
    }

    private async IAsyncEnumerable<InformalIntermediateLevelSubdivision> ReadInformalIntermediateLevelSubdivisionCsv(
        NodeIdReaderByUrlId nodeIdReader,
        VocabularyIdReaderByOwnerAndName vocabularyIdReader,
        TermReaderByName termReaderByName,
        TermReaderByNameableId termReaderByNameableId
        )
    {
        var vocabularyId = await vocabularyIdReader.ReadAsync(new VocabularyIdReaderByOwnerAndName.Request {
            OwnerId = Constants.OWNER_GEOGRAPHY,
            Name = "Subdivision type"
        });

        await foreach (string line in System.IO.File.ReadLinesAsync(@"..\..\..\files\InformalIntermediateLevelSubdivisions.csv").Skip(1)) {

            var parts = line.Split(new char[] { ';' }).Select(x => x.TrimStart()).ToList();
            int? id = int.Parse(parts[0]) == 0 ? null : int.Parse(parts[0]);
            var title = parts[8];
            var countryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                TenantId = Constants.PPL,
                UrlId = int.Parse(parts[7])
            });
            var countryName = (await termReaderByNameableId.ReadAsync(new TermReaderByNameableId.Request {
                NameableId = countryId,
                OwnerId = Constants.PPL,
                VocabularyName = Constants.VOCABULARY_TOPICS
            })).Name;
            yield return new InformalIntermediateLevelSubdivision {
                Id = null,
                CreatedDateTime = DateTime.Parse(parts[1]),
                ChangedDateTime = DateTime.Parse(parts[2]),
                VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = Constants.PPL,
                        Name = Constants.VOCABULARY_TOPICS,
                        TermName = title,
                        ParentNames = new List<string> { countryName },
                    }
                },
                Description = "",
                FileIdTileImage = null,
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
                SubdivisionTypeId = (await termReaderByName.ReadAsync(new TermReaderByName.Request {
                    Name = "Region",
                    VocabularyId = vocabularyId
                })).NameableId
            };
        }
    }

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await _nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var vocabularyIdReader = await _vocabularyIdReaderByOwnerAndNameFactory.CreateAsync(_postgresConnection);
        await using var termReaderByName = await _termReaderByNameFactory.CreateAsync(_postgresConnection);
        await using var termReaderByNameableId = await _termReaderByNameableIdFactory.CreateAsync(_postgresConnection);

        await _informalIntermediateLevelSubdivisionCreator.CreateAsync(ReadInformalIntermediateLevelSubdivisionCsv(
            nodeIdReader,
            vocabularyIdReader,
            termReaderByName,
            termReaderByNameableId
        ), _postgresConnection);
        await _informalIntermediateLevelSubdivisionCreator.CreateAsync(ReadInformalIntermediateLevelSubdivisions(
            nodeIdReader,
            vocabularyIdReader,
            termReaderByName
        ), _postgresConnection);
    }
    private async IAsyncEnumerable<InformalIntermediateLevelSubdivision> ReadInformalIntermediateLevelSubdivisions(
        NodeIdReaderByUrlId nodeIdReader,
        VocabularyIdReaderByOwnerAndName vocabularyIdReader,
        TermReaderByName termReaderByName
        )
    {

        var vocabularyId = await vocabularyIdReader.ReadAsync(new VocabularyIdReaderByOwnerAndName.Request {
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

        while (await reader.ReadAsync()) {

            var id = reader.GetInt32("id");
            var title = $"{reader.GetString("title")} (region of the USA)";
            var vocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.PPL,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = title,
                    ParentNames = new List<string>{ "United States of America" },
                }
            };

            yield return new InformalIntermediateLevelSubdivision {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = title,
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
                        UrlId = id
                    }
                },
                NodeTypeId = 18,
                CountryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                    TenantId = Constants.PPL,
                    UrlId = reader.GetInt32("country_id")
                }),
                Name = reader.GetString("title"),
                VocabularyNames = vocabularyNames,
                Description = "",
                FileIdTileImage = null,
                SubdivisionTypeId = (await termReaderByName.ReadAsync(new TermReaderByName.Request {
                    Name = "Region",
                    VocabularyId = vocabularyId
                })).NameableId
            };
        }
        await reader.CloseAsync();
    }
}
