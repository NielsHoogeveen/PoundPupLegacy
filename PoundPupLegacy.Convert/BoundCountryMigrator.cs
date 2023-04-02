﻿using PoundPupLegacy.CreateModel.Readers;

namespace PoundPupLegacy.Convert;

internal sealed class BoundCountryMigrator : CountryMigrator
{
    private readonly IDatabaseReaderFactory<NodeIdReaderByUrlId> _nodeIdReaderFactory;
    private readonly IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> _vocabularyIdReaderByOwnerAndNameFactory;
    private readonly IDatabaseReaderFactory<TermReaderByName> _termReaderByNameFactory;
    private readonly IEntityCreator<BoundCountry> _boundCountryCreator;
    public BoundCountryMigrator(
        IDatabaseConnections databaseConnections,
        IDatabaseReaderFactory<NodeIdReaderByUrlId> nodeIdReaderFactory,
        IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> vocabularyIdReaderByOwnerAndNameFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderByNameFactory,
        IEntityCreator<BoundCountry> boundCountryCreator
    ) : base(databaseConnections) 
    { 
        _nodeIdReaderFactory = nodeIdReaderFactory;
        _vocabularyIdReaderByOwnerAndNameFactory = vocabularyIdReaderByOwnerAndNameFactory;
        _termReaderByNameFactory = termReaderByNameFactory;
        _boundCountryCreator = boundCountryCreator;
    }

    protected override string Name => "bound countries";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await _nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var vocabularyReader = await _vocabularyIdReaderByOwnerAndNameFactory.CreateAsync(_postgresConnection);
        await using var termReaderByName = await _termReaderByNameFactory.CreateAsync(_postgresConnection);

        await _boundCountryCreator.CreateAsync(ReadBoundCountries(
            nodeIdReader,
            vocabularyReader,
            termReaderByName
        ), _postgresConnection);
    }

    private async IAsyncEnumerable<BoundCountry> ReadBoundCountries(
        NodeIdReaderByUrlId nodeIdReader,
        VocabularyIdReaderByOwnerAndName vocabularyReader,
        TermReaderByName termReaderByName
        )
    {

        var vocabularyId = await vocabularyReader.ReadAsync(new VocabularyIdReaderByOwnerAndName.Request {
            OwnerId = Constants.OWNER_GEOGRAPHY,
            Name = "Subdivision type"
        });
        var subdivisionType = await termReaderByName.ReadAsync(new TermReaderByName.Request {
            VocabularyId = vocabularyId,
            Name = "Country"
        });

        var sql = $"""
            SELECT
                n.nid id,
                n.uid access_role_id,
                n.title,
                n.`status` node_status_id,
                FROM_UNIXTIME(n.created) created_date_time, 
                FROM_UNIXTIME(n.changed) changed_date_time,
                n2.nid binding_country_id,
                n2.title binding_country_name,
                ua.dst url_path
            FROM node n 
            LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
            JOIN content_type_country_type cou ON cou.nid = n.nid
            JOIN category_hierarchy ch ON ch.cid = n.nid
            JOIN node n2 ON n2.nid = ch.parent
            WHERE n.`type` = 'country_type'
            AND n2.`type` = 'country_type'
            AND n.nid <> 11572
            """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");
            var bindingCountryName = reader.GetString("binding_country_name");
            var vocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.PPL,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = name,
                    ParentNames = new List<string>{ bindingCountryName },
                }
            };

            yield return new BoundCountry {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = name,
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
                NodeTypeId = 14,
                Description = "",
                VocabularyNames = vocabularyNames,
                BindingCountryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                    TenantId = Constants.PPL,
                    UrlId = reader.GetInt32("binding_country_id")
                }),
                Name = name,
                ISO3166_2_Code = GetISO3166Code2ForCountry(reader.GetInt32("id")),
                CountryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request() {
                    TenantId = Constants.PPL,
                    UrlId = reader.GetInt32("binding_country_id")
                }),
                FileIdFlag = null,
                FileIdTileImage = null,
                HagueStatusId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                    TenantId = Constants.PPL,
                    UrlId = 41215
                }),
                ResidencyRequirements = null,
                AgeRequirements = null,
                HealthRequirements = null,
                IncomeRequirements = null,
                MarriageRequirements = null,
                OtherRequirements = null,
                SubdivisionTypeId = subdivisionType!.NameableId,
            };

        }
        await reader.CloseAsync();
    }
}
