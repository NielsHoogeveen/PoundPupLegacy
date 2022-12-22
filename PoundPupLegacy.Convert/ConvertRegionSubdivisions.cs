using MySqlConnector;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {
        private record RegionSubDivision : PoundPupLegacy.Model.BasicNode
        {
            public required int CountryRegionId { get; init; }
            public required string ISO3166_2Code { get; init; }
        }


        private static void MigrateRegionSubdivisions(MySqlConnection mysqlconnection, NpgsqlConnection postgresqlconnection)
        {
            var continentIds = new List<int> { 3806, 3810, 3811, 3816, 3822, 3823 };

            var sql = $"SELECT\r\nn.nid id,\r\nn.uid user_id,\r\nn.title,\r\nn.`status`,\r\nFROM_UNIXTIME(n.created) created, \r\nFROM_UNIXTIME(n.changed) `changed`,\r\nn2.nid country_region_id,\r\nCONCAT('US-', s.field_statecode_value) iso_3166_2_code\r\nFROM node n \r\nJOIN content_type_statefact s ON s.nid = n.nid\r\nJOIN category_hierarchy ch ON ch.cid = n.nid\r\nJOIN node n2 ON n2.nid = ch.parent\r\nWHERE n.`type` = 'statefact'\r\nAND n2.`type` = 'region_facts'\r\nAND s.field_statecode_value IS NOT NULL\r\nORDER BY s.field_statecode_value";
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

            using var writeCommand2 = postgresqlconnection.CreateCommand();
            writeCommand2.CommandType = CommandType.Text;
            writeCommand2.CommandTimeout = 300;
            writeCommand2.CommandText = $"""INSERT INTO public."country_region_subdivision" (id, country_region_id) VALUES(@id, @country_region_id)""";
            writeCommand2.Parameters.Add("id", NpgsqlDbType.Integer);
            writeCommand2.Parameters.Add("country_region_id", NpgsqlDbType.Integer);

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
            writeCommand5.CommandText = $"""INSERT INTO public."country_subdivision" (id, iso_3166_2_code) VALUES(@id,@iso_3166_2_code)""";
            writeCommand5.Parameters.Add("id", NpgsqlDbType.Integer);
            writeCommand5.Parameters.Add("iso_3166_2_code", NpgsqlDbType.Varchar);

            var reader = readCommand.ExecuteReader();

            while (reader.Read())
            {
                var country = new RegionSubDivision
                {
                    Id = reader.GetInt32("id"),
                    UserId = reader.GetInt32("user_id"),
                    Created = reader.GetDateTime("created"),
                    Changed = reader.GetDateTime("changed"),
                    Title = reader.GetString("title"),
                    Status = reader.GetInt32("status"),
                    NodeTypeId = 19,
                    IsTerm = true,
                    CountryRegionId = reader.GetInt32("country_region_id"),
                    ISO3166_2Code = reader.GetString("iso_3166_2_code")
                };

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
                writeCommand5.Parameters["iso_3166_2_code"].Value = country.ISO3166_2Code;
                writeCommand5.ExecuteNonQuery();

                writeCommand2.Parameters["id"].Value = country.Id;
                writeCommand2.Parameters["country_region_id"].Value = country.CountryRegionId;
                writeCommand2.ExecuteNonQuery();

                writeCommand3.Parameters["term_id_child"].Value = country.Id;
                writeCommand3.Parameters["term_id_parent"].Value = country.CountryRegionId;
                writeCommand3.ExecuteNonQuery();
            }
            reader.Close();
        }

    }
}
