using System.Xml.Linq;

namespace PoundPupLegacy.Convert;

internal sealed class WrongfulMedicationCaseMigrator : MigratorPPL
{
    private readonly IEntityCreator<WrongfulMedicationCase> _wrongfulMedicationCaseCreator;
    public WrongfulMedicationCaseMigrator(
        IDatabaseConnections databaseConnections,
        IEntityCreator<WrongfulMedicationCase> wrongfulMedicationCaseCreator
    ) : base(databaseConnections)
    {
        _wrongfulMedicationCaseCreator = wrongfulMedicationCaseCreator;
    }

    protected override string Name => "wrongful medication cases";

    protected override async Task MigrateImpl()
    {
        await _wrongfulMedicationCaseCreator.CreateAsync(ReadWrongfulMedicationCases(), _postgresConnection);
    }
    private async IAsyncEnumerable<WrongfulMedicationCase> ReadWrongfulMedicationCases()
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

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var title = reader.GetString("title");
            var vocabularyNames = new List<VocabularyName> {
                new VocabularyName {
                    OwnerId = Constants.PPL,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = title,
                    ParentNames = new List<string>{ "overmedication in foster care"},
                }
            };

            var country = new WrongfulMedicationCase {
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = reader.GetString("title"),
                OwnerId = Constants.OWNER_CASES,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = reader.GetInt32("status"),
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
                NodeTypeId = reader.GetInt32("node_type_id"),
                VocabularyNames = vocabularyNames,
                Date = reader.IsDBNull("date") ? null : StringToDateTimeRange(reader.GetString("date")),
                Description = reader.GetString("description"),
                FileIdTileImage = null,
            };
            yield return country;

        }
        await reader.CloseAsync();
    }
}
