using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {

        private static async Task MigratePoliticalEntityRelationTypes(MySqlConnection mysqlconnection, NpgsqlConnection connection)
        {
            await PoliticalEntityRelationTypeCreator.CreateAsync(ReadPoliticalEntityRelationTypes(mysqlconnection), connection);
        }
        private static async IAsyncEnumerable<PoliticalEntityRelationType> ReadPoliticalEntityRelationTypes(MySqlConnection mysqlconnection)
        {

            var sql = $"""
                    SELECT
                        n.nid id,
                        n.uid access_role_id,
                        n.title,
                        n.`status` node_status_id,
                        FROM_UNIXTIME(n.created) created_date_time, 
                        FROM_UNIXTIME(n.changed) changed_date_time,
                        '' description,
                        NULL file_id_tile_image,
                    	case 
                            when n.nid IN (12662, 12660) then true
                            ELSE false
                        END has_concrete_subtype
                    FROM node n
                    JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                    JOIN category c ON c.cid = n.nid AND c.cnid = 12652
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
                        VocabularyId = 12652,
                        Name = name,
                        ParentNames = new List<string>(),
                    }
                };

                yield return new PoliticalEntityRelationType
                {
                    Id = id,
                    AccessRoleId = reader.GetInt32("access_role_id"),
                    CreatedDateTime = reader.GetDateTime("created_date_time"),
                    ChangedDateTime = reader.GetDateTime("changed_date_time"),
                    Title = name,
                    NodeStatusId = reader.GetInt32("node_status_id"),
                    NodeTypeId = 3,
                    Description = reader.GetString("description"),
                    FileIdTileImage = reader.IsDBNull("file_id_tile_image") ? null : reader.GetInt32("file_id_tile_image"),
                    HasConcreteSubtype = reader.GetBoolean("has_concrete_subtype"),
                    VocabularyNames = vocabularyNames,
                };

            }
            await reader.CloseAsync();
        }
    }
}
