using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{


    private static void MigrateBindingCountries(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        BindingCountryCreator.Create(ReadBindingCountries(mysqlconnection), connection);
    }
    private static IEnumerable<BindingCountry> ReadBindingCountries(MySqlConnection mysqlconnection)
    {

        var sql = $"""
                SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`, 
                    n2.nid global_region_id,
                    upper(cou.field_country_code_value) iso_3166_1_code
                FROM node n 
                JOIN content_type_country_type cou ON cou.nid = n.nid 
                JOIN category_hierarchy ch ON ch.cid = n.nid 
                JOIN node n2 ON n2.nid = ch.parent 
                WHERE n.`type` = 'country_type' 
                AND n2.`type` = 'region_facts' 
                AND n.nid IN (
                    3992
                )
                """;
        using var readCommand = mysqlconnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = readCommand.ExecuteReader();

        while (reader.Read())
        {
            var country = new BindingCountry
            {
                Id = reader.GetInt32("id"),
                UserId = reader.GetInt32("user_id"),
                Created = reader.GetDateTime("created"),
                Changed = reader.GetDateTime("changed"),
                Title = reader.GetString("title"),
                Status = reader.GetInt32("status"),
                NodeTypeId = 20,
                IsTerm = true,
                GlobalRegionId = reader.GetInt32("global_region_id"),
                ISO3166_1_Code = reader.GetString("iso_3166_1_code"),
                FileIdFlag = null,
            };
            yield return country;

        }
        reader.Close();
    }


}
