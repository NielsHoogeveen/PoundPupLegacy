using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {
        private static void MigrateFirstLevelGlobalRegions(MySqlConnection mysqlconnection, NpgsqlConnection connection)
        {
            FirstLevelGlobalRegionCreator.Create(ReadFirstLevelGlobalRegions(mysqlconnection), connection);
        }

        private static IEnumerable<FirstLevelGlobalRegion> ReadFirstLevelGlobalRegions(MySqlConnection mysqlconnection)
        {
            var sql = $"""
                SELECT n.nid id,
                n.uid user_id,
                n.title,
                n.`status`,
                FROM_UNIXTIME(n.created) created, 
                FROM_UNIXTIME(n.changed) `changed`
                FROM node n 
                JOIN category_hierarchy ch ON ch.cid = n.nid
                JOIN node n2 ON n2.nid = ch.parent
                WHERE n.`type` = 'region_facts'
                AND n.nid < 30000
                AND n2.`type` = 'category_cont'
                """;

            using var readCommand = mysqlconnection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;

            var reader = readCommand.ExecuteReader();

            while (reader.Read())
            {
                var id = reader.GetInt32("id");
                var name = reader.GetString("title");
                yield return new FirstLevelGlobalRegion
                {
                    Id = id,
                    AccessRoleId = reader.GetInt32("user_id"),
                    CreatedDateTime = reader.GetDateTime("created"),
                    ChangedDateTime = reader.GetDateTime("changed"),
                    Title = name,
                    NodeStatusId = reader.GetInt32("status"),
                    NodeTypeId = 11,
                    VocabularyNames = GetVocabularyNames(TOPICS, id, name, new Dictionary<int, List<VocabularyName>>()),
                    Description = "",
                    Name = name,
                };
            }
            reader.Close();
        }
    }
}

