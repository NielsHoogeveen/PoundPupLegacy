using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {

        private static async Task MigratePersonOrganizationRelationTypes(MySqlConnection mysqlconnection, NpgsqlConnection connection)
        {
            await PersonOrganizationRelationTypeCreator.CreateAsync(ReadPersonOrganizationRelationTypes(mysqlconnection), connection);
        }
        private static async IAsyncEnumerable<PersonOrganizationRelationType> ReadPersonOrganizationRelationTypes(MySqlConnection mysqlconnection)
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
                    NULL file_id_tile_image
                    FROM node n
                    JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                    JOIN category c ON c.cid = n.nid AND c.cnid = 12663
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
                        VocabularyId = 12663,
                        Name = name,
                        ParentNames = new List<string>(),
                    }
                };

                yield return new PersonOrganizationRelationType
                {
                    Id = id,
                    AccessRoleId = reader.GetInt32("access_role_id"),
                    CreatedDateTime = reader.GetDateTime("created_date_time"),
                    ChangedDateTime = reader.GetDateTime("changed_date_time"),
                    Title = name,
                    NodeStatusId = reader.GetInt32("node_status_id"),
                    NodeTypeId = 4,
                    Description = reader.GetString("description"),
                    FileIdTileImage = reader.IsDBNull("file_id_tile_image") ? null : reader.GetInt32("file_id_tile_image"),
                    VocabularyNames = vocabularyNames,
                    HasConcreteSubtype = false,
                };

            }
            await reader.CloseAsync();
        }
    }
}
