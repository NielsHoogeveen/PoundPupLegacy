using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {

        private static void MigrateDocumentTypes(MySqlConnection mysqlconnection, NpgsqlConnection connection)
        {
            DocumentTypeCreator.Create(ReadSelectionOptions(mysqlconnection), connection);
        }
        private static IEnumerable<DocumentType> ReadSelectionOptions(MySqlConnection mysqlconnection)
        {

            var sql = $"""
                    SELECT 
                    n2.nid id,
                    n2.uid access_role_id,
                    n2.title,
                    n2.`status`,
                    FROM_UNIXTIME(n2.created) created_date_time, 
                    FROM_UNIXTIME(n2.changed) `changed`
                    FROM node n1 
                    JOIN category c ON c.cnid = n1.nid
                    JOIN node n2 ON n2.nid = c.cid
                    WHERE n1.nid  = 42416
                    """;

            using var readCommand = mysqlconnection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;

            var reader = readCommand.ExecuteReader();

            while (reader.Read())
            {
                var name = reader.GetString("title");
                yield return new DocumentType
                {
                    Id = reader.GetInt32("id"),
                    AccessRoleId = reader.GetInt32("access_role_id"),
                    CreatedDateTime = reader.GetDateTime("created_date_time"),
                    ChangedDateTime = reader.GetDateTime("changed_date_time"),
                    Title = name,
                    NodeStatusId = reader.GetInt32("status"),
                    NodeTypeId = 9,
                    Description = reader.GetString("description"),
                    FileIdTileImage = null,
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = DOCUMENT_TYPES,
                            Name = name,
                            ParentNames = new List<string>(),
                        },
                    },
                };
            }
            reader.Close();
        }


    }
}
