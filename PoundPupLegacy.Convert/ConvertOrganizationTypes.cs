using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {

        private static void MigrateOrganizationTypes(MySqlConnection mysqlconnection, NpgsqlConnection connection)
        {
            OrganizationTypeCreator.Create(ReadOrganizationTypes(mysqlconnection), connection);
        }
        private static IEnumerable<OrganizationType> ReadOrganizationTypes(MySqlConnection mysqlconnection)
        {

            var sql = $"""
                    SELECT 
                    n2.nid id,
                    n2.uid access_role_id,
                    n2.title,
                    n2.node_status_id,
                    FROM_UNIXTIME(n2.created) created_date_time, 
                    FROM_UNIXTIME(n2.changed) `changed`
                    FROM node n1 
                    JOIN category c ON c.cnid = n1.nid
                    JOIN node n2 ON n2.nid = c.cid
                    WHERE n1.nid  = 12622
                    """;

            using var readCommand = mysqlconnection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;

            var reader = readCommand.ExecuteReader();

            while (reader.Read())
            {
                var name = reader.GetString("title");
                yield return new OrganizationType
                {
                    Id = reader.GetInt32("id"),
                    AccessRoleId = reader.GetInt32("access_role_id"),
                    CreatedDateTime = reader.GetDateTime("created_date_time"),
                    ChangedDateTime = reader.GetDateTime("changed_date_time"),
                    Title = name,
                    NodeStatusId = reader.GetInt32("node_status_id"),
                    NodeTypeId = 1,
                    Description = reader.GetString("description"),
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = ORGANIZATION_TYPE,
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
