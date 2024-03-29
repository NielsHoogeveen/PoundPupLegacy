﻿using PoundPupLegacy.DomainModel;
using PoundPupLegacy.DomainModel.Creators;
using PoundPupLegacy.DomainModel.Readers;

namespace PoundPupLegacy.Convert;

internal sealed class DeportationCaseMigrator(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameRequest, int> termIdReaderFactory,
    IEntityCreatorFactory<DeportationCase.ToCreate> deportationCaseCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "deportation cases";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termIdReader = await termIdReaderFactory.CreateAsync(_postgresConnection);
        await using var deportationCaseCreator = await deportationCaseCreatorFactory.CreateAsync(_postgresConnection);
        await deportationCaseCreator.CreateAsync(ReadDeportationCases(nodeIdReader,termIdReader));
    }
    private async IAsyncEnumerable<DeportationCase.ToCreate> ReadDeportationCases(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader)
    {

        var sql = $"""
                SELECT
                     n.nid id,
                     n.uid user_id,
                     n.title,
                     n.`status`,
                     FROM_UNIXTIME(n.created) created, 
                     FROM_UNIXTIME(n.changed) `changed`,
                     31 node_type_id,
                     field_description_6_value description,
                     MIN(cf.field_date_value) `date`,
                     case 
                	    when field_state_nid = 0 then null 
                		else field_state_nid
                	end subdivision_id_from,
                	case
                		when field_country_0_nid = 0 then null
                		ELSE field_country_0_nid
                	END country_id_to,
                    ua.dst url_path
                FROM node n
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                JOIN content_type_deportation_case c ON c.nid = n.nid AND c.vid = n.vid
                LEFT JOIN content_field_cases fc ON fc.field_cases_nid = n.nid
                LEFT JOIN node n3 ON fc.nid = n3.nid AND fc.vid = n3.vid
                LEFT JOIN content_type_case_file cf ON cf.nid = n3.nid AND cf.vid = n3.vid
                GROUP BY 
                     n.nid,
                     n.uid,
                     n.title,
                     n.`status`,
                     n.created, 
                     n.changed,
                     field_description_6_value
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
        var parentTermIds = new List<int> {
            await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                Name = "adoptee deportation",
                VocabularyId = vocabularyId
            })
        };

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");
            var vocabularyNames = new List<Term.ToCreateForNewNameable> {
                new Term.ToCreateForNewNameable {
                    Identification = new Identification.Possible {
                        Id = null,
                    },
                    VocabularyId = vocabularyId,
                    Name = name,
                    ParentTermIds = parentTermIds,
                }
            };
            var country = new DeportationCase.ToCreate {
                Identification = new Identification.Possible {
                    Id = null
                },
                NodeDetails = new NodeDetails.ForCreate {
                    PublisherId = reader.GetInt32("user_id"),
                    CreatedDateTime = reader.GetDateTime("created"),
                    ChangedDateTime = reader.GetDateTime("changed"),
                    Title = name,
                    OwnerId = Constants.OWNER_CASES,
                    AuthoringStatusId = 1,
                    TenantNodes = new List<TenantNode.ToCreate.ForNewNode>
                    {
                        new TenantNode.ToCreate.ForNewNode
                        {
                            Identification = new Identification.Possible {
                                Id = null
                            },
                            TenantId = Constants.PPL,
                            PublicationStatusId = reader.GetInt32("status"),
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
                    NodeTypeId = reader.GetInt32("node_type_id"),
                    TermIds = new List<int>(),
                },
                NameableDetails = new NameableDetails.ForCreate {
                    Terms = vocabularyNames,
                    Description = reader.GetString("description"),
                    FileIdTileImage = null,
                },
                LocatableDetails = new LocatableDetails.ForCreate { 
                    Locations = new List<Location.ToCreate>(),
                },
                CaseDetails = new CaseDetails.CaseDetailsForCreate {
                    Date = reader.IsDBNull("date") ? null : StringToDateTimeRange(reader.GetString("date"))?.ToFuzzyDate(),
                    CaseCaseParties = new List<CaseCaseParties.ToCreate.ForNewCase>(),
                },
                DeportationCaseDetails = new DeportationCaseDetails {
                    SubdivisionIdFrom = reader.IsDBNull("subdivision_id_from")
                    ? null
                    : await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                        TenantId = Constants.PPL,
                        UrlId = reader.GetInt32("subdivision_id_from")
                    }),
                    CountryIdTo = reader.IsDBNull("country_id_to")
                    ? null
                    : await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                        TenantId = Constants.PPL,
                        UrlId = reader.GetInt32("country_id_to")
                    }),
                },
            };
            yield return country;
        }
        await reader.CloseAsync();
    }
}
