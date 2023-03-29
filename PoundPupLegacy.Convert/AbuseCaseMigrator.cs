namespace PoundPupLegacy.Convert;
internal sealed class AbuseCaseMigrator : PPLMigrator
{
    public AbuseCaseMigrator(MySqlToPostgresConverter converter) : base(converter)
    {

    }

    protected override string Name => "abuse cases";

    protected override async Task MigrateImpl()
    {
        await AbuseCaseCreator.CreateAsync(ReadAbuseCases(), _postgresConnection);
    }
    private async IAsyncEnumerable<AbuseCase> ReadAbuseCases()
    {

        var sql = $"""
                SELECT
                n.nid id,
                n.uid access_role_id,
                n.title,
                n.`status` node_status_id,
                FROM_UNIXTIME(n.created) created_date_time, 
                FROM_UNIXTIME(n.changed) changed_date_time,
                26 node_type_id,
                		case 
                        when c2.title IS NOT NULL then c2.title
                        ELSE c3.title
                END topic_name,
                case 
                    when c2.topic_parent_names IS NOT NULL then c2.topic_parent_names
                    ELSE c3.topic_parent_names
                END topic_parent_names,
                		c.field_discovery_date_value `date`,
                c.field_body_0_value description,
                case 
                    when field_child_placement_type_value = 'Adoption' then {Constants.ADOPTION}
                    when field_child_placement_type_value = 'Foster care' then {Constants.FOSTER_CARE}
                    when field_child_placement_type_value = 'To be adopted' then {Constants.TO_BE_ADOPTED}
                    when field_child_placement_type_value = 'Legal Guardianship' then {Constants.LEGAL_GUARDIANSHIP}
                    when field_child_placement_type_value = 'Institution' then {Constants.INSTITUTION}
                END child_placement_type_id,
                case 
                    when c.field_family_size_value = '1 to 4' then {Constants.ONE_TO_FOUR}
                    when c.field_family_size_value = '4 to 8' then {Constants.FOUR_TO_EIGHT}
                    when c.field_family_size_value = '8 to 12' then {Constants.EIGHT_TO_TWELVE}
                    when c.field_family_size_value = 'more than 12' then {Constants.MORE_THAN_TWELVE}
                    when field_child_placement_type_value = '' then null
                    when field_child_placement_type_value = null then null
                END family_size_id,
                case when c.field_home_schooling_value = 'yes' then true else null END home_schooling_involved,
                case when field_fundamentalist_faith_value = 'yes' then TRUE ELSE NULL END fundamental_faith_involved,
                case when field_disabilities_value = 'yes' then TRUE ELSE NULL END disabilities_involved,
                ua.dst url_path
                FROM node n
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                JOIN content_type_case c ON c.nid = n.nid AND c.vid = n.vid
                LEFT JOIN content_type_category_cat cc ON cc.field_related_page_nid = n.nid AND cc.nid <> 44518
                LEFT JOIN node n2 ON n2.nid = cc.nid AND n2.vid = cc.vid
                LEFT JOIN (
                    select
                    n.nid,
                    n.title,
                    cc.field_tile_image_title,
                    cc.field_related_page_nid,
                    GROUP_CONCAT(p.title, ',') topic_parent_names
                    FROM node n
                    JOIN content_type_category_cat cc ON cc.nid = n.nid AND cc.vid = n.vid
                    LEFT JOIN (
                        SELECT
                        n.nid, 
                        n.title,
                        ch.cid
                        FROM node n
                        JOIN category_hierarchy ch ON ch.parent = n.nid
                        WHERE n.`type` = 'category_cat'
                    ) p ON p.cid = n.nid
                    WHERE n.nid NOT IN (44881)
                    GROUP BY
                    n.nid,
                    n.title,
                    cc.field_tile_image_title,
                    cc.field_related_page_nid
                ) c2 ON c2.field_related_page_nid = n.nid
                		LEFT JOIN (
                    select
                        n.nid,
                        n.title,
                        GROUP_CONCAT(p.title, ',') topic_parent_names
                    FROM node n
                    JOIN category c ON c.cid = n.nid AND c.cnid = 4126
                    LEFT JOIN (
                        SELECT
                            n.nid, 
                            n.title,
                            ch.cid
                        FROM node n
                        JOIN category_hierarchy ch ON ch.parent = n.nid
                        WHERE n.`type` = 'category_cat'
                    ) p ON p.cid = n.nid
                    GROUP BY 
                        n.nid,
                        n.title
                ) c3 ON c3.title = n.title                
                """;
        using var readCommand = MysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");

            var vocabularyNames = new List<VocabularyName>();

            if (!reader.IsDBNull("topic_name")) {
                var topicName = reader.GetString("topic_name");
                var topicParentNames = reader.IsDBNull("topic_parent_names") ?
                    new List<string>() : reader.GetString("topic_parent_names")
                    .Split(',')
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();

                vocabularyNames.Add(new VocabularyName {
                    OwnerId = Constants.PPL,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = topicName,
                    ParentNames = topicParentNames,
                });
            }


            var country = new AbuseCase {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = name,
                OwnerId = Constants.OWNER_CASES,
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
                NodeTypeId = reader.GetInt32("node_type_id"),
                Date = reader.IsDBNull("date") ? null : StringToDateTimeRange(reader.GetString("date")),
                Description = reader.GetString("description"),
                FileIdTileImage = null,
                ChildPlacementTypeId = await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = reader.GetInt32("child_placement_type_id")
                }),
                FamilySizeId = reader.IsDBNull("family_size_id") ? null : await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.NodeIdReaderByUrlIdRequest {
                    TenantId = Constants.PPL,
                    UrlId = reader.GetInt32("family_size_id")
                }),
                HomeschoolingInvolved = reader.IsDBNull("home_schooling_involved") ? null : reader.GetBoolean("home_schooling_involved"),
                FundamentalFaithInvolved = reader.IsDBNull("fundamental_faith_involved") ? null : reader.GetBoolean("fundamental_faith_involved"),
                DisabilitiesInvolved = reader.IsDBNull("disabilities_involved") ? null : reader.GetBoolean("disabilities_involved"),
                VocabularyNames = vocabularyNames,
            };
            yield return country;

        }
        await reader.CloseAsync();
    }
}
