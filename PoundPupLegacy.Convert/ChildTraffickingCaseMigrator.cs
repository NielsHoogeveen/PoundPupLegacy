namespace PoundPupLegacy.Convert;

internal sealed class ChildTraffickingCaseMigrator : MigratorPPL
{
    private readonly IDatabaseReaderFactory<NodeIdReaderByUrlId> _nodeIdReaderFactory;
    private readonly IEntityCreator<ChildTraffickingCase> _childTraffickingCaseCreator;
    public ChildTraffickingCaseMigrator(
        IDatabaseConnections databaseConnections,
        IDatabaseReaderFactory<NodeIdReaderByUrlId> nodeIdReaderFactory,
        IEntityCreator<ChildTraffickingCase> childTraffickingCaseCreator
    ) : base(databaseConnections)
    {
        _childTraffickingCaseCreator = childTraffickingCaseCreator;
        _nodeIdReaderFactory = nodeIdReaderFactory;
    }

    protected override string Name => "child trafficking cases";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await _nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await _childTraffickingCaseCreator.CreateAsync(ReadChildTraffickingCases(nodeIdReader), _postgresConnection);
    }
    private async IAsyncEnumerable<ChildTraffickingCase> ReadChildTraffickingCases(NodeIdReaderByUrlId nodeIdReader)
    {
        var sql = $"""
                SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`,
                    29 node_type_id,
                    cc.nid IS NOT null is_topic,
                    field_description_7_value description,
                    field_discovery_date_0_value `date`,
                    field_number_of_children_value number_of_children_involved,
                    field_country_from_nid country_id_from,
                    ua.dst url_path
                FROM node n
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                JOIN content_type_child_trafficking_case c ON c.nid = n.nid AND c.vid = n.vid
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
            var country = new ChildTraffickingCase {
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = name,
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
                VocabularyNames = new List<VocabularyName>(),
                Date = reader.IsDBNull("date") ? null : StringToDateTimeRange(reader.GetString("date")),
                Description = reader.GetString("description"),
                NumberOfChildrenInvolved = reader.IsDBNull("number_of_children_involved") ? null : reader.GetInt32("number_of_children_involved"),
                CountryIdFrom = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                    TenantId = Constants.PPL,
                    UrlId = reader.GetInt32("country_id_from")
                }),
                FileIdTileImage = null,
            };
            yield return country;

        }
        await reader.CloseAsync();
    }
}
