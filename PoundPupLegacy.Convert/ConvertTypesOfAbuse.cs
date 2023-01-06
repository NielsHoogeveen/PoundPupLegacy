using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{

    private static async Task MigrateTypesOfAbuse(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        await using var tx = await connection.BeginTransactionAsync();
        try
        {
            await TypeOfAbuseCreator.CreateAsync(ReadTypesOfAbuse(mysqlconnection), connection);
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
    }
    private static async IAsyncEnumerable<TypeOfAbuse> ReadTypesOfAbuse(MySqlConnection mysqlconnection)
    {

        var sql = $"""
                SELECT
                t.id,
                1 access_role_id,
                NOW() created_date_time,
                NOW() changed_date_time,
                t.title,
                1 node_status_id,
                39 node_type_id,
                case 
                  when nr.body IS NULL then ''
                  ELSE nr.body
                	end description,
                t.topic_name,
                t.first_parent_topic_name,
                t.second_parent_topic_name,
                case 
                when cc.field_tile_image_fid = 0 then null
                ELSE cc.field_tile_image_fid 
                end file_id_tile_image
                FROM(
                SELECT 118 AS id, 'Non-lethal physical abuse' AS title, 'non-lethal physical abuse' AS topic_name, 'physical abuse' AS first_parent_topic_name, NULL AS second_parent_topic_name
                UNION
                SELECT 119, 'Lethal physical abuse', 'lethal physical abuse', 'physical abuse', 'lethal abuse'
                UNION
                SELECT 120, 'Physical exploitation', 'physical exploitation', 'exploitation', null
                UNION
                SELECT 121, 'Sexual abuse', 'sexual abuse', 'Child abuse forms', null
                UNION
                SELECT 122, 'Sexual exploitation', 'sexual exploitation', 'exploitation', 'sexual abuse'
                UNION
                SELECT 123, 'Non-lethal neglect', 'non-lethal neglect' , 'neglect', null
                UNION
                SELECT 124, 'Lethal neglect', 'lethal neglect' , 'lethal abuse', 'neglect'
                UNION
                SELECT 125, 'Non-lethal deprivation', 'non-lethal deprivation' , 'deprivation', null
                UNION
                SELECT 126, 'Lethal deprivation', 'lethal deprivation' , 'lethal abuse', 'deprivation'
                UNION
                SELECT 127, 'Economic exploitation', 'economic exploitation' , 'exploitation', null
                UNION
                SELECT 128, 'Verbal abuse', 'verbal abuse' , 'emotional abuse', NULL
                UNION
                SELECT 129, 'Medical abuse', 'medical abuse' , 'Child abuse forms', NULL
                UNION
                SELECT 130, 'Death by unknown cause', NULL, NULL , null
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
            var firstParentTopicName = reader.IsDBNull("first_parent_topic_name") ? null : reader.GetString("first_parent_topic_name");
            var secondParentTopicName = reader.IsDBNull("second_parent_topic_name") ? null : reader.GetString("second_parent_topic_name");

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
                if (firstParentTopicName != null)
                {
                    lst.Add(firstParentTopicName);
                }
                if (secondParentTopicName != null)
                {
                    lst.Add(secondParentTopicName);
                }

                vocabularyNames.Add(new VocabularyName
                {
                    VocabularyId = TOPICS,
                    Name = topicName,
                    ParentNames = lst
                });
            }

            yield return new TypeOfAbuse
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
