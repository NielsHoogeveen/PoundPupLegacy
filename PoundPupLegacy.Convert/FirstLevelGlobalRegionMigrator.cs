using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class FirstLevelGlobalRegionMigrator : PPLMigrator
{
    public FirstLevelGlobalRegionMigrator(MySqlToPostgresConverter converter) : base(converter) { }

    protected override string Name => "first level global regions";

    protected override async Task MigrateImpl()
    {
        await FirstLevelGlobalRegionCreator.CreateAsync(ReadFirstLevelGlobalRegions(), _postgresConnection);
    }

    private async IAsyncEnumerable<FirstLevelGlobalRegion> ReadFirstLevelGlobalRegions()
    {
        var sql = $"""
            SELECT n.nid id,
            n.uid access_role_id,
            n.title,
            n.`status` node_status_id,
            FROM_UNIXTIME(n.created) created_date_time, 
            FROM_UNIXTIME(n.changed) changed_date_time,
            nr.body description,
            cc.field_tile_image_fid file_id_tile_image,
            ua.dst url_path
            FROM node n 
            LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
            JOIN category_hierarchy ch ON ch.cid = n.nid
            JOIN node n2 ON n2.nid = ch.parent
                	LEFT JOIN content_type_category_cat cc ON cc.nid = n.nid AND cc.vid = n.vid
                LEFT JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
            WHERE n.`type` = 'region_facts'
            AND n.nid < 30000
            AND n2.`type` = 'category_cont'
            """;

        using var readCommand = _mysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");

            var vocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.PPL,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = name,
                    ParentNames = new List<string>{ "Around the world"},
                }
            };

            yield return new FirstLevelGlobalRegion
            {
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
                        TenantId = 1,
                        PublicationStatusId = reader.GetInt32("node_status_id"),
                        UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                NodeTypeId = 11,
                VocabularyNames = vocabularyNames,
                Description = reader.GetString("description"),
                FileIdTileImage = reader.IsDBNull("file_id_tile_image") ? null : reader.GetInt32("file_id_tile_image"),
                Name = name,
            };
        }
        await reader.CloseAsync();
    }
}

