using MySqlConnector;
using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {

        private static List<BasicCountryAndFirstLevelSubdivision> RegionSubdivisionCountries = new List<BasicCountryAndFirstLevelSubdivision>
        {
            new BasicCountryAndFirstLevelSubdivision
            {
                Id = 0,
                Title = "Saint Barthélemy",
                Name = "Saint Barthélemy",
                Status = 1,
                NodeTypeId = 16,
                Created = DateTime.Now,
                Changed = DateTime.Now,
                UserId = 1,
                IsTerm = true,
                GlobalRegionId = 3809,
                CountryId = 4018,
                ISO3166_1_Code = "BL",
                ISO3166_2_Code = "FR-BL"
            },
            new BasicCountryAndFirstLevelSubdivision
            {
                Id = 0,
                Title = "Saint Martin",
                Name = "Saint Martin",
                Status = 1,
                NodeTypeId = 16,
                Created = DateTime.Now,
                Changed = DateTime.Now,
                UserId = 1,
                IsTerm = true,
                GlobalRegionId = 3809,
                CountryId = 4018,
                ISO3166_1_Code = "MF",
                ISO3166_2_Code = "FR-MF"
            },
            new BasicCountryAndFirstLevelSubdivision
            {
                Id = 0,
                Title = "French Southern Territories",
                Name = "French Southern Territories",
                Status = 1,
                NodeTypeId = 15,
                Created = DateTime.Now,
                Changed = DateTime.Now,
                UserId = 1,
                IsTerm = true,
                GlobalRegionId = 3828,
                CountryId = 4018,
                ISO3166_1_Code = "TF",
                ISO3166_2_Code = "FR-TF"
            },

        };

        private static void MigrateRegionSubdivisionCountries(MySqlConnection mysqlconnection, NpgsqlConnection postgresqlconnection)
        {


            var sql = $"SELECT\r\nn.nid id,\r\nn.uid user_id,\r\nn.title,\r\nn.`status`,\r\nFROM_UNIXTIME(n.created) created, \r\nFROM_UNIXTIME(n.changed) `changed`,\r\nn2.nid continental_region_id,\r\nupper(cou.field_country_code_value) iso_3166_code\r\nFROM node n \r\nJOIN content_type_country_type cou ON cou.nid = n.nid\r\nJOIN category_hierarchy ch ON ch.cid = n.nid\r\nJOIN node n2 ON n2.nid = ch.parent\r\nWHERE n.`type` = 'country_type'\r\nAND n2.`type` = 'region_facts'\r\nAND n.nid IN (\r\n3935,\r\n3903,\r\n3908,\r\n4044,\r\n4057,\r\n3887,\r\n3879,\r\n4063,\r\n3878)";
            using var readCommand = mysqlconnection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;

            using var writeCommand = postgresqlconnection.CreateCommand();
            writeCommand.CommandType = CommandType.Text;
            writeCommand.CommandTimeout = 300;
            writeCommand.CommandText = """INSERT INTO public."node" (id, user_id, created, changed, title, status, node_type_id, is_term) VALUES(@id, @user_id, @created, @changed, @title, @status, @node_type_id, @is_term)""";
            writeCommand.Parameters.Add("id", NpgsqlDbType.Integer);
            writeCommand.Parameters.Add("user_id", NpgsqlDbType.Integer);
            writeCommand.Parameters.Add("created", NpgsqlDbType.Timestamp);
            writeCommand.Parameters.Add("changed", NpgsqlDbType.Timestamp);
            writeCommand.Parameters.Add("title", NpgsqlDbType.Varchar);
            writeCommand.Parameters.Add("status", NpgsqlDbType.Integer);
            writeCommand.Parameters.Add("node_type_id", NpgsqlDbType.Integer);
            writeCommand.Parameters.Add("is_term", NpgsqlDbType.Boolean);
            writeCommand.Prepare();

            using var writeCommand2a = postgresqlconnection.CreateCommand();
            writeCommand2a.CommandType = CommandType.Text;
            writeCommand2a.CommandTimeout = 300;
            writeCommand2a.CommandText = $"""INSERT INTO public."continental_region_country" (id, continental_region_id) VALUES(@id, @continental_region_id)""";
            writeCommand2a.Parameters.Add("id", NpgsqlDbType.Integer);
            writeCommand2a.Parameters.Add("continental_region_id", NpgsqlDbType.Integer);

            using var writeCommand2b = postgresqlconnection.CreateCommand();
            writeCommand2b.CommandType = CommandType.Text;
            writeCommand2b.CommandTimeout = 300;
            writeCommand2b.CommandText = $"""INSERT INTO public."continental_country" (id, continent_id) VALUES(@id, @continent_id)""";
            writeCommand2b.Parameters.Add("id", NpgsqlDbType.Integer);
            writeCommand2b.Parameters.Add("continent_id", NpgsqlDbType.Integer);

            using var writeCommand3 = postgresqlconnection.CreateCommand();
            writeCommand3.CommandType = CommandType.Text;
            writeCommand3.CommandTimeout = 300;
            writeCommand3.CommandText = $"""INSERT INTO public."term_hierarchy" (term_id_child, term_id_parent) VALUES(@term_id_child, @term_id_parent)""";
            writeCommand3.Parameters.Add("term_id_child", NpgsqlDbType.Integer);
            writeCommand3.Parameters.Add("term_id_parent", NpgsqlDbType.Integer);

            using var writeCommand4 = postgresqlconnection.CreateCommand();
            writeCommand4.CommandType = CommandType.Text;
            writeCommand4.CommandTimeout = 300;
            writeCommand4.CommandText = $"""INSERT INTO public."political_entity" (id) VALUES(@id)""";
            writeCommand4.Parameters.Add("id", NpgsqlDbType.Integer);

            using var writeCommand5 = postgresqlconnection.CreateCommand();
            writeCommand5.CommandType = CommandType.Text;
            writeCommand5.CommandTimeout = 300;
            writeCommand5.CommandText = $"""INSERT INTO public."country" (id) VALUES(@id)""";
            writeCommand5.Parameters.Add("id", NpgsqlDbType.Integer);

            using var writeCommand6 = postgresqlconnection.CreateCommand();
            writeCommand6.CommandType = CommandType.Text;
            writeCommand6.CommandTimeout = 300;
            writeCommand6.CommandText = $"""INSERT INTO public."top_level_country" (id, iso_3166_code) VALUES(@id, @iso_3166_code)""";
            writeCommand6.Parameters.Add("id", NpgsqlDbType.Integer);
            writeCommand6.Parameters.Add("iso_3166_code", NpgsqlDbType.Char);

            using var writeCommand7 = postgresqlconnection.CreateCommand();
            writeCommand7.CommandType = CommandType.Text;
            writeCommand7.CommandTimeout = 300;
            writeCommand7.CommandText = $"""INSERT INTO public."country_subdivision" (id,iso_3166_2_code) VALUES(@id, @iso_3166_2_code)""";
            writeCommand7.Parameters.Add("id", NpgsqlDbType.Integer);
            writeCommand7.Parameters.Add("iso_3166_2_code", NpgsqlDbType.Varchar);


            using var writeCommand9 = postgresqlconnection.CreateCommand();
            writeCommand9.CommandType = CommandType.Text;
            writeCommand9.CommandTimeout = 300;
            writeCommand9.CommandText = $"""INSERT INTO public."country_direct_subdivision" (id,country_id) VALUES(@id, @country_id)""";
            writeCommand9.Parameters.Add("id", NpgsqlDbType.Integer);
            writeCommand9.Parameters.Add("country_id", NpgsqlDbType.Integer);

            using var writeCommand10 = postgresqlconnection.CreateCommand();
            writeCommand10.CommandType = CommandType.Text;
            writeCommand10.CommandTimeout = 300;
            writeCommand10.CommandText = $"""INSERT INTO public."country_region" (id, country_id) VALUES(@id, @country_id)""";
            writeCommand10.Parameters.Add("id", NpgsqlDbType.Integer);
            writeCommand10.Parameters.Add("country_id", NpgsqlDbType.Integer);

            using var writeCommand11 = postgresqlconnection.CreateCommand();
            writeCommand11.CommandType = CommandType.Text;
            writeCommand11.CommandTimeout = 300;
            writeCommand11.CommandText = $"""INSERT INTO public."country_region_political_entity" (id) VALUES(@id)""";
            writeCommand11.Parameters.Add("id", NpgsqlDbType.Integer);

            using var writeCommand12 = postgresqlconnection.CreateCommand();
            writeCommand12.CommandType = CommandType.Text;
            writeCommand12.CommandTimeout = 300;
            writeCommand12.CommandText = $"""INSERT INTO public."country_country_subdivision" (id) VALUES(@id)""";
            writeCommand12.Parameters.Add("id", NpgsqlDbType.Integer);

            using var writeCommand8 = postgresqlconnection.CreateCommand();
            writeCommand8.CommandType = CommandType.Text;
            writeCommand8.CommandTimeout = 300;
            writeCommand8.CommandText = $"""INSERT INTO public."country_country_region_subdivision" (id) VALUES(@id)""";
            writeCommand8.Parameters.Add("id", NpgsqlDbType.Integer);


            using var writeCommand13 = postgresqlconnection.CreateCommand();
            writeCommand13.CommandType = CommandType.Text;
            writeCommand13.CommandTimeout = 300;
            writeCommand13.CommandText = $"""INSERT INTO public."country_part_name" (id, name, country_id) VALUES(@id,@name,@country_id)""";
            writeCommand13.Parameters.Add("id", NpgsqlDbType.Integer);
            writeCommand13.Parameters.Add("country_id", NpgsqlDbType.Integer);
            writeCommand13.Parameters.Add("name", NpgsqlDbType.Varchar);


            var reader = readCommand.ExecuteReader();

            foreach (var country in RegionSubdivisionCountries)
            {
                NodeId++;
                country.Id = NodeId;
                WriteRegionSubDivisionCountry(
                    country,
                    writeCommand,
                    writeCommand2a,
                    writeCommand2b,
                    writeCommand3,
                    writeCommand4,
                    writeCommand5,
                    writeCommand6,
                    writeCommand7,
                    writeCommand8,
                    writeCommand9,
                    writeCommand10,
                    writeCommand11,
                    writeCommand12,
                    writeCommand13);
            }

            while (reader.Read())
            {
                var country = new BasicCountryAndFirstLevelSubdivision
                {
                    Id = reader.GetInt32("id"),
                    UserId = reader.GetInt32("user_id"),
                    Created = reader.GetDateTime("created"),
                    Changed = reader.GetDateTime("changed"),
                    Title = reader.GetInt32("id") == 3879 ? "Réunion" :
                            reader.GetString("title"),
                    Name = reader.GetInt32("id") == 3879 ? "Réunion" :
                            reader.GetString("title"),
                    Status = reader.GetInt32("status"),
                    NodeTypeId = 16,
                    IsTerm = true,
                    GlobalRegionId = reader.GetInt32("continental_region_id"),
                    ISO3166_1_Code = reader.GetInt32("id") == 3847 ? "NE" :
                                  reader.GetInt32("id") == 4010 ? "RS" :
                                  reader.GetInt32("id") == 4014 ? "XK" :
                                  reader.GetString("iso_3166_code"),
                    ISO3166_2_Code = GetISO3166Code2ForCountry(reader.GetInt32("id")),
                    CountryId = GetSupervisingCountryId(reader.GetInt32("id")),

                };
                WriteRegionSubDivisionCountry(
                    country,
                    writeCommand,
                    writeCommand2a,
                    writeCommand2b,
                    writeCommand3,
                    writeCommand4,
                    writeCommand5,
                    writeCommand6,
                    writeCommand7,
                    writeCommand8,
                    writeCommand9,
                    writeCommand10,
                    writeCommand11,
                    writeCommand12,
                    writeCommand13);
            }
            reader.Close();
        }
        private static void WriteRegionSubDivisionCountry(
            BasicCountryAndFirstLevelSubdivision country,
            NpgsqlCommand writeCommand,
            NpgsqlCommand writeCommand2a,
            NpgsqlCommand writeCommand2b,
            NpgsqlCommand writeCommand3,
            NpgsqlCommand writeCommand4,
            NpgsqlCommand writeCommand5,
            NpgsqlCommand writeCommand6,
            NpgsqlCommand writeCommand7,
            NpgsqlCommand writeCommand8,
            NpgsqlCommand writeCommand9,
            NpgsqlCommand writeCommand10,
            NpgsqlCommand writeCommand11,
            NpgsqlCommand writeCommand12,
            NpgsqlCommand writeCommand13
            )
        {
            var continentIds = new List<int> { 3806, 3810, 3811, 3816, 3822, 3823 };

            writeCommand.Parameters["id"].Value = country.Id;
            writeCommand.Parameters["user_id"].Value = country.UserId;
            writeCommand.Parameters["created"].Value = country.Created;
            writeCommand.Parameters["changed"].Value = country.Changed;
            writeCommand.Parameters["title"].Value = country.Title;
            writeCommand.Parameters["status"].Value = country.Status;
            writeCommand.Parameters["node_type_id"].Value = country.NodeTypeId;
            writeCommand.Parameters["is_term"].Value = country.IsTerm;
            writeCommand.ExecuteNonQuery();

            writeCommand4.Parameters["id"].Value = country.Id;
            writeCommand4.ExecuteNonQuery();

            writeCommand5.Parameters["id"].Value = country.Id;
            writeCommand5.ExecuteNonQuery();

            writeCommand13.Parameters["id"].Value = country.Id;
            writeCommand13.Parameters["name"].Value = country.Name;
            writeCommand13.Parameters["country_id"].Value = country.CountryId;
            writeCommand13.ExecuteNonQuery();

            writeCommand6.Parameters["id"].Value = country.Id;
            writeCommand6.Parameters["iso_3166_code"].Value = country.ISO3166_1_Code;
            writeCommand6.ExecuteNonQuery();

            if (!continentIds.Contains(country.GlobalRegionId))
            {
                writeCommand2a.Parameters["id"].Value = country.Id;
                writeCommand2a.Parameters["continental_region_id"].Value = country.GlobalRegionId;
                writeCommand2a.ExecuteNonQuery();
            }
            else
            {
                writeCommand2b.Parameters["id"].Value = country.Id;
                writeCommand2b.Parameters["continent_id"].Value = country.GlobalRegionId;
                writeCommand2b.ExecuteNonQuery();
            }

            writeCommand7.Parameters["id"].Value = country.Id;
            writeCommand7.Parameters["iso_3166_2_code"].Value = country.ISO3166_2_Code;
            writeCommand7.ExecuteNonQuery();

            writeCommand9.Parameters["id"].Value = country.Id;
            writeCommand9.Parameters["country_id"].Value = country.CountryId;
            writeCommand9.ExecuteNonQuery();

            writeCommand12.Parameters["id"].Value = country.Id;
            writeCommand12.ExecuteNonQuery();

            writeCommand10.Parameters["id"].Value = country.Id;
            writeCommand10.Parameters["country_id"].Value = country.CountryId;
            writeCommand10.ExecuteNonQuery();

            writeCommand11.Parameters["id"].Value = country.Id;
            writeCommand11.ExecuteNonQuery();

            writeCommand8.Parameters["id"].Value = country.Id;
            writeCommand8.ExecuteNonQuery();


            writeCommand3.Parameters["term_id_child"].Value = country.Id;
            writeCommand3.Parameters["term_id_parent"].Value = country.GlobalRegionId;
            writeCommand3.ExecuteNonQuery();

        }

    }
}
