using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {
        private static int GetSubdivisionId(string code, NpgsqlConnection connection)
        {
            try
            {
                var sql = $"""
                SELECT id
                FROM public.iso_coded_subdivision 
                WHERE iso_3166_2_code = '{code}' 
                """;

                using var readCommand = connection.CreateCommand();
                readCommand.CommandType = CommandType.Text;
                readCommand.CommandTimeout = 300;
                readCommand.CommandText = sql;

                var reader = readCommand.ExecuteReader();
                reader.Read();
                var id = reader.GetInt32(0);
                reader.Close();
                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot find subdivision with code {code}");
                throw;
            }

        }

        private static int GetIntermediateLevelSubdivisionId(int countryId, string name, NpgsqlConnection connection)
        {
            try
            {
                var sql = $"""
                SELECT id
                FROM public.subdivision 
                WHERE name = '{name}' 
                AND country_id = {countryId}
                """;

                using var readCommand = connection.CreateCommand();
                readCommand.CommandType = CommandType.Text;
                readCommand.CommandTimeout = 300;
                readCommand.CommandText = sql;

                var reader = readCommand.ExecuteReader();
                reader.Read();
                var id = reader.GetInt32(0);
                reader.Close();
                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot find subdivision with name {name} and counrtyId {countryId}");
                throw;
            }

        }
        private static IEnumerable<BasicSecondLevelSubdivision> ReadBasicSecondLevelSubdivisionsInInformalPrimarySubdivisionCsv(NpgsqlConnection connection)
        {
            foreach (string line in System.IO.File.ReadLines(@"..\..\..\BasicSecondLevelSubdivisionsInInformalPrimarySubdivision.csv").Skip(1))
            {

                var parts = line.Split(new char[] { ';' });
                yield return new BasicSecondLevelSubdivision
                {
                    Id = int.Parse(parts[0]),
                    CreatedDateTime = DateTime.Parse(parts[1]),
                    ChangedDateTime = DateTime.Parse(parts[2]),
                    NodeTypeId = int.Parse(parts[4]),
                    NodeStatusId = int.Parse(parts[5]),
                    AccessRoleId = int.Parse(parts[6]),
                    CountryId = int.Parse(parts[7]),
                    Title = parts[8],
                    Name = parts[9],
                    VocabularyId = 4126,
                    ISO3166_2_Code = parts[10],
                    IntermediateLevelSubdivisionId = GetIntermediateLevelSubdivisionId(int.Parse(parts[7]), parts[11], connection),
                    FileIdFlag = null,
                };
            }
        }

        private static IEnumerable<BasicSecondLevelSubdivision> ReadFormalIntermediateLevelSubdivisionCsv(NpgsqlConnection connection)
        {
            foreach (string line in System.IO.File.ReadLines(@"..\..\..\BasicSecondLevelSubdivisions.csv").Skip(1))
            {

                var parts = line.Split(new char[] { ';' });
                yield return new BasicSecondLevelSubdivision
                {
                    Id = int.Parse(parts[0]),
                    CreatedDateTime = DateTime.Parse(parts[1]),
                    ChangedDateTime = DateTime.Parse(parts[2]),
                    NodeTypeId = int.Parse(parts[4]),
                    NodeStatusId = int.Parse(parts[5]),
                    AccessRoleId = int.Parse(parts[6]),
                    CountryId = int.Parse(parts[7]),
                    Title = parts[8],
                    Name = parts[9],
                    VocabularyId = 4126,
                    ISO3166_2_Code = parts[10],
                    IntermediateLevelSubdivisionId = GetSubdivisionId(parts[11], connection),
                    FileIdFlag = null,
                };
            }
        }
        private static void MigrateBasicSecondLevelSubdivisions(MySqlConnection mysqlconnection, NpgsqlConnection connection)
        {
            var subdivisions1 = ReadBasicSecondLevelSubdivisionsInInformalPrimarySubdivisionCsv(connection).ToList();
            foreach (var subdivision in subdivisions1)
            {
                if (subdivision.Id == 0)
                {
                    NodeId++;
                    subdivision.Id = NodeId;
                }
            }
            var subdivisions2 = ReadFormalIntermediateLevelSubdivisionCsv(connection).ToList();
            foreach (var subdivision in subdivisions2)
            {
                if (subdivision.Id == 0)
                {
                    NodeId++;
                    subdivision.Id = NodeId;
                }
            }
            BasicSecondLevelSubdivisionCreator.Create(subdivisions1, connection);
            BasicSecondLevelSubdivisionCreator.Create(subdivisions2, connection);
            BasicSecondLevelSubdivisionCreator.Create(ReadBasicSecondLevelSubdivisions(mysqlconnection), connection);
        }
        private static IEnumerable<BasicSecondLevelSubdivision> ReadBasicSecondLevelSubdivisions(MySqlConnection mysqlconnection)
        {
            var continentIds = new List<int> { 3806, 3810, 3811, 3816, 3822, 3823 };

            var sql = $"""
                SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`,
                    n2.nid intermediate_level_subdivision_id,
                    3805 country_id,
                    CONCAT('US-', s.field_statecode_value) 
                    iso_3166_2_code,
                    s.field_state_flag_fid file_id_flag
                FROM node n 
                JOIN content_type_statefact s ON s.nid = n.nid
                JOIN category_hierarchy ch ON ch.cid = n.nid
                JOIN node n2 ON n2.nid = ch.parent
                WHERE n.`type` = 'statefact'
                AND n2.`type` = 'region_facts'
                AND s.field_statecode_value IS NOT NULL
                ORDER BY s.field_statecode_value
                """;
            using var readCommand = mysqlconnection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;

            var reader = readCommand.ExecuteReader();

            while (reader.Read())
            {
                yield return new BasicSecondLevelSubdivision
                {
                    Id = reader.GetInt32("id"),
                    AccessRoleId = reader.GetInt32("user_id"),
                    CreatedDateTime = reader.GetDateTime("created"),
                    ChangedDateTime = reader.GetDateTime("changed"),
                    Title = $"{reader.GetString("title")} (state of the USA)",
                    Name = reader.GetString("title"),
                    NodeStatusId = reader.GetInt32("status"),
                    NodeTypeId = 19,
                    VocabularyId = 4126,
                    IntermediateLevelSubdivisionId = reader.GetInt32("intermediate_level_subdivision_id"),
                    CountryId = reader.GetInt32("country_id"),
                    ISO3166_2_Code = reader.GetString("iso_3166_2_code"),
                    FileIdFlag = reader.IsDBNull("file_id_flag") ? null : reader.GetInt32("file_id_flag"),
                };
            }
            reader.Close();
        }

    }
}
