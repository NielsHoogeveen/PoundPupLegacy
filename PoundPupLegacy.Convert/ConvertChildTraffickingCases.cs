using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{


    private static void MigrateChildTraffickingCases(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        ChildTraffickingCaseCreator.Create(ReadChildTraffickingCases(mysqlconnection), connection);
    }
    private static IEnumerable<ChildTraffickingCase> ReadChildTraffickingCases(MySqlConnection mysqlconnection)
    {

        var sql = $"""
                SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`,
                    29 node_type_id,
                    cc.nid IS NOT null is_topic,
                    field_description_7_value description,
                    field_discovery_date_0_value `date`,
                    field_number_of_children_value number_of_children_involved,
                    field_country_from_nid country_id_from
                FROM node n
                JOIN content_type_child_trafficking_case c ON c.nid = n.nid AND c.vid = n.vid
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
            var country = new ChildTraffickingCase
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
                NumberOfChildrenInvolved = reader.IsDBNull("number_of_children_involved") ? null : reader.GetInt32("number_of_children_involved"),
                CountryIdFrom = reader.GetInt32("country_id_from"),
            };
            yield return country;

        }
        reader.Close();
    }
}
