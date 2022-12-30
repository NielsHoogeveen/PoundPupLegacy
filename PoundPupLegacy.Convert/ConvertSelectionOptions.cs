using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {

        private static IEnumerable<BasicNode> GetChildPlacementTypes()
        {
            return new List<BasicNode>
            {
                new BasicNode
                {
                    Id = 106,
                    UserId = 1,
                    Created = DateTime.Now,
                    Changed = DateTime.Now,
                    Title = "Adoption",
                    Status = 1,
                    NodeTypeId = 27,
                },
                new BasicNode
                {
                    Id = 107,
                    UserId = 1,
                    Created = DateTime.Now,
                    Changed = DateTime.Now,
                    Title = "Foster care",
                    Status = 1,
                    NodeTypeId = 27,
                },
                new BasicNode
                {
                    Id = 108,
                    UserId = 1,
                    Created = DateTime.Now,
                    Changed = DateTime.Now,
                    Title = "To be adopted",
                    Status = 1,
                    NodeTypeId = 27,
                },
                new BasicNode
                {
                    Id = 109,
                    UserId = 1,
                    Created = DateTime.Now,
                    Changed = DateTime.Now,
                    Title = "Legal Guardianship",
                    Status = 1,
                    NodeTypeId = 27,
                },
                 new BasicNode
                {
                    Id = 110,
                    UserId = 1,
                    Created = DateTime.Now,
                    Changed = DateTime.Now,
                    Title = "Institution",
                    Status = 1,
                    NodeTypeId = 27,
                },
            };
        }
        private static IEnumerable<BasicNode> GetFamilySizes()
        {
            return new List<BasicNode>
            {
                new BasicNode
                {
                    Id = 111,
                    UserId = 1,
                    Created = DateTime.Now,
                    Changed = DateTime.Now,
                    Title = "1 to 4",
                    Status = 1,
                    NodeTypeId = 28,
                },
                new BasicNode
                {
                    Id = 112,
                    UserId = 1,
                    Created = DateTime.Now,
                    Changed = DateTime.Now,
                    Title = "4 to 8",
                    Status = 1,
                    NodeTypeId = 28,
                },
                new BasicNode
                {
                    Id = 113,
                    UserId = 1,
                    Created = DateTime.Now,
                    Changed = DateTime.Now,
                    Title = "8 to 12",
                    Status = 1,
                    NodeTypeId = 28,
                },
                new BasicNode
                {
                    Id = 114,
                    UserId = 1,
                    Created = DateTime.Now,
                    Changed = DateTime.Now,
                    Title = "more than 12",
                    Status = 1,
                    NodeTypeId = 28,
                },
            };
        }


        private static BasicNode GetNodeFromReader(MySqlDataReader reader, int nodeTypeId)
        {
            return new BasicNode
            {
                Id = reader.GetInt32("id"),
                UserId = reader.GetInt32("user_id"),
                Created = reader.GetDateTime("created"),
                Changed = reader.GetDateTime("changed"),
                Title = reader.GetString("title"),
                Status = reader.GetInt32("status"),
                NodeTypeId = nodeTypeId,
            };
        }
        private static void MigrateSelectionOptions(MySqlConnection mysqlconnection, NpgsqlConnection connection, int categoryId, int nodeTypeId, string tableName)
        {
            var sql = $"""
                    SELECT 
                    n2.nid id,
                    n2.uid user_id,
                    n2.title,
                    n2.`status`,
                    FROM_UNIXTIME(n2.created) created, 
                    FROM_UNIXTIME(n2.changed) `changed`
                    FROM node n1 
                    JOIN category c ON c.cnid = n1.nid
                    JOIN node n2 ON n2.nid = c.cid
                    WHERE n1.nid  = {categoryId}
                    """;
            MigrateSelectionOptions(mysqlconnection, connection, nodeTypeId, tableName, false, sql);
        }

        private static void MigrateChildPlacementTypes(NpgsqlConnection connection)
        {
            SingleIdEnitityCreator.Create(GetChildPlacementTypes(), "child_placement_type", connection);
        }
        private static void MigrateFamilySizes(NpgsqlConnection connection)
        {
            SingleIdEnitityCreator.Create(GetFamilySizes(), "family_size", connection);
        }

        private static void MigrateSelectionOptions(MySqlConnection mysqlconnection, NpgsqlConnection connection, int nodeTypeId, string tableName, bool isTerm, string sql)
        {
            SingleIdEnitityCreator.Create(ReadSelectionOptions(mysqlconnection, nodeTypeId, isTerm, sql), tableName, connection);
        }
        private static IEnumerable<BasicNode> ReadSelectionOptions(MySqlConnection mysqlconnection, int nodeTypeId, bool isTerm, string sql)
        {

            using var readCommand = mysqlconnection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;

            var reader = readCommand.ExecuteReader();

            while (reader.Read())
            {
                yield return GetNodeFromReader(reader, nodeTypeId);

            }
            reader.Close();
        }


    }
}
