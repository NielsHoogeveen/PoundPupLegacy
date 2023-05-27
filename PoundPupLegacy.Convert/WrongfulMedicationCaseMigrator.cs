﻿namespace PoundPupLegacy.Convert;

internal sealed class WrongfulMedicationCaseMigrator(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameRequest, int> termIdReaderFactory,
    IEntityCreatorFactory<EventuallyIdentifiableWrongfulMedicationCase> wrongfulMedicationCaseCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "wrongful medication cases";

    protected override async Task MigrateImpl()
    {
        await using var wrongfulMedicationCaseCreator = await wrongfulMedicationCaseCreatorFactory.CreateAsync(_postgresConnection);
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termIdReader = await termIdReaderFactory.CreateAsync(_postgresConnection);
        await wrongfulMedicationCaseCreator.CreateAsync(ReadWrongfulMedicationCases(nodeIdReader,termIdReader));
    }
    private async IAsyncEnumerable<NewWrongfulMedicationCase> ReadWrongfulMedicationCases(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader
    )
    {

        var sql = $"""
                SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`,
                    33 node_type_id,
                    field_long_description_1_value description,
                    field_report_date_0_value `date`,
                    ua.dst url_path
                FROM node n
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                JOIN content_type_wrongful_medication_case c ON c.nid = n.nid AND c.vid = n.vid
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
                    Name = "overmedication in foster care",
                    VocabularyId = vocabularyId
                })
        };
        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var title = reader.GetString("title");
            var vocabularyNames = new List<NewTermForNewNameable> {
                new NewTermForNewNameable {
                    VocabularyId = vocabularyId,
                    Name = title,
                    ParentTermIds = parentTermIds,
                }
            };

            var country = new NewWrongfulMedicationCase {
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = reader.GetString("title"),
                OwnerId = Constants.OWNER_CASES,
                AuthoringStatusId = 1,
                TenantNodes = new List<NewTenantNodeForNewNode>
                {
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = reader.GetInt32("status"),
                        UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                        SubgroupId = null,
                        UrlId = id
                    },
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = id < 33163 ? id : null
                    }
                },
                NodeTypeId = reader.GetInt32("node_type_id"),
                Terms = vocabularyNames,
                Date = reader.IsDBNull("date") ? null : StringToDateTimeRange(reader.GetString("date"))?.ToFuzzyDate(),
                Description = reader.GetString("description"),
                FileIdTileImage = null,
                TermIds = new List<int>(),
                Locations = new List<EventuallyIdentifiableLocation>(),
                CaseParties = new List<NewCaseNewCaseParties>(),
            };
            yield return country;

        }
        await reader.CloseAsync();
    }
}
