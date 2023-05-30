﻿using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.Convert;

internal sealed class BoundCountryMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
        IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameRequest, int> termIdReaderFactory,
        IMandatorySingleItemDatabaseReaderFactory<NameableIdReaderByTermNameRequest, int> termReaderByNameFactory,
        IEntityCreatorFactory<BoundCountry.ToCreate> boundCountryCreatorFactory
    ) : CountryMigrator(databaseConnections)
{
    protected override string Name => "bound countries";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termIdReader = await termIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termReaderByName = await termReaderByNameFactory.CreateAsync(_postgresConnection);
        await using var boundCountryCreator = await boundCountryCreatorFactory.CreateAsync(_postgresConnection);
        await boundCountryCreator.CreateAsync(ReadBoundCountries(
            nodeIdReader,
            termIdReader,
            termReaderByName
        ));
    }

    private async IAsyncEnumerable<BoundCountry.ToCreate> ReadBoundCountries(
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

        var subdivisionType = await termReaderByName.ReadAsync(new NameableIdReaderByTermNameRequest {
            VocabularyId = vocabularyIdSubdivisionTypes,
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
                            Name = bindingCountryName,
                            VocabularyId = vocabularyIdTopics
                        })
                    },
                },
            };

            yield return new BoundCountry.ToCreate {
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
                    TenantNodes = new List<TenantNode.ToCreate.ForNewNode>
                    {
                        new TenantNode.ToCreate.ForNewNode
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
                        new TenantNode.ToCreate.ForNewNode
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
                    NodeTypeId = 14,
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
                        UrlId = 41215
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
                    CountryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest() {
                        TenantId = Constants.PPL,
                        UrlId = reader.GetInt32("binding_country_id")
                    }),
                    SubdivisionTypeId = subdivisionType,
                },
                ISOCodedSubdivisionDetails = new ISOCodedSubdivisionDetails {
                    ISO3166_2_Code = GetISO3166Code2ForCountry(reader.GetInt32("id")),
                },
                BoundCountryDetails = new BoundCountryDetails {
                    BindingCountryId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                        TenantId = Constants.PPL,
                        UrlId = reader.GetInt32("binding_country_id")
                    }),
                },
            };
        }
        await reader.CloseAsync();
    }
}
