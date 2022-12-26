using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{


    private static void MigrateAttachmentTherapists(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        AttachmentTherapistCreator.Create(ReadAttachmentTherapists(mysqlconnection), connection);
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
                TRUE is_topic,
                NULL file_id_portrait,
                NULL date_of_birth,
                NULL date_of_death,
                case when field_long_description_4_value = '' then NULL ELSE field_long_description_4_value end description
                FROM node n 
                JOIN content_type_attachment_therapist t ON t.nid = n.nid
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
                UserId = reader.GetInt32("user_id"),
                Created = reader.GetDateTime("created"),
                Changed = reader.GetDateTime("changed"),
                Title = reader.GetString("title"),
                Status = reader.GetInt32("status"),
                NodeTypeId = reader.GetInt16("node_type_id"),
                IsTerm = reader.GetBoolean("is_topic"),
                DateOfBirth = reader.IsDBNull("date_of_birth") ? null : reader.GetDateTime("date_of_birth"),
                DateOfDeath = GetDateOfDeath(reader.GetInt32("id"), reader.IsDBNull("date_of_death") ? null : reader.GetDateTime("date_of_death")),
                FileIdPortrait = reader.IsDBNull("file_id_portrait") ? null : reader.GetInt32("file_id_portrait"),
                Description = reader.IsDBNull("description") ? null : reader.GetString("description"),
            };

        }
        reader.Close();
    }


}
