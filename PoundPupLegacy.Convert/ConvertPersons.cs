using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{


    private static void MigratePersons(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        PersonCreator.Create(ReadPersons(mysqlconnection), connection);
    }
    private static DateTime? GetDateOfDeath(int id, DateTime? dateTime)
    {
        return id switch
        {
            60412 => DateTime.Parse("2022-03-18"),
            10329 => DateTime.Parse("2018-08-25"),
            _ => dateTime
        };
    }

    private static IEnumerable<BasicPerson> ReadPersons(MySqlConnection mysqlconnection)
    {

        var sql = $"""
                SELECT
                n.nid id,
                n.uid user_id,
                n.title,
                n.`status`,
                FROM_UNIXTIME(n.created) created, 
                FROM_UNIXTIME(n.changed) `changed`,
                24 node_type_id,
                CASE WHEN n2.nid IS NULL THEN FALSE ELSE TRUE END is_topic,
                CASE WHEN o.field_image_fid = 0 THEN null ELSE o.field_image_fid END file_id_portrait,
                STR_TO_DATE(field_born_value,'%Y-%m-%d') date_of_birth,
                STR_TO_DATE(field_died_value,'%Y-%m-%d') date_of_death
                FROM node n 
                JOIN content_type_adopt_person o ON o.nid = n.nid AND o.vid = n.vid
                LEFT JOIN node n2 ON n2.title = n.title AND n2.nid <> n.nid AND n2.`type` = 'category_cat'
                WHERE n.`type` = 'adopt_person'
                """;
        using var readCommand = mysqlconnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = readCommand.ExecuteReader();

        while (reader.Read())
        {
            yield return new BasicPerson
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
            };

        }
        reader.Close();
    }


}
