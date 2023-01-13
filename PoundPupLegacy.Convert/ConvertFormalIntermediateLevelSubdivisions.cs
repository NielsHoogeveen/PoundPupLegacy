﻿using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Db.Readers;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert;

internal partial class Program
{
    private static async IAsyncEnumerable<FormalIntermediateLevelSubdivision> ReadFormalIntermediateLevelSubdivisionCsv(NpgsqlConnection connection, NodeIdByUrlIdReader nodeIdReader)
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
            var countryId = await nodeIdReader.ReadAsync(PPL, int.Parse(parts[7]));
            var countryName = (await reader.ReadAsync(PPL, VOCABULARY_TOPICS, countryId)).Name;
            yield return new FormalIntermediateLevelSubdivision
            {
                Id = null,
                CreatedDateTime = DateTime.Parse(parts[1]),
                ChangedDateTime = DateTime.Parse(parts[2]),
                VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = PPL,
                        Name = VOCABULARY_TOPICS,
                        TermName = title,
                        ParentNames = new List<string> { countryName },
                    }
                },
                Description = "",
                FileIdTileImage = null,
                NodeTypeId = int.Parse(parts[4]),
                OwnerId = OWNER_GEOGRAPHY,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        TenantId = 1,
                        PublicationStatusId = int.Parse(parts[5]),
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                PublisherId = int.Parse(parts[6]),
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
        await using var nodeIdReader = await NodeIdByUrlIdReader.CreateAsync(connection);
        await using var tx = await connection.BeginTransactionAsync();
        try
        {
            await FormalIntermediateLevelSubdivisionCreator.CreateAsync(ReadFormalIntermediateLevelSubdivisionCsv(connection, nodeIdReader), connection);
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
    }

}
