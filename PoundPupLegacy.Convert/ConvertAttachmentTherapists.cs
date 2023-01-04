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
        await AttachmentTherapistCreator.CreateAsync(ReadAttachmentTherapists(mysqlconnection), connection);
    }

    private static IEnumerable<AttachmentTherapist> ReadAttachmentTherapists(MySqlConnection mysqlconnection)
    {

        var sql = $"""
                SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`,
                    25 node_type_id,
                    NULL file_id_portrait,
                    NULL date_of_birth,
                    NULL date_of_death,
                    CASE 
                        WHEN field_long_description_4_value = '' THEN NULL 
                        ELSE field_long_description_4_value 
                    END description,
                    CASE 
                        WHEN n2.nid IS NULL THEN FALSE 
                        ELSE TRUE 
                    END is_topic
                FROM node n 
                JOIN content_type_attachment_therapist t ON t.nid = n.nid
                LEFT JOIN content_type_adopt_orgs o ON o.nid = n.nid AND o.vid = n.vid
                LEFT JOIN node n2 ON n2.title = n.title AND n2.nid <> n.nid
                
                """;
        using var readCommand = mysqlconnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = readCommand.ExecuteReader();

        while (reader.Read())
        {
            yield return new AttachmentTherapist
            {
                Id = reader.GetInt32("id"),
                AccessRoleId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = reader.GetString("title"),
                NodeStatusId = reader.GetInt32("status"),
                NodeTypeId = reader.GetInt16("node_type_id"),
                DateOfBirth = reader.IsDBNull("date_of_birth") ? null : reader.GetDateTime("date_of_birth"),
                DateOfDeath = GetDateOfDeath(reader.GetInt32("id"), reader.IsDBNull("date_of_death") ? null : reader.GetDateTime("date_of_death")),
                FileIdPortrait = reader.IsDBNull("file_id_portrait") ? null : reader.GetInt32("file_id_portrait"),
                Description = reader.IsDBNull("description") ? "" : reader.GetString("description"),
                FileIdTileImage = null,
                VocabularyNames = new List<VocabularyName>(),
            };

        }
        reader.Close();
    }


}
