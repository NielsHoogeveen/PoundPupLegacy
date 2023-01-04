using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {

        private static async Task MigrateTypesOfAbusers(MySqlConnection mysqlconnection, NpgsqlConnection connection)
        {
            await TypeOfAbuserCreator.CreateAsync(ReadTypesOfAbusers(mysqlconnection), connection);
        }
        private static async IAsyncEnumerable<TypeOfAbuser> ReadTypesOfAbusers(MySqlConnection mysqlconnection)
        {

            var sql = $"""
                SELECT
                t.id,
                1 access_role_id,
                NOW() created_date_time,
                NOW() changed_date_time,
                t.title,
                1 node_status_id,
                40 node_type_id,
                case 
                  when nr.body IS NULL then ''
                  ELSE nr.body
                	end description,
                t.topic_name,
                t.parent_topic_name,
                case 
                when cc.field_tile_image_fid = 0 then null
                ELSE cc.field_tile_image_fid 
                end file_id_tile_image
                FROM(
                SELECT 131 AS id, 'Adoptive father' AS title, 'adoptive father' AS topic_name, 'adoptive parents' AS parent_topic_name
                UNION
                SELECT 132, 'Foster father', 'foster father', 'foster parents'
                UNION
                SELECT 133, 'Adoptive mother', 'adoptive mother', NULL
                UNION
                SELECT 134, 'Foster mother', 'foster mother', 'foster parents'
                UNION
                SELECT 135, 'Legal guardian', 'legal guardian', NULL
                UNION
                SELECT 136, 'Adopted sibling', NULL , NULL
                UNION
                SELECT 137, 'Foster sibling', NULL , null
                UNION
                SELECT 138, 'Non-adopted sibling', NULL , NULL
                UNION
                SELECT 139, 'Non-fostered sibling', NULL , NULL
                UNION
                SELECT 140, 'Other family member', NULL , NULL
                UNION
                SELECT 141, 'Other non-family member', NULL , NULL
                UNION
                SELECT 142, 'Undetermined', NULL , null
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


            var reader = await readCommand.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var id = reader.GetInt32("id");
                var name = reader.GetString("title");
                var topicName = reader.IsDBNull("topic_name") ? null : reader.GetString("topic_name");
                var parentTopicName = reader.IsDBNull("parent_topic_name") ? null : reader.GetString("parent_topic_name");

                var vocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        VocabularyId = TYPE_OF_ABUSE,
                        Name = name,
                        ParentNames = new List<string>(),
                    }
                };
                if (topicName != null)
                {
                    var lst = new List<string>();
                    if (parentTopicName != null)
                    {
                        lst.Add(parentTopicName);
                    }

                    vocabularyNames.Add(new VocabularyName
                    {
                        VocabularyId = TOPICS,
                        Name = topicName,
                        ParentNames = lst
                    });
                }

                yield return new TypeOfAbuser
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
            await reader.CloseAsync();
        }

    }
}
