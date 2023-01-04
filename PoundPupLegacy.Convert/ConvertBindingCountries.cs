using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{


    private static async Task MigrateBindingCountries(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        await BindingCountryCreator.CreateAsync(ReadBindingCountries(mysqlconnection), connection);
    }
    private static async IAsyncEnumerable<BindingCountry> ReadBindingCountries(MySqlConnection mysqlconnection)
    {

        var sql = $"""
                SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`, 
                    n2.nid global_region_id,
                    upper(cou.field_country_code_value) iso_3166_1_code
                FROM node n 
                JOIN content_type_country_type cou ON cou.nid = n.nid 
                JOIN category_hierarchy ch ON ch.cid = n.nid 
                JOIN node n2 ON n2.nid = ch.parent 
                WHERE n.`type` = 'country_type' 
                AND n2.`type` = 'region_facts' 
                AND n.nid IN (
                    3992
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
            var name = reader.GetString("title");
            var country = new BindingCountry
            {
                Id = id,
                AccessRoleId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = name,
                NodeStatusId = reader.GetInt32("status"),
                NodeTypeId = 20,
                Description = "",
                VocabularyNames = GetVocabularyNames(TOPICS, id, name, new Dictionary<int, List<VocabularyName>>()),
                Name = name,
                GlobalRegionId = reader.GetInt32("global_region_id"),
                ISO3166_1_Code = reader.GetString("iso_3166_1_code"),
                FileIdFlag = null,
                FileIdTileImage = null,
                HagueStatusId = 41215,
                ResidencyRequirements = null,
                AgeRequirements = null,
                HealthRequirements = null,
                IncomeRequirements = null,
                MarriageRequirements = null,
                OtherRequirements = null,
            };
            yield return country;

        }
        await reader.CloseAsync();
    }


}
