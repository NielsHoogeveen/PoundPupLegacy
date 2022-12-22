using MySqlConnector;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {



        private static BasicNode GetNodeFromReader(MySqlDataReader reader, int nodeTypeId, bool isTerm)
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
                IsTerm = isTerm
            };
        }
        private static void MigrateSelectionOptions(MySqlConnection mysqlconnection, TargetConnection targetConnection, int categoryId, int nodeTypeId, string tableName)
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
            MigrateSelectionOptions(mysqlconnection, targetConnection, nodeTypeId, tableName, false, sql);
        }

        private static void MigrateSelectionOptions(MySqlConnection mysqlconnection, TargetConnection targetConnection, int nodeTypeId, string tableName, bool isTerm, string sql)
        {
            targetConnection.Create(ReadSelectionOptions(mysqlconnection, nodeTypeId, isTerm, sql), tableName);
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
                yield return GetNodeFromReader(reader, nodeTypeId, isTerm);

            }
            reader.Close();
        }


    }
}
