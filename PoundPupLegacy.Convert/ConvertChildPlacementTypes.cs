using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {

        private static void MigrateChildPlacementTypes(MySqlConnection mysqlconnection, NpgsqlConnection connection)
        {
            ChildPlacementTypeCreator.Create(ReadChildPlacementTypes(mysqlconnection), connection);
        }

        private static IEnumerable<ChildPlacementType> ReadChildPlacementTypes(MySqlConnection mysqlconnection)
        {

            var sql = $"""
                SELECT
                t.id,
                1 access_role_id,
                NOW() created_date_time,
                NOW() changed_date_time,
                t.title,
                1 node_status_id,
                27 node_type_id,
                case 
                    when nr.body IS NULL then ''
                    ELSE nr.body
                		end description,
                t.topic_name,
                n.nid,
                case 
                	when cc.field_tile_image_fid = 0 then null
                	ELSE cc.field_tile_image_fid 
                end file_id_tile_image
                FROM(
                SELECT 106 AS id, 'Adoption' AS title, 'adoption' AS topic_name
                UNION
                SELECT 107, 'Foster Care', 'foster care' 
                UNION
                SELECT 108, 'To be adopted', NULL
                UNION
                SELECT 109, 'Legal Guardianship', 'guardianship'
                UNION
                SELECT 110, 'Institution', 'residential care'
                ) AS t
                LEFT JOIN node n ON n.title = t.topic_name AND n.`type` = 'category_cat'
                LEFT JOIN category c ON c.cid = n.nid AND c.cnid = 4126
                LEFT JOIN content_type_category_cat cc on cc.nid = n.nid AND cc.vid = n.vid
                LEFT JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
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
                var topicName = reader.IsDBNull("topic_name") ? null : reader.GetString("topic_name");

                var vocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        VocabularyId = CHILD_PLACEMENT_TYPE,
                        Name = name,
                        ParentNames = new List<string>(),
                    }
                };
                if (topicName != null)
                {
                    vocabularyNames.Add(new VocabularyName
                    {
                        VocabularyId = TOPICS,
                        Name = topicName,
                        ParentNames = new List<string>()
                    });
                }

                yield return new ChildPlacementType
                {
                    Id = id,
                    AccessRoleId = reader.GetInt32("access_role_id"),
                    CreatedDateTime = reader.GetDateTime("created_date_time"),
                    ChangedDateTime = reader.GetDateTime("changed_date_time"),
                    Title = name,
                    NodeStatusId = reader.GetInt32("node_status_id"),
                    NodeTypeId = reader.GetInt32("node_type_id"),
                    Description = reader.GetString("description"),
                    FileIdTileImage = reader.IsDBNull("file_id_tile_image") ? null : reader.GetInt32("file_id_tile_image"),
                    VocabularyNames = vocabularyNames,
                };

            }
            reader.Close();
        }

    }
}
