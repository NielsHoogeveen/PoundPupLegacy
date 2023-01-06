using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{
    private static async Task MigrateFirstLevelGlobalRegions(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        await using var tx = await connection.BeginTransactionAsync();
        try
        {
            await FirstLevelGlobalRegionCreator.CreateAsync(ReadFirstLevelGlobalRegions(mysqlconnection), connection);
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    private static async IAsyncEnumerable<FirstLevelGlobalRegion> ReadFirstLevelGlobalRegions(MySqlConnection mysqlconnection)
    {
        var sql = $"""
            SELECT n.nid id,
            n.uid access_role_id,
            n.title,
            n.`status` node_status_id,
            FROM_UNIXTIME(n.created) created_date_time, 
            FROM_UNIXTIME(n.changed) changed_date_time,
            nr.body description,
            cc.field_tile_image_fid file_id_tile_image
            FROM node n 
            JOIN category_hierarchy ch ON ch.cid = n.nid
            JOIN node n2 ON n2.nid = ch.parent
                	LEFT JOIN content_type_category_cat cc ON cc.nid = n.nid AND cc.vid = n.vid
                LEFT JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
            WHERE n.`type` = 'region_facts'
            AND n.nid < 30000
            AND n2.`type` = 'category_cont'
            """;

        using var readCommand = mysqlconnection.CreateCommand();
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
                    VocabularyId = TOPICS,
                    Name = name,
                    ParentNames = new List<string>{ "Around the world"},
                }
            };

            yield return new FirstLevelGlobalRegion
            {
                Id = id,
                AccessRoleId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = name,
                NodeStatusId = reader.GetInt32("node_status_id"),
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

