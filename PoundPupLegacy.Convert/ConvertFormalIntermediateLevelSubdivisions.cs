using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Db.Readers;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert;

internal partial class Program
{
    private static async IAsyncEnumerable<FormalIntermediateLevelSubdivision> ReadFormalIntermediateLevelSubdivisionCsv(NpgsqlConnection connection)
    {
        await using var reader = await TermReaderByNameableId.CreateAsync(connection);
        await foreach (string line in System.IO.File.ReadLinesAsync(@"..\..\..\FormalIntermediateLevelSubdivisions.csv").Skip(1))
        {

            var parts = line.Split(new char[] { ';' });
            var id = int.Parse(parts[0]);
            if (id == 0)
            {
                NodeId++;
                id = NodeId;
            }
            var title = parts[8];
            var countryId = int.Parse(parts[7]);
            var countryName = (await reader.ReadAsync(TOPICS, countryId)).Name;
            yield return new FormalIntermediateLevelSubdivision
            {
                Id = id,
                CreatedDateTime = DateTime.Parse(parts[1]),
                ChangedDateTime = DateTime.Parse(parts[2]),
                VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        VocabularyId = TOPICS,
                        Name = title,
                        ParentNames = new List<string> { countryName },
                    }
                },
                Description = "",
                FileIdTileImage = null,
                NodeTypeId = int.Parse(parts[4]),
                NodeStatusId = int.Parse(parts[5]),
                AccessRoleId = int.Parse(parts[6]),
                CountryId = countryId,
                Title = title,
                Name = parts[9],
                ISO3166_2_Code = parts[10],
                FileIdFlag = null,
            };
        }
    }

    private static async Task MigrateFormalIntermediateLevelSubdivisions(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        await using var tx = await connection.BeginTransactionAsync();
        try
        {
            await FormalIntermediateLevelSubdivisionCreator.CreateAsync(ReadFormalIntermediateLevelSubdivisionCsv(connection), connection);
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
    }

}
