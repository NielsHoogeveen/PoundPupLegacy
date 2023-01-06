using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;
using System.Numerics;
using System.Xml.Linq;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {

        private static async Task MigrateCountryAndFirstLevelSubdivisions(MySqlConnection mysqlconnection, NpgsqlConnection connection)
        {
            await using var tx = await connection.BeginTransactionAsync();
            try
            {
                await CountryAndFirstAndBottomLevelSubdivisionCreator.CreateAsync(CountryAndFirstAndBottomLevelSubdivisions.ToAsyncEnumerable(), connection);
                await CountryAndFirstAndBottomLevelSubdivisionCreator.CreateAsync(ReadCountryAndFirstAndIntermediateLevelSubdivisions(mysqlconnection), connection);
                await tx.CommitAsync();
            }
            catch (Exception)
            {
                await tx.RollbackAsync();
                throw;
            }

        }
        private static string GetISO3166Code2ForCountry(int id)
        {
            return id switch
            {
                3891 => "NL-AW",
                3914 => "US-PR",
                3920 => "US-VI",
                3980 => "CN-HK",
                3981 => "CN-MO",
                4048 => "US-GU",
                4053 => "US-MP",
                4055 => "US-AS",

                3878 => "FR-YT",
                3879 => "FR-RE",
                3887 => "FR-PM",
                3903 => "FR-GP",
                3908 => "FR-MQ",
                3935 => "FR-GF",
                4044 => "FR-NC",
                4057 => "FR-PF",
                4063 => "FR-WF",

                11570 => "GB-ENG",
                11569 => "GB-SCT",
                11571 => "GB-WLS",
                _ => throw new Exception($"No ISO3166-2 code is defined for {id}")
            };
        }
        private static int GetSupervisingCountryId(int id)
        {
            return id switch
            {
                3891 => 4023,
                3914 => 3805,
                3920 => 3805,
                3980 => 3975,
                3981 => 3975,
                4048 => 3805,
                4053 => 3805,
                4055 => 3805,

                3878 => 4018,
                3879 => 4018,
                3887 => 4018,
                3903 => 4018,
                3908 => 4018,
                3935 => 4018,
                4044 => 4018,
                4057 => 4018,
                4063 => 4018,
                _ => throw new Exception($"No supervising country is defined for {id}")
            };
        }

        private static IEnumerable<CountryAndFirstAndBottomLevelSubdivision> CountryAndFirstAndBottomLevelSubdivisions = new List<CountryAndFirstAndBottomLevelSubdivision>
        {
            new CountryAndFirstAndBottomLevelSubdivision
            {
                Id = ALAND,
                Title = "Åland",
                Name = "Åland",
                NodeStatusId = 1,
                NodeTypeId = 15,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                AccessRoleId = 1,
                Description = "",
                VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        VocabularyId = TOPICS,
                        Name = "Åland",
                        ParentNames = new List<string>{ "Northern Europe" },
                    }
                },
                SecondLevelRegionId = 3813,
                CountryId = 3985,
                ISO3166_1_Code = "AX",
                ISO3166_2_Code = "FI-01",
                FileIdFlag = null,
                FileIdTileImage = null,
                HagueStatusId = 41213,
                ResidencyRequirements = null,
                AgeRequirements = null,
                HealthRequirements = null,
                IncomeRequirements = null,
                MarriageRequirements = null,
                OtherRequirements = null,
            },
            new CountryAndFirstAndBottomLevelSubdivision
            {
                Id = CURACAO,
                Title = "Curaçao",
                Name = "Curaçao",
                NodeStatusId = 1,
                NodeTypeId = 15,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                AccessRoleId = 1,
                Description = "",
                VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        VocabularyId = TOPICS,
                        Name = "Curaçao",
                        ParentNames = new List<string>{ "Caribbean" },
                    }
                },
                SecondLevelRegionId = 3809,
                CountryId = 4023,
                ISO3166_1_Code = "CW",
                ISO3166_2_Code = "NL-CW",
                FileIdFlag = null,
                FileIdTileImage = null,
                HagueStatusId = 41213,
                ResidencyRequirements = null,
                AgeRequirements = null,
                HealthRequirements = null,
                IncomeRequirements = null,
                MarriageRequirements = null,
                OtherRequirements = null,          },
            new CountryAndFirstAndBottomLevelSubdivision
            {
                Id = SINT_MAARTEN,
                Title = "Sint Maarten",
                Name = "Sint Maarten",
                NodeStatusId = 1,
                NodeTypeId = 15,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                AccessRoleId = 1,
                Description = "",
                VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        VocabularyId = TOPICS,
                        Name = "Sint Maarten",
                        ParentNames = new List<string>{ "Caribbean" },
                    }
                },
                SecondLevelRegionId = 3809,
                CountryId = 4023,
                ISO3166_1_Code = "SX",
                ISO3166_2_Code = "NL-SX",
                FileIdFlag = null,
                FileIdTileImage = null,
                HagueStatusId = 41213,
                ResidencyRequirements = null,
                AgeRequirements = null,
                HealthRequirements = null,
                IncomeRequirements = null,
                MarriageRequirements = null,
                OtherRequirements = null,
            },
            new CountryAndFirstAndBottomLevelSubdivision
            {
                Id = UNITED_STATES_MINOR_OUTLYING_ISLANDS,
                Title = "United States Minor Outlying Islands",
                Name = "United States Minor Outlying Islands",
                NodeStatusId = 1,
                NodeTypeId = 15,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                AccessRoleId = 1,
                Description = "",
                VocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        VocabularyId = TOPICS,
                        Name = "United States Minor Outlying Islands",
                        ParentNames = new List<string>{ "Oceania" },
                    }
                },
                SecondLevelRegionId = 3822,
                CountryId = 3805,
                ISO3166_1_Code = "UM",
                ISO3166_2_Code = "US-UM",
                FileIdFlag = null,
                FileIdTileImage = null,
                HagueStatusId = 41213,
                ResidencyRequirements = null,
                AgeRequirements = null,
                HealthRequirements = null,
                IncomeRequirements = null,
                MarriageRequirements = null,
                OtherRequirements = null,
            },
        };

        private static async IAsyncEnumerable<CountryAndFirstAndBottomLevelSubdivision> ReadCountryAndFirstAndIntermediateLevelSubdivisions(MySqlConnection mysqlconnection)
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
                upper(cou.field_country_code_value) iso_3166_code
                FROM node n 
                JOIN content_type_country_type cou ON cou.nid = n.nid
                JOIN category_hierarchy ch ON ch.cid = n.nid
                JOIN node n2 ON n2.nid = ch.parent
                WHERE n.`type` = 'country_type'
                AND n2.`type` = 'region_facts'
                AND n.nid IN (
                    3891,
                    3914,
                    3920,
                    3980,
                    3981,
                    4048,
                    4053,
                    4055
                )
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
                        VocabularyId = TOPICS,
                        Name = name,
                        ParentNames = new List<string>{ regionName },
                    }
                };


                yield return new CountryAndFirstAndBottomLevelSubdivision
                {
                    Id = id,
                    AccessRoleId = reader.GetInt32("access_role_id"),
                    CreatedDateTime = reader.GetDateTime("created_date_time"),
                    ChangedDateTime = reader.GetDateTime("changed_date_time"),
                    Title = name,
                    Name = name,
                    NodeStatusId = reader.GetInt32("node_status_id"),
                    NodeTypeId = 15,
                    Description = "",
                    VocabularyNames = vocabularyNames,
                    SecondLevelRegionId = reader.GetInt32("second_level_region_id"),
                    ISO3166_1_Code = reader.GetInt32("id") == 3847 ? "NE" :
                                  reader.GetInt32("id") == 4010 ? "RS" :
                                  reader.GetInt32("id") == 4014 ? "XK" :
                                  reader.GetString("iso_3166_code"),
                    ISO3166_2_Code = GetISO3166Code2ForCountry(reader.GetInt32("id")),
                    CountryId = GetSupervisingCountryId(reader.GetInt32("id")),
                    FileIdFlag = null,
                    FileIdTileImage = null,
                    HagueStatusId = 41213,
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
