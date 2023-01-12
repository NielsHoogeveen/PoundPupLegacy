using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Db.Readers;
using PoundPupLegacy.Model;
using System.Data;
using System.Reflection.PortableExecutable;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {

        private static async IAsyncEnumerable<BasicSecondLevelSubdivision> ReadBasicSecondLevelSubdivisionsInInformalPrimarySubdivisionCsv(NpgsqlConnection connection)
        {
            await using var termReader = await TermReaderByNameableId.CreateAsync(connection);
            await using var subdivisionReader = await SubdivisionIdReaderByName.CreateAsync(connection);
            await foreach (string line in System.IO.File.ReadLinesAsync(@"..\..\..\BasicSecondLevelSubdivisionsInInformalPrimarySubdivision.csv").Skip(1))
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
                var subdivisionId = await subdivisionReader.ReadAsync(countryId, parts[11]);
                var topicName = (await termReader.ReadAsync(TOPICS, subdivisionId)).Name;
                yield return new BasicSecondLevelSubdivision
                {
                    Id = null,
                    CreatedDateTime = DateTime.Parse(parts[1]),
                    ChangedDateTime = DateTime.Parse(parts[2]),
                    NodeTypeId = int.Parse(parts[4]),
                    OwnerId = null,
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
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = TOPICS,
                            Name = title,
                            ParentNames = new List<string> { topicName },
                        }
                    },
                    ISO3166_2_Code = parts[10],
                    IntermediateLevelSubdivisionId = subdivisionId,
                    FileIdFlag = null,
                    FileIdTileImage = null,
                };
            }
        }

        private static async IAsyncEnumerable<BasicSecondLevelSubdivision> ReadBasicSecondLevelSubdivisionCsv(NpgsqlConnection connection)
        {
            await using var termReader = await TermReaderByNameableId.CreateAsync(connection);
            await using var subdivisionReader = await SubdivisionIdReaderByIso3166Code.CreateAsync(connection);
            await foreach (string line in System.IO.File.ReadLinesAsync(@"..\..\..\BasicSecondLevelSubdivisions.csv").Skip(1))
            {

                var parts = line.Split(new char[] { ';' });
                var id = int.Parse(parts[0]);
                if (id == 0)
                {
                    NodeId++;
                    id = NodeId;
                }
                var title = parts[8];
                var subdivisionId = await subdivisionReader.ReadAsync(parts[11]);
                var topicName = (await termReader.ReadAsync(TOPICS, subdivisionId)).Name;
                yield return new BasicSecondLevelSubdivision
                {
                    Id = null,
                    CreatedDateTime = DateTime.Parse(parts[1]),
                    ChangedDateTime = DateTime.Parse(parts[2]),
                    NodeTypeId = int.Parse(parts[4]),
                    OwnerId = null,
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
                    CountryId = int.Parse(parts[7]),
                    Title = title,
                    Name = parts[9],
                    Description = "",
                    VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            VocabularyId = TOPICS,
                            Name = title,
                            ParentNames = new List<string> { topicName },
                        }
                    },
                    ISO3166_2_Code = parts[10],
                    IntermediateLevelSubdivisionId = subdivisionId,
                    FileIdFlag = null,
                    FileIdTileImage = null,
                };
            }
        }
        private static async Task MigrateBasicSecondLevelSubdivisions(MySqlConnection mysqlconnection, NpgsqlConnection connection)
        {
            await using var tx = await connection.BeginTransactionAsync();
            try
            {
                await BasicSecondLevelSubdivisionCreator.CreateAsync(ReadBasicSecondLevelSubdivisionsInInformalPrimarySubdivisionCsv(connection), connection);
                await BasicSecondLevelSubdivisionCreator.CreateAsync(ReadBasicSecondLevelSubdivisionCsv(connection), connection);
                await BasicSecondLevelSubdivisionCreator.CreateAsync(ReadBasicSecondLevelSubdivisions(mysqlconnection), connection);
                await tx.CommitAsync();
            }
            catch (Exception)
            {
                await tx.RollbackAsync();
                throw;
            }

        }
        private static async IAsyncEnumerable<BasicSecondLevelSubdivision> ReadBasicSecondLevelSubdivisions(MySqlConnection mysqlconnection)
        {
            var sql = $"""
                SELECT
                    n.nid id,
                    n.uid access_role_id,
                    n.title,
                    n.`status` node_status_id,
                    FROM_UNIXTIME(n.created) created_date_time, 
                    FROM_UNIXTIME(n.changed) changed_date_time,
                    n2.nid intermediate_level_subdivision_id,
                    n2.title subdivision_name,
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

            var reader = await readCommand.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var id = reader.GetInt32("id");
                var title = $"{reader.GetString("title").Replace(" (state)", "")} (state of the USA)";
                var subdivisioName = $"{reader.GetString("subdivision_name")} (region of the USA)";
                var vocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        VocabularyId = TOPICS,
                        Name = title,
                        ParentNames = new List<string>{ subdivisioName },
                    }
                };
                yield return new BasicSecondLevelSubdivision
                {
                    Id = null,
                    PublisherId = reader.GetInt32("access_role_id"),
                    CreatedDateTime = reader.GetDateTime("created_date_time"),
                    ChangedDateTime = reader.GetDateTime("changed_date_time"),
                    Title = title,
                    Name = reader.GetString("title"),
                    OwnerId = null,
                    TenantNodes = new List<TenantNode>
                    {
                        new TenantNode
                        {
                            TenantId = 1,
                            PublicationStatusId = reader.GetInt32("node_status_id"),
                            UrlPath = null,
                            NodeId = null,
                            SubgroupId = null,
                            UrlId = reader.GetInt32("id")
                        }
                    },
                    NodeTypeId = 19,
                    Description = "",
                    VocabularyNames = GetVocabularyNames(TOPICS, id, title, new Dictionary<int, List<VocabularyName>>()),
                    IntermediateLevelSubdivisionId = reader.GetInt32("intermediate_level_subdivision_id"),
                    CountryId = reader.GetInt32("country_id"),
                    ISO3166_2_Code = reader.GetString("iso_3166_2_code"),
                    FileIdFlag = reader.IsDBNull("file_id_flag") ? null : reader.GetInt32("file_id_flag"),
                    FileIdTileImage = null,
                };
            }
            await reader.CloseAsync();
        }

    }
}
