using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {

        private static Dictionary<int, List<VocabularyName>> OrganizationTypeVocubularyNames = new Dictionary<int, List<VocabularyName>>
        {
            [12625] = new List<VocabularyName>
            {
                new VocabularyName
                {
                    Name = "adoption agencies",
                    VocabularyId = 4126,
                    ParentNames = new List<string>()
                }
            },
            [35715] = new List<VocabularyName> {
                new VocabularyName
                {
                    Name = "church",
                    VocabularyId = 4126,
                    ParentNames = new List<string>()
                }
            },
            [31716] = new List<VocabularyName> {
                new VocabularyName
                {
                    Name = "boot camp",
                    VocabularyId = 4126,
                    ParentNames = new List<string>()
                }
            },
            [12632] = new List<VocabularyName> {
                new VocabularyName
                {
                    Name = "media",
                    VocabularyId = 4126,
                    ParentNames = new List<string>()
                }
             },
            [48309] = new List<VocabularyName> {
                new VocabularyName
                {
                    Name = "blog",
                    VocabularyId = 4126,
                    ParentNames = new List<string>()
                }
            },
            [12635] = new List<VocabularyName> {
                new VocabularyName
                {
                    Name = "orphanages",
                    VocabularyId = 4126,
                    ParentNames = new List<string>()
                }
            },
            [12624] = new List<VocabularyName> {
                new VocabularyName
                {
                    Name = "adoption advocates",
                    VocabularyId = 4126,
                    ParentNames = new List<string>()
                }
            },
            [31586] = new List<VocabularyName> {
                new VocabularyName
                {
                    Name = "boarding school",
                    VocabularyId = 4126,
                    ParentNames = new List<string>()
                }
            },
            [14670] = new List<VocabularyName> {
                new VocabularyName
                {
                    Name = "adoption facilitators",
                    VocabularyId = 4126,
                    ParentNames = new List<string>()
                }
            },
            [17310] = new List<VocabularyName> {
                new VocabularyName
                {
                    Name = "maternity homes",
                    VocabularyId = 4126,
                    ParentNames = new List<string>()
                }
            },
        };


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
                var id = reader.GetInt32("id");
                var name = reader.GetString("title");
                yield return new OrganizationType
                {
                    Id = id,
                    AccessRoleId = reader.GetInt32("access_role_id"),
                    CreatedDateTime = reader.GetDateTime("created_date_time"),
                    ChangedDateTime = reader.GetDateTime("changed_date_time"),
                    Title = name,
                    NodeStatusId = reader.GetInt32("node_status_id"),
                    NodeTypeId = 1,
                    Description = reader.GetString("description"),
                    VocabularyNames = GetVocabularyNames(ORGANIZATION_TYPE, id, name, OrganizationTypeVocubularyNames),
                };

            }
            reader.Close();
        }
    }
}
