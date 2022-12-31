using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {
        private static IEnumerable<FormalIntermediateLevelSubdivision> ReadFormalIntermediateLevelSubdivisionCsv()
        {
            foreach (string line in System.IO.File.ReadLines(@"..\..\..\FormalIntermediateLevelSubdivisions.csv").Skip(1))
            {

                var parts = line.Split(new char[] { ';' });
                yield return new FormalIntermediateLevelSubdivision
                {
                    Id = int.Parse(parts[0]),
                    CreatedDateTime = DateTime.Parse(parts[1]),
                    ChangedDateTime = DateTime.Parse(parts[2]),
                    VocabularyId = 4126,
                    NodeTypeId = int.Parse(parts[4]),
                    NodeStatusId = int.Parse(parts[5]),
                    AccessRoleId = int.Parse(parts[6]),
                    CountryId = int.Parse(parts[7]),
                    Title = parts[8],
                    Name = parts[9],
                    ISO3166_2_Code = parts[10],
                    FileIdFlag = null,
                };
            }
        }

        private static void MigrateFormalIntermediateLevelSubdivisions(MySqlConnection mysqlconnection, NpgsqlConnection connection)
        {
            var subdivisions = ReadFormalIntermediateLevelSubdivisionCsv().ToList();
            foreach (var subdivision in subdivisions)
            {
                if (subdivision.Id == 0)
                {
                    NodeId++;
                    subdivision.Id = NodeId;
                }
            }
            FormalIntermediateLevelSubdivisionCreator.Create(subdivisions, connection);
        }

    }
}
