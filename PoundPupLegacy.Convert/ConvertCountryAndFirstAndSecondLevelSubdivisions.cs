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

        private static async IAsyncEnumerable<CountryAndFirstAndSecondLevelSubdivision> GetRegionSubdivisionCountries(NodeIdByUrlIdReader nodeIdReader)
        {
            yield return new CountryAndFirstAndSecondLevelSubdivision
            {
                Id = null,
                Title = "Saint Barthélemy",
                Name = "Saint Barthélemy",
                OwnerId = OWNER_GEOGRAPHY,
                TenantNodes = new List<TenantNode>
                    {
                        new TenantNode
                        {
                            TenantId = 1,
                            PublicationStatusId = 1,
                            UrlPath = null,
                            NodeId = null,
                            SubgroupId = null,
                            UrlId = SAINT_BARTH
                        }
                    },
                NodeTypeId = 16,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                PublisherId = 1,
                Description = "",
                VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            OwnerId = PPL,
                            Name = VOCABULARY_TOPICS,
                            TermName = "Saint Barthélemy",
                            ParentNames = new List<string>{ "Caribbean" },
                        }
                    },
                SecondLevelRegionId = await nodeIdReader.ReadAsync(PPL, 3809),
                CountryId = await nodeIdReader.ReadAsync(PPL, 4018),
                ISO3166_1_Code = "BL",
                ISO3166_2_Code = "FR-BL",
                FileIdFlag = null,
                FileIdTileImage = null,
                HagueStatusId = await nodeIdReader.ReadAsync(PPL, 41213),
                ResidencyRequirements = null,
                AgeRequirements = null,
                HealthRequirements = null,
                IncomeRequirements = null,
                MarriageRequirements = null,
                OtherRequirements = null,

            };
            yield return new CountryAndFirstAndSecondLevelSubdivision
            {
                Id = null,
                Title = "Saint Martin",
                Name = "Saint Martin",
                OwnerId = OWNER_GEOGRAPHY,
                TenantNodes = new List<TenantNode>
                    {
                        new TenantNode
                        {
                            TenantId = 1,
                            PublicationStatusId = 1,
                            UrlPath = null,
                            NodeId = null,
                            SubgroupId = null,
                            UrlId = SAINT_MARTIN
                        }
                    },
                NodeTypeId = 16,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                PublisherId = 1,
                Description = "",
                VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            OwnerId = PPL,
                            Name = VOCABULARY_TOPICS,
                            TermName = "Saint Martin",
                            ParentNames = new List<string>{ "Caribbean" },
                        }
                    },
                SecondLevelRegionId = await nodeIdReader.ReadAsync(PPL, 3809),
                CountryId = await nodeIdReader.ReadAsync(PPL, 4018),
                ISO3166_1_Code = "MF",
                ISO3166_2_Code = "FR-MF",
                FileIdFlag = null,
                FileIdTileImage = null,
                HagueStatusId = await nodeIdReader.ReadAsync(PPL, 41213),
                ResidencyRequirements = null,
                AgeRequirements = null,
                HealthRequirements = null,
                IncomeRequirements = null,
                MarriageRequirements = null,
                OtherRequirements = null,
            };
            yield return new CountryAndFirstAndSecondLevelSubdivision
            {
                Id = null,
                Title = "French Southern Territories",
                Name = "French Southern Territories",
                OwnerId = OWNER_GEOGRAPHY,
                TenantNodes = new List<TenantNode>
                    {
                        new TenantNode
                        {
                            TenantId = 1,
                            PublicationStatusId = 1,
                            UrlPath = null,
                            NodeId = null,
                            SubgroupId = null,
                            UrlId = FRENCH_SOUTHERN_TERRITORIES
                        }
                    },
                NodeTypeId = 15,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                PublisherId = 1,
                Description = "",
                VocabularyNames = new List<VocabularyName>
                    {
                        new VocabularyName
                        {
                            OwnerId = PPL,
                            Name = VOCABULARY_TOPICS,
                            TermName = "French Southern Territories",
                            ParentNames = new List<string>{ "Southern Africa" },
                        }
                    },
                SecondLevelRegionId = await nodeIdReader.ReadAsync(PPL, 3828),
                CountryId = await nodeIdReader.ReadAsync(PPL, 4018),
                ISO3166_1_Code = "TF",
                ISO3166_2_Code = "FR-TF",
                FileIdFlag = null,
                FileIdTileImage = null,
                HagueStatusId = await nodeIdReader.ReadAsync(PPL, 41213),
                ResidencyRequirements = null,
                AgeRequirements = null,
                HealthRequirements = null,
                IncomeRequirements = null,
                MarriageRequirements = null,
                OtherRequirements = null,
            };
        }

        private static async Task MigrateCountryAndFirstAndSecondLevelSubdivisions(MySqlConnection mysqlconnection, NpgsqlConnection connection)
        {
            await using var nodeIdReader = await NodeIdByUrlIdReader.CreateAsync(connection);
            await using var tx = await connection.BeginTransactionAsync();
            try
            {
                await CountryAndFirstAndSecondLevelSubdivisionCreator.CreateAsync(GetRegionSubdivisionCountries(nodeIdReader), connection);
                await CountryAndFirstAndSecondLevelSubdivisionCreator.CreateAsync(ReadCountryAndFirstAndSecondLevelSubdivision(mysqlconnection, nodeIdReader), connection);
                await tx.CommitAsync();
            }
            catch (Exception)
            {
                await tx.RollbackAsync();
                throw;
            }
        }
        private static async IAsyncEnumerable<CountryAndFirstAndSecondLevelSubdivision> ReadCountryAndFirstAndSecondLevelSubdivision(MySqlConnection mysqlconnection, NodeIdByUrlIdReader nodeIdReader)
        {


            var sql = $"""
                SELECT
                    n.nid id,
                    n.uid access_role_id,
                    n.title,
                    n.`status` node_status_id,
                    FROM_UNIXTIME(n.created) created_date_time, 
                    FROM_UNIXTIME(n.changed) changed_date_time,
                    n2.nid second_level_region_id,
                    n2.title second_level_region_name,
                    upper(cou.field_country_code_value) iso_3166_code,
                    ua.dst url_path
                FROM node n 
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                JOIN content_type_country_type cou ON cou.nid = n.nid
                JOIN category_hierarchy ch ON ch.cid = n.nid
                JOIN node n2 ON n2.nid = ch.parent
                WHERE n.`type` = 'country_type'
                AND n2.`type` = 'region_facts'
                AND n.nid IN (
                    3935,
                    3903,
                    3908,
                    4044,
                    4057,
                    3887,
                    3879,
                    4063,
                    3878)
                """;
            using var readCommand = mysqlconnection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;


            var reader = await readCommand.ExecuteReaderAsync();


            while (await reader.ReadAsync())
            {
                var id = reader.GetInt32("id");
                var name = reader.GetInt32("id") == 3879 ? "Réunion" :
                            reader.GetString("title");
                var regionName = reader.GetString("second_level_region_name");
                var vocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = PPL,
                        Name = VOCABULARY_TOPICS,
                        TermName = name,
                        ParentNames = new List<string>{ regionName },
                    }
                };


                yield return new CountryAndFirstAndSecondLevelSubdivision
                {
                    Id = null,
                    PublisherId = reader.GetInt32("access_role_id"),
                    CreatedDateTime = reader.GetDateTime("created_date_time"),
                    ChangedDateTime = reader.GetDateTime("changed_date_time"),
                    Title = name,
                    Name = name,
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
                    NodeTypeId = 16,
                    Description = "",
                    VocabularyNames = vocabularyNames,
                    SecondLevelRegionId = await nodeIdReader.ReadAsync(PPL, reader.GetInt32("second_level_region_id")),
                    ISO3166_1_Code = id == 3847 ? "NE" :
                                     id == 4010 ? "RS" :
                                     id == 4014 ? "XK" :
                                     reader.GetString("iso_3166_code"),
                    ISO3166_2_Code = GetISO3166Code2ForCountry(id),
                    CountryId = await nodeIdReader.ReadAsync(PPL, GetSupervisingCountryId(id)),
                    FileIdFlag = null,
                    FileIdTileImage = null,
                    HagueStatusId = await nodeIdReader.ReadAsync(PPL, 41213),
                    ResidencyRequirements = null,
                    AgeRequirements = null,
                    HealthRequirements = null,
                    IncomeRequirements = null,
                    MarriageRequirements = null,
                    OtherRequirements = null,
                };
            }
            await reader.CloseAsync();
        }
    }
}
