using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{


    private static async Task MigrateAttachmentTherapists(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        await using var tx = await connection.BeginTransactionAsync();
        try
        {
            await PersonCreator.CreateAsync(ReadAttachmentTherapists(mysqlconnection), connection);
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }

        
    }

    private static async IAsyncEnumerable<Person> ReadAttachmentTherapists(MySqlConnection mysqlconnection)
    {

        var sql = $"""
                 SELECT
                  n.nid id,
                  n.uid access_role_id,
                  n.title,
                  n.`status` node_status_id,
                  FROM_UNIXTIME(n.created) created_date_time, 
                  FROM_UNIXTIME(n.changed) changed_date_time,
                  24 node_type_id,
                  NULL file_id_portrait,
                  NULL date_of_birth,
                  NULL date_of_death,
                  CASE 
                      WHEN field_long_description_4_value = '' THEN NULL 
                      ELSE field_long_description_4_value 
                  END description,
                c.title topic_name,
                c.parent_name
                FROM node n 
                JOIN content_type_attachment_therapist t ON t.nid = n.nid
                LEFT JOIN content_type_adopt_orgs o ON o.nid = n.nid AND o.vid = n.vid
                LEFT JOIN node n2 ON n2.title = n.title AND n2.nid <> n.nid
                LEFT JOIN (
                    select
                    n.nid,
                    n.title,
                    cc.field_tile_image_title,
                    cc.field_related_page_nid,
                    p.nid parent_id,
                    p.title parent_name
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
                ) c ON c.field_related_page_nid = n.nid
                WHERE c.title IS NOT NULL
                
                """;
        using var readCommand = mysqlconnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var id = reader.GetInt32( "id" );
            var topicName = reader.GetString("topic_name");
            var parentName = reader.IsDBNull("parent_name")? null : reader.GetString("parent_name");
            var parentNames = new List<string>();
            if(parentName is not null)
            {
                parentNames.Add(parentName);
            }
            var vocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    VocabularyId = TOPICS,
                    Name = topicName,
                    ParentNames = parentNames,
                }
            };

            yield return new Person
            {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = reader.GetString("title"),
                OwnerId = null,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        TenantId = 1,
                        PublicationStatusId = reader.GetInt32("node_status_id"),
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                NodeTypeId = reader.GetInt16("node_type_id"),
                DateOfBirth = reader.IsDBNull("date_of_birth") ? null : reader.GetDateTime("date_of_birth"),
                DateOfDeath = GetDateOfDeath(reader.GetInt32("id"), reader.IsDBNull("date_of_death") ? null : reader.GetDateTime("date_of_death")),
                FileIdPortrait = reader.IsDBNull("file_id_portrait") ? null : reader.GetInt32("file_id_portrait"),
                Description = reader.IsDBNull("description") ? "" : reader.GetString("description"),
                FileIdTileImage = null,
                VocabularyNames = vocabularyNames,
            };

        }
        await reader.CloseAsync();
    }


}
