using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {

        private static async Task MigrateBasicCountryAndFirstLevelSubdivisions(MySqlConnection mysqlconnection, NpgsqlConnection connection)
        {
            await CountryAndFirstAndBottomLevelSubdivisionCreator.CreateAsync(CountryAndFirstAndBottomLevelSubdivisions, connection);
            await CountryAndFirstAndBottomLevelSubdivisionCreator.CreateAsync(ReadCountryAndFirstAndIntermediateLevelSubdivisions(mysqlconnection), connection);
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
                VocabularyNames = GetVocabularyNames(TOPICS,ALAND, "Åland", new Dictionary<int, List<VocabularyName>>()),
                GlobalRegionId = 3813,
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
                VocabularyNames = GetVocabularyNames(TOPICS,CURACAO, "Curaçao", new Dictionary<int, List<VocabularyName>>()),
                GlobalRegionId = 3809,
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
                VocabularyNames = GetVocabularyNames(TOPICS,SINT_MAARTEN, "Sint Maarten", new Dictionary<int, List<VocabularyName>>()),
                GlobalRegionId = 3809,
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
                VocabularyNames = GetVocabularyNames(TOPICS,UNITED_STATES_MINOR_OUTLYING_ISLANDS, "United States Minor Outlying Islands", new Dictionary<int, List<VocabularyName>>()),
                GlobalRegionId = 3822,
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

        private static IEnumerable<CountryAndFirstAndBottomLevelSubdivision> ReadCountryAndFirstAndIntermediateLevelSubdivisions(MySqlConnection mysqlconnection)
        {


            var sql = $"""
            SELECT
                n.nid id,
                n.uid user_id,
                n.title,
                n.`status`,
                FROM_UNIXTIME(n.created) created, 
                FROM_UNIXTIME(n.changed) `changed`,
                n2.nid continental_region_id,
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


            var reader = readCommand.ExecuteReader();

            while (reader.Read())
            {
                var id = reader.GetInt32("id");
                var name = reader.GetInt32("id") == 3879 ? "Réunion" :
                           reader.GetString("title");

                yield return new CountryAndFirstAndBottomLevelSubdivision
                {
                    Id = id,
                    AccessRoleId = reader.GetInt32("user_id"),
                    CreatedDateTime = reader.GetDateTime("created"),
                    ChangedDateTime = reader.GetDateTime("changed"),
                    Title = name,
                    Name = name,
                    NodeStatusId = reader.GetInt32("status"),
                    NodeTypeId = 15,
                    Description = "",
                    VocabularyNames = GetVocabularyNames(TOPICS, id, name, new Dictionary<int, List<VocabularyName>>()),
                    GlobalRegionId = reader.GetInt32("continental_region_id"),
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
            reader.Close();
        }
    }
}
