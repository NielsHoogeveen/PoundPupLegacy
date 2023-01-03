using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{


    public static int GetHagueStatus(string name)
    {
        return name switch
        {
            "Albania" => 41215,
            "Andorra" => 41214,
            "Armenia" => 41214,
            "Australia" => 41215,
            "Austria" => 41215,
            "Azerbaijan" => 41214,
            "Belarus" => 41215,
            "Belgium" => 41215,
            "Belize" => 41214,
            "Benin" => 41215,
            "Bolivia" => 41215,
            "Botswana" => 41214,
            "Brazil" => 41215,
            "Bulgaria" => 41215,
            "Burkina Faso" => 41215,
            "Burundi" => 41214,
            "Cape Verde" => 41214,
            "Cambodia" => 41214,
            "Canada" => 41215,
            "Chile" => 41215,
            "China" => 41215,
            "Colombia" => 41215,
            "Democratic Republic of The Congo" => 41214,
            "Costa Rica" => 41215,
            "Croatia" => 41214,
            "Cuba" => 41214,
            "Cyprus" => 41215,
            "Czech Republic" => 41215,
            "Côte d'Ivoire" => 41214,
            "Denmark" => 41215,
            "Dominican Republic" => 41214,
            "Ecuador" => 41215,
            "El Salvador" => 41215,
            "Estonia" => 41214,
            "Eswatini" => 41214,
            "Fiji" => 41214,
            "Finland" => 41215,
            "France" => 41215,
            "Georgia (country)" => 41214,
            "Germany" => 41215,
            "Ghana" => 41214,
            "Greece" => 41215,
            "Guatemala" => 41214,
            "Guinea" => 41214,
            "Guyana" => 41214,
            "Haiti" => 41215,
            "Honduras" => 41215,
            "Hungary" => 41215,
            "Iceland" => 41214,
            "India" => 41215,
            "Ireland" => 41215,
            "Israel" => 41215,
            "Italy" => 41215,
            "Kazakhstan" => 41214,
            "Kenya" => 41214,
            "Kyrgyzstan" => 41214,
            "Latvia" => 41215,
            "Lesotho" => 41214,
            "Liechtenstein" => 41214,
            "Lithuania" => 41214,
            "Luxembourg" => 41215,
            "Madagascar" => 41215,
            "Mali" => 41214,
            "Malta" => 41214,
            "Mauritius" => 41214,
            "Mexico" => 41215,
            "Monaco" => 41214,
            "Mongolia" => 41214,
            "Montenegro" => 41214,
            "Namibia" => 41214,
            "Nepal" => 41213,
            "Netherlands" => 41215,
            "New Zealand" => 41214,
            "Niger" => 41214,
            "North Macedonia" => 41214,
            "Norway" => 41215,
            "Panama" => 41215,
            "Paraguay" => 41214,
            "Peru" => 41215,
            "Philippines" => 41215,
            "Poland" => 41215,
            "Portugal" => 41215,
            "Republic of Korea (South Korea)" => 41215,
            "Moldova" => 41214,
            "Romania" => 41215,
            "Russian Federation" => 41213,
            "Rwanda" => 41214,
            "Saint Kitts & Nevis" => 41214,
            "San Marino" => 41214,
            "Senegal" => 41214,
            "Serbia" => 41214,
            "Seychelles" => 41214,
            "Slovakia" => 41215,
            "Slovenia" => 41215,
            "South Africa" => 41214,
            "Spain" => 41215,
            "Sri Lanka" => 41215,
            "Sweden" => 41215,
            "Switzerland" => 41215,
            "Thailand" => 41215,
            "Togo" => 41214,
            "Turkey" => 41215,
            "United Kingdom" => 41215,
            "United States of America" => 41215,
            "Uruguay" => 41215,
            "Venezuela" => 41215,
            "Vietnam" => 41215,
            "Zambia" => 41214,
            _ => 41213
        };
    }

    private static IEnumerable<BasicCountry> basicCountries = new List<BasicCountry>
    {
        new BasicCountry
        {
            Id = ANTIGUA_AND_BARBUDA,
            AccessRoleId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "Antigua and Barbuda",
            NodeStatusId = 1,
            NodeTypeId = 13,
            Name = "Antigua and Barbuda",
            Description = "",
            VocabularyNames = new List<VocabularyName>(),
            GlobalRegionId = 3822,
            ISO3166_1_Code = "AG",
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
        new BasicCountry
        {
            Id = PALESTINE,
            AccessRoleId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "Palestine",
            NodeStatusId = 1,
            NodeTypeId = 13,
            Name = "Palestine",
            Description = "",
            VocabularyNames = new List<VocabularyName>(),
            GlobalRegionId = 3817,
            ISO3166_1_Code = "PS",
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
        new BasicCountry
        {
            Id = SAINT_HELENA_ETC,
            AccessRoleId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "Saint Helena, Ascension and Tristan da Cunha",
            NodeStatusId = 1,
            NodeTypeId = 13,
            Name = "Saint Helena, Ascension and Tristan da Cunha",
            Description = "",
            VocabularyNames = new List<VocabularyName>(),
            GlobalRegionId = 3825,
            ISO3166_1_Code = "SH",
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
        new BasicCountry
        {
            Id = SOUTH_SUDAN,
            AccessRoleId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "South Sudan",
            NodeStatusId = 1,
            NodeTypeId = 13,
            Name = "South Sudan",
            Description = "",
            VocabularyNames = new List<VocabularyName>(),
            GlobalRegionId = 3827,
            ISO3166_1_Code = "SS",
            FileIdFlag = null,
            FileIdTileImage = null,
            HagueStatusId = 41213,
            ResidencyRequirements = null,
            AgeRequirements = null,
            HealthRequirements = null,
            IncomeRequirements = null,
            MarriageRequirements = null,
            OtherRequirements = null,

        }
    };

    private static void MigrateBasicCountries(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        var countries = basicCountries.Select(x =>
        {
            if (x.Id == 0)
            {
                NodeId++;
                x.Id = NodeId;
            }
            return x;
        });
        BasicCountryCreator.Create(countries, connection);
        BasicCountryCreator.Create(ReadBasicCountries(mysqlconnection), connection);
    }
    private static IEnumerable<BasicCountry> ReadBasicCountries(MySqlConnection mysqlconnection)
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
                    upper(cou.field_country_code_value) iso_3166_code,
                    CASE 
                		WHEN field_residency_requirement_value = '' THEN NULL
                		ELSE field_residency_requirement_value
                    END residency_requirements,
                    CASE
                		WHEN field_age_requirements_value = '' THEN NULL
                		ELSE field_age_requirements_value
                	END age_requirements,
                    CASE
                		WHEN field_marriage_requirements_value = '' THEN NULL
                		ELSE field_marriage_requirements_value
                	END marriage_requirements,
                    CASE
                		WHEN field_income_requirements_value = '' THEN NULL
                		ELSE field_income_requirements_value
                	END income_requirements,
                    CASE
                		WHEN field_health_requirements_value = '' THEN NULL
                		ELSE field_health_requirements_value
                	END health_requirements,
                    CASE
                		WHEN field_other_requirements_value = '' THEN NULL
                		ELSE field_other_requirements_value
                	END other_requirements
                FROM node n 
                JOIN content_type_country_type cou ON cou.nid = n.nid 
                JOIN category_hierarchy ch ON ch.cid = n.nid 
                JOIN node n2 ON n2.nid = ch.parent 
                WHERE n.`type` = 'country_type' 
                AND n2.`type` = 'region_facts' 
                AND n.nid not IN (
                    3980,
                    3981,
                    3935,
                    3903,
                    3908,
                    4044,
                    4057,
                    3887,
                    3879,
                    4063,
                    3878,
                    3891,
                    4055,
                    4048,
                    4053,
                    3914,
                    3920,
                    4000,
                    3992
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
            var name = reader.GetInt32("id") == 3839 ? "Côte d'Ivoire" :
                        reader.GetInt32("id") == 3999 ? "Bosnia and Herzegovina" :
                        reader.GetInt32("id") == 4005 ? "North Macedonia" :
                        reader.GetInt32("id") == 4046 ? "Solomon Islands" :
                        reader.GetInt32("id") == 3884 ? "Eswatini" :
                        reader.GetString("title");

            var country = new BasicCountry
            {
                Id = id,
                AccessRoleId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = name,
                NodeStatusId = reader.GetInt32("status"),
                NodeTypeId = 13,
                Name = name,
                Description = "",
                VocabularyNames = GetVocabularyNames(TOPICS, id, name, new Dictionary<int, List<VocabularyName>>()),
                GlobalRegionId = reader.GetInt32("continental_region_id"),
                ISO3166_1_Code = reader.GetInt32("id") == 3847 ? "NE" :
                              reader.GetInt32("id") == 4010 ? "RS" :
                              reader.GetInt32("id") == 4014 ? "XK" :
                              reader.GetString("iso_3166_code"),
                FileIdFlag = null,
                FileIdTileImage = null,
                HagueStatusId = GetHagueStatus(name),
                ResidencyRequirements = reader.IsDBNull("residency_requirements") ? null : reader.GetString("residency_requirements"),
                AgeRequirements = reader.IsDBNull("age_requirements") ? null : reader.GetString("age_requirements"),
                MarriageRequirements = reader.IsDBNull("marriage_requirements") ? null : reader.GetString("marriage_requirements"),
                HealthRequirements = reader.IsDBNull("health_requirements") ? null : reader.GetString("health_requirements"),
                IncomeRequirements = reader.IsDBNull("income_requirements") ? null : reader.GetString("income_requirements"),
                OtherRequirements = reader.IsDBNull("other_requirements") ? null : reader.GetString("other_requirements"),
            };
            yield return country;

        }
        reader.Close();
    }


}
