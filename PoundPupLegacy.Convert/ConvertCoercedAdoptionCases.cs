using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{


    private static void MigrateCoercedAdoptionCases(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        CoercedAdoptionCaseCreator.Create(ReadCoercedAdoptionCases(mysqlconnection), connection);
    }
    private static IEnumerable<CoercedAdoptionCase> ReadCoercedAdoptionCases(MySqlConnection mysqlconnection)
    {

        var sql = $"""
                SELECT
                     n.nid id,
                     n.uid user_id,
                     n.title,
                     n.`status`,
                     FROM_UNIXTIME(n.created) created, 
                     FROM_UNIXTIME(n.changed) `changed`,
                     30 node_type_id,
                     cc.nid IS NOT null is_topic,
                     field_long_description_3_value description,
                     field_reporting_date_0_value `date`
                FROM node n
                JOIN content_type_coerced_adoption_cases c ON c.nid = n.nid AND c.vid = n.vid
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
            var country = new CoercedAdoptionCase
            {
                Id = reader.GetInt32("id"),
                UserId = reader.GetInt32("user_id"),
                Created = reader.GetDateTime("created"),
                Changed = reader.GetDateTime("changed"),
                Title = reader.GetString("title"),
                Status = reader.GetInt32("status"),
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
