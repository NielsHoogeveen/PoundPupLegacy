using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {

        private static void MigrateSecondLevelGlobalRegions(MySqlConnection mysqlconnection, NpgsqlConnection connection)
        {
            SecondLevelGlobalRegionCreator.Create(ReadSecondLevelGlobalRegion(mysqlconnection), connection);
        }

        private static IEnumerable<SecondLevelGlobalRegion> ReadSecondLevelGlobalRegion(MySqlConnection mysqlconnection)
        {
            var sql = $"""
                SELECT
                n.nid id,
                n.uid user_id,
                n.title,
                n.`status`,
                FROM_UNIXTIME(n.created) created, 
                FROM_UNIXTIME(n.changed) `changed`, 
                n2.nid continent_id
                FROM node n 
                JOIN category_hierarchy ch ON ch.cid = n.nid 
                JOIN node n2 ON n2.nid = ch.parent 
                WHERE n.`type` = 'region_facts'
                AND n.nid < 30000
                AND n2.`type` = 'region_facts'
                """;

            using var readCommand = mysqlconnection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;

            var reader = readCommand.ExecuteReader();

            while (reader.Read())
            {
                yield return new SecondLevelGlobalRegion
                {
                    Id = reader.GetInt32("id"),
                    AccessRoleId = reader.GetInt32("user_id"),
                    CreatedDateTime = reader.GetDateTime("created"),
                    ChangedDateTime = reader.GetDateTime("changed"),
                    Title = reader.GetString("title"),
                    NodeStatusId = reader.GetInt32("status"),
                    NodeTypeId = 12,
                    VocabularyId = 4126,
                    Name = reader.GetString("title"),
                    FirstLevelGlobalRegionId = reader.GetInt32("continent_id")
                };

            }
            reader.Close();
        }

    }
}
