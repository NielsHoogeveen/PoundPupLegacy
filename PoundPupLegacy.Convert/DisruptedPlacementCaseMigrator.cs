namespace PoundPupLegacy.Convert;

internal sealed class DisruptedPlacementCaseMigrator(
    IDatabaseConnections databaseConnections,
    INameableCreatorFactory<EventuallyIdentifiableDisruptedPlacementCase> disruptedPlacementCaseCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "disrupted placement cases";

    protected override async Task MigrateImpl()
    {
        await using var disruptedPlacementCaseCreator = await disruptedPlacementCaseCreatorFactory.CreateAsync(_postgresConnection);
        await disruptedPlacementCaseCreator.CreateAsync(ReadDisruptedPlacementCases());
    }
    private async IAsyncEnumerable<NewDisruptedPlacementCase> ReadDisruptedPlacementCases()
    {

        var sql = $"""
                SELECT
                     n.nid id,
                     n.uid user_id,
                     n.title,
                     n.`status`,
                     FROM_UNIXTIME(n.created) created, 
                     FROM_UNIXTIME(n.changed) `changed`,
                     44 node_type_id,
                     cc.nid IS NOT null is_topic,
                     field_description_long_value description,
                     field_disruption_date_value `date`,
                    ua.dst url_path
                FROM node n
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                JOIN content_type_disrupted_placement_case c ON c.nid = n.nid AND c.vid = n.vid
                LEFT JOIN content_type_category_cat cc ON cc.field_related_page_nid = n.nid 
                LEFT JOIN node n2 ON n2.nid = cc.nid AND n2.vid = cc.vid
                """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");
            var vocabularyNames = new List<VocabularyName> {
                new VocabularyName {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = name,
                    ParentNames = new List<string>{ "disrupted adoption"},
                }
            };
            var country = new NewDisruptedPlacementCase {
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = name,
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
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    },
                    new NewTenantNodeForNewNode
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
                Date = reader.IsDBNull("date") ? null : StringToDateTimeRange(reader.GetString("date"))?.ToFuzzyDate(),
                Description = reader.GetString("description"),
                FileIdTileImage = null,
                NodeTermIds = new List<int>(),
            };
            yield return country;

        }
        await reader.CloseAsync();
    }
}
