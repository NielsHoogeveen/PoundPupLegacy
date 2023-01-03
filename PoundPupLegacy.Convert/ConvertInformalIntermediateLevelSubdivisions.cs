using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {
        private static IEnumerable<InformalIntermediateLevelSubdivision> ReadInformalIntermediateLevelSubdivisionCsv()
        {
            foreach (string line in System.IO.File.ReadLines(@"..\..\..\InformalIntermediateLevelSubdivisions.csv").Skip(1))
            {

                var parts = line.Split(new char[] { ';' });
                var id = int.Parse(parts[0]);
                var title = parts[8];
                yield return new InformalIntermediateLevelSubdivision
                {
                    Id = id,
                    CreatedDateTime = DateTime.Parse(parts[1]),
                    ChangedDateTime = DateTime.Parse(parts[2]),
                    VocabularyNames = GetVocabularyNames(TOPICS, id, title, new Dictionary<int, List<VocabularyName>>()),
                    Description = "",
                    FileIdTileImage = null,
                    NodeTypeId = int.Parse(parts[4]),
                    NodeStatusId = int.Parse(parts[5]),
                    AccessRoleId = int.Parse(parts[6]),
                    CountryId = int.Parse(parts[7]),
                    Title = title,
                    Name = parts[9],
                };
            }
        }

        private static void MigrateInformalIntermediateLevelSubdivisions(MySqlConnection mysqlconnection, NpgsqlConnection connection)
        {
            var subdivisions = ReadInformalIntermediateLevelSubdivisionCsv().ToList();
            foreach (var subdivision in subdivisions)
            {
                if (subdivision.Id == 0)
                {
                    NodeId++;
                    subdivision.Id = NodeId;
                }
            }
            InformalIntermediateLevelSubdivisionCreator.Create(subdivisions, connection);
            InformalIntermediateLevelSubdivisionCreator.Create(ReadInformalIntermediateLevelSubdivisions(mysqlconnection), connection);
        }
        private static IEnumerable<InformalIntermediateLevelSubdivision> ReadInformalIntermediateLevelSubdivisions(MySqlConnection mysqlconnection)
        {
            var sql = $"""
            SELECT
                n.nid id,
                n.uid user_id,
                n.title,
                n.`status`,
                FROM_UNIXTIME(n.created) created, 
                FROM_UNIXTIME(n.changed) `changed`,
                n2.nid country_id
                FROM node n 
                JOIN category_hierarchy ch ON ch.cid = n.nid
                JOIN node n2 ON n2.nid = ch.parent
                WHERE n.`type` = 'region_facts'AND n2.`type` = 'country_type'
            """;
            using var readCommand = mysqlconnection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;


            var reader = readCommand.ExecuteReader();

            while (reader.Read())
            {

                var id = reader.GetInt32("id");
                var title = $"{reader.GetString("title")} (region of the USA)";
                yield return new InformalIntermediateLevelSubdivision
                {
                    Id = id,
                    AccessRoleId = reader.GetInt32("user_id"),
                    CreatedDateTime = reader.GetDateTime("created"),
                    ChangedDateTime = reader.GetDateTime("changed"),
                    Title = title,
                    NodeStatusId = reader.GetInt32("status"),
                    NodeTypeId = 18,
                    CountryId = reader.GetInt32("country_id"),
                    Name = reader.GetString("title"),
                    VocabularyNames = GetVocabularyNames(TOPICS, id, title, new Dictionary<int, List<VocabularyName>>()),
                    Description = "",
                    FileIdTileImage = null,
                };
            }
            reader.Close();
        }
    }
}
