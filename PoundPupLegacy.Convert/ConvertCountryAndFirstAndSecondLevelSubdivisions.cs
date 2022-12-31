using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {

        private static List<CountryAndFirstAndSecondLevelSubdivision> RegionSubdivisionCountries = new List<CountryAndFirstAndSecondLevelSubdivision>
        {
            new CountryAndFirstAndSecondLevelSubdivision
            {
                Id = SAINT_BARTH,
                Title = "Saint Barthélemy",
                Name = "Saint Barthélemy",
                NodeStatusId = 1,
                NodeTypeId = 16,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                AccessRoleId = 1,
                VocabularyId = 4126,
                GlobalRegionId = 3809,
                CountryId = 4018,
                ISO3166_1_Code = "BL",
                ISO3166_2_Code = "FR-BL",
                FileIdFlag = null,
                HagueStatusId = 41213,
                ResidencyRequirements = null,
                AgeRequirements = null,
                HealthRequirements = null,
                IncomeRequirements = null,
                MarriageRequirements = null,
                OtherRequirements = null,

            },
            new CountryAndFirstAndSecondLevelSubdivision
            {
                Id = SAINT_MARTIN,
                Title = "Saint Martin",
                Name = "Saint Martin",
                NodeStatusId = 1,
                NodeTypeId = 16,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                AccessRoleId = 1,
                VocabularyId = 4126,
                GlobalRegionId = 3809,
                CountryId = 4018,
                ISO3166_1_Code = "MF",
                ISO3166_2_Code = "FR-MF",
                FileIdFlag = null,
                HagueStatusId = 41213,
                ResidencyRequirements = null,
                AgeRequirements = null,
                HealthRequirements = null,
                IncomeRequirements = null,
                MarriageRequirements = null,
                OtherRequirements = null,
            },
            new CountryAndFirstAndSecondLevelSubdivision
            {
                Id = FRENCH_SOUTHERN_TERRITORIES,
                Title = "French Southern Territories",
                Name = "French Southern Territories",
                NodeStatusId = 1,
                NodeTypeId = 15,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                AccessRoleId = 1,
                VocabularyId = 4126,
                GlobalRegionId = 3828,
                CountryId = 4018,
                ISO3166_1_Code = "TF",
                ISO3166_2_Code = "FR-TF",
                FileIdFlag = null,
                HagueStatusId = 41213,
                ResidencyRequirements = null,
                AgeRequirements = null,
                HealthRequirements = null,
                IncomeRequirements = null,
                MarriageRequirements = null,
                OtherRequirements = null,
            },

        };

        private static void MigrateCountryAndFirstAndSecondLevelSubdivisions(MySqlConnection mysqlconnection, NpgsqlConnection connection)
        {
            foreach (var country in RegionSubdivisionCountries)
            {
                NodeId++;
                country.Id = NodeId;
            }
            CountryAndFirstAndSecondLevelSubdivisionCreator.Create(RegionSubdivisionCountries, connection);
            CountryAndFirstAndSecondLevelSubdivisionCreator.Create(ReadCountryAndFirstAndSecondLevelSubdivision(mysqlconnection), connection);
        }
        private static IEnumerable<CountryAndFirstAndSecondLevelSubdivision> ReadCountryAndFirstAndSecondLevelSubdivision(MySqlConnection mysqlconnection)
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


            var reader = readCommand.ExecuteReader();


            while (reader.Read())
            {
                yield return new CountryAndFirstAndSecondLevelSubdivision
                {
                    Id = reader.GetInt32("id"),
                    AccessRoleId = reader.GetInt32("user_id"),
                    CreatedDateTime = reader.GetDateTime("created"),
                    ChangedDateTime = reader.GetDateTime("changed"),
                    Title = reader.GetInt32("id") == 3879 ? "Réunion" :
                            reader.GetString("title"),
                    Name = reader.GetInt32("id") == 3879 ? "Réunion" :
                            reader.GetString("title"),
                    NodeStatusId = reader.GetInt32("status"),
                    NodeTypeId = 16,
                    VocabularyId = 4126,
                    GlobalRegionId = reader.GetInt32("continental_region_id"),
                    ISO3166_1_Code = reader.GetInt32("id") == 3847 ? "NE" :
                                  reader.GetInt32("id") == 4010 ? "RS" :
                                  reader.GetInt32("id") == 4014 ? "XK" :
                                  reader.GetString("iso_3166_code"),
                    ISO3166_2_Code = GetISO3166Code2ForCountry(reader.GetInt32("id")),
                    CountryId = GetSupervisingCountryId(reader.GetInt32("id")),
                    FileIdFlag = null,
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
