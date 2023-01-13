using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Db.Readers;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{
    private static async IAsyncEnumerable<InformalIntermediateLevelSubdivision> ReadInformalIntermediateLevelSubdivisionCsv(NpgsqlConnection connection, NodeIdByUrlIdReader nodeIdReader)
    {
        await using var reader = await TermReaderByNameableId.CreateAsync(connection);
        await foreach (string line in System.IO.File.ReadLinesAsync(@"..\..\..\InformalIntermediateLevelSubdivisions.csv").Skip(1))
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
            yield return new InformalIntermediateLevelSubdivision
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
            };
        }
    }

    private static async Task MigrateInformalIntermediateLevelSubdivisions(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        await using var nodeIdReader = await NodeIdByUrlIdReader.CreateAsync(connection);
        await using var tx = await connection.BeginTransactionAsync();
        try
        {
            await InformalIntermediateLevelSubdivisionCreator.CreateAsync(ReadInformalIntermediateLevelSubdivisionCsv(connection, nodeIdReader), connection);
            await InformalIntermediateLevelSubdivisionCreator.CreateAsync(ReadInformalIntermediateLevelSubdivisions(mysqlconnection, nodeIdReader), connection);
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }

    }
    private static async IAsyncEnumerable<InformalIntermediateLevelSubdivision> ReadInformalIntermediateLevelSubdivisions(MySqlConnection mysqlconnection, NodeIdByUrlIdReader nodeIdReader)
    {
        var sql = $"""
            SELECT
                n.nid id,
                n.uid access_role_id,
                n.title,
                n.`status` node_status_id,
                FROM_UNIXTIME(n.created) created_date_time, 
                FROM_UNIXTIME(n.changed) changed_date_time,
                n2.nid country_id,
                ua.dst url_path
            FROM node n 
            LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
            JOIN category_hierarchy ch ON ch.cid = n.nid
            JOIN node n2 ON n2.nid = ch.parent
            WHERE n.`type` = 'region_facts'AND n2.`type` = 'country_type'
            """;
        using var readCommand = mysqlconnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {

            var id = reader.GetInt32("id");
            var title = $"{reader.GetString("title")} (region of the USA)";
            var vocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = PPL,
                    Name = VOCABULARY_TOPICS,
                    TermName = title,
                    ParentNames = new List<string>{ "United States of America" },
                }
            };

            yield return new InformalIntermediateLevelSubdivision
            {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = title,
                OwnerId = OWNER_GEOGRAPHY,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        TenantId = 1,
                        PublicationStatusId = reader.GetInt32("node_status_id"),
                        UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                NodeTypeId = 18,
                CountryId = await nodeIdReader.ReadAsync(PPL, reader.GetInt32("country_id")),
                Name = reader.GetString("title"),
                VocabularyNames = vocabularyNames,
                Description = "",
                FileIdTileImage = null,
            };
        }
        await reader.CloseAsync();
    }
}
