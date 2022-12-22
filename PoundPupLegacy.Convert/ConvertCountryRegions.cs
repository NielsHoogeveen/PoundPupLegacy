using MySqlConnector;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {
        private record CountryRegion : PoundPupLegacy.Model.BasicNode
        {
            public required int CountryId { get; set; }
        }

        private static void MigrateCountryRegions(MySqlConnection mysqlconnection, NpgsqlConnection postgresqlconnection)
        {
            var sql = $"SELECT\r\nn.nid id,\r\nn.uid user_id,\r\nn.title,\r\nn.`status`,\r\nFROM_UNIXTIME(n.created) created, \r\nFROM_UNIXTIME(n.changed) `changed`,\r\nn2.nid country_id\r\nFROM node n \r\nJOIN category_hierarchy ch ON ch.cid = n.nid\r\nJOIN node n2 ON n2.nid = ch.parent\r\nWHERE n.`type` = 'region_facts'\r\nAND n2.`type` = 'country_type'";
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
            writeCommand2.CommandText = $"""INSERT INTO public."country_region" (id, country_id) VALUES(@id, @country_id)""";
            writeCommand2.Parameters.Add("id", NpgsqlDbType.Integer);
            writeCommand2.Parameters.Add("country_id", NpgsqlDbType.Integer);

            using var writeCommand3 = postgresqlconnection.CreateCommand();
            writeCommand3.CommandType = CommandType.Text;
            writeCommand3.CommandTimeout = 300;
            writeCommand3.CommandText = $"""INSERT INTO public."term_hierarchy" (term_id_child, term_id_parent) VALUES(@term_id_child, @term_id_parent)""";
            writeCommand3.Parameters.Add("term_id_child", NpgsqlDbType.Integer);
            writeCommand3.Parameters.Add("term_id_parent", NpgsqlDbType.Integer);

            var reader = readCommand.ExecuteReader();

            while (reader.Read())
            {
                var region = new CountryRegion
                {
                    Id = reader.GetInt32("id"),
                    UserId = reader.GetInt32("user_id"),
                    Created = reader.GetDateTime("created"),
                    Changed = reader.GetDateTime("changed"),
                    Title = reader.GetString("title"),
                    Status = reader.GetInt32("status"),
                    NodeTypeId = 18,
                    IsTerm = true,
                    CountryId = reader.GetInt32("country_id"),
                };

                writeCommand.Parameters["id"].Value = region.Id;
                writeCommand.Parameters["user_id"].Value = region.UserId;
                writeCommand.Parameters["created"].Value = region.Created;
                writeCommand.Parameters["changed"].Value = region.Changed;
                writeCommand.Parameters["title"].Value = region.Title;
                writeCommand.Parameters["status"].Value = region.Status;
                writeCommand.Parameters["node_type_id"].Value = region.NodeTypeId;
                writeCommand.Parameters["is_term"].Value = region.IsTerm;
                writeCommand.ExecuteNonQuery();

                writeCommand2.Parameters["id"].Value = region.Id;
                writeCommand2.Parameters["country_id"].Value = region.CountryId;
                writeCommand2.ExecuteNonQuery();

                writeCommand3.Parameters["term_id_child"].Value = region.Id;
                writeCommand3.Parameters["term_id_parent"].Value = region.CountryId;
                writeCommand3.ExecuteNonQuery();
            }
            reader.Close();
        }

    }
}
