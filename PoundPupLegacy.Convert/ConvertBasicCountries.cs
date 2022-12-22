using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{

    private static IEnumerable<BasicCountry> basicCountries = new List<BasicCountry>
    {
        new BasicCountry
        {
            Id = 4073,
            UserId = 1,
            Created = DateTime.Now,
            Changed = DateTime.Now,
            Title = "Antigua and Barbuda",
            Status = 1,
            NodeTypeId = 13,
            IsTerm = true,
            GlobalRegionId = 3822,
            ISO3166_1_Code = "AG"
        },
        new BasicCountry
        {
            Id = 4082,
            UserId = 1,
            Created = DateTime.Now,
            Changed = DateTime.Now,
            Title = "Palestine",
            Status = 1,
            NodeTypeId = 13,
            IsTerm = true,
            GlobalRegionId = 3817,
            ISO3166_1_Code = "PS"
        },
        new BasicCountry
        {
            Id = 4087,
            UserId = 1,
            Created = DateTime.Now,
            Changed = DateTime.Now,
            Title = "Saint Helena, Ascension and Tristan da Cunha",
            Status = 1,
            NodeTypeId = 13,
            IsTerm = true,
            GlobalRegionId = 3825,
            ISO3166_1_Code = "SH"
        },
        new BasicCountry
        {
            Id = 4093,
            UserId = 1,
            Created = DateTime.Now,
            Changed = DateTime.Now,
            Title = "South Sudan",
            Status = 1,
            NodeTypeId = 13,
            IsTerm = true,
            GlobalRegionId = 3827,
            ISO3166_1_Code = "SS"
        }
    };

    private static void MigrateBasicCountries(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        var countries = basicCountries.Select(x =>
        {
            if (x.Id == 0)
            {
                NodeId++;
                x.Id = NodeId;
            }
            return x;
        });
        BasicCountryCreator.Create(countries, connection);
        BasicCountryCreator.Create(ReadBasicCountries(mysqlconnection), connection);
    }
    private static IEnumerable<BasicCountry> ReadBasicCountries(MySqlConnection mysqlconnection)
    {

        var sql = $"""
                SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`, 
                    n2.nid continental_region_id,
                    upper(cou.field_country_code_value) iso_3166_code
                FROM node n 
                JOIN content_type_country_type cou ON cou.nid = n.nid 
                JOIN category_hierarchy ch ON ch.cid = n.nid 
                JOIN node n2 ON n2.nid = ch.parent 
                WHERE n.`type` = 'country_type' 
                AND n2.`type` = 'region_facts' 
                AND n.nid not IN (
                    3980,
                    3981,
                    3935,
                    3903,
                    3908,
                    4044,
                    4057,
                    3887,
                    3879,
                    4063,
                    3878,
                    3891,
                    4055,
                    4048,
                    4053,
                    3914,
                    3920,
                    4000,
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
            var country = new BasicCountry
            {
                Id = reader.GetInt32("id"),
                UserId = reader.GetInt32("user_id"),
                Created = reader.GetDateTime("created"),
                Changed = reader.GetDateTime("changed"),
                Title = reader.GetInt32("id") == 3839 ? "Côte d'Ivoire" :
                        reader.GetInt32("id") == 3999 ? "Bosnia and Herzegovina" :
                        reader.GetInt32("id") == 4005 ? "North Macedonia" :
                        reader.GetInt32("id") == 4046 ? "Solomon Islands" :
                        reader.GetInt32("id") == 3884 ? "Eswatini" :
                        reader.GetString("title"),
                Status = reader.GetInt32("status"),
                NodeTypeId = 13,
                IsTerm = true,
                GlobalRegionId = reader.GetInt32("continental_region_id"),
                ISO3166_1_Code = reader.GetInt32("id") == 3847 ? "NE" :
                              reader.GetInt32("id") == 4010 ? "RS" :
                              reader.GetInt32("id") == 4014 ? "XK" :
                              reader.GetString("iso_3166_code")
            };
            yield return country;

        }
        reader.Close();
    }


}
