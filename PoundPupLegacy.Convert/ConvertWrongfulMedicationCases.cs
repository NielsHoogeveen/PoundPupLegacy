using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{


    private static void MigrateWrongfulMedicationCases(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        WrongfulMedicationCaseCreator.Create(ReadWrongfulMedicationCases(mysqlconnection), connection);
    }
    private static IEnumerable<WrongfulMedicationCase> ReadWrongfulMedicationCases(MySqlConnection mysqlconnection)
    {

        var sql = $"""
                SELECT
                     n.nid id,
                     n.uid user_id,
                     n.title,
                     n.`status`,
                     FROM_UNIXTIME(n.created) created, 
                     FROM_UNIXTIME(n.changed) `changed`,
                     33 node_type_id,
                     cc.nid IS NOT null is_topic,
                     field_long_description_1_value description,
                     field_report_date_0_value `date`
                FROM node n
                JOIN content_type_wrongful_medication_case c ON c.nid = n.nid AND c.vid = n.vid
                LEFT JOIN content_type_category_cat cc ON cc.field_related_page_nid = n.nid 
                LEFT JOIN node n2 ON n2.nid = cc.nid AND n2.vid = cc.vid
                """;
        using var readCommand = mysqlconnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = readCommand.ExecuteReader();

        while (reader.Read())
        {
            var country = new WrongfulMedicationCase
            {
                Id = reader.GetInt32("id"),
                AccessRoleId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = reader.GetString("title"),
                NodeStatusId = reader.GetInt32("status"),
                NodeTypeId = reader.GetInt32("node_type_id"),
                IsTopic = reader.GetBoolean("is_topic"),
                Date = reader.IsDBNull("date") ? null : StringToDateTimeRange(reader.GetString("date")),
                Description = reader.GetString("description"),
            };
            yield return country;

        }
        reader.Close();
    }
}
