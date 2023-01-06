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
        await using var tx = await connection.BeginTransactionAsync();
        try
        {
            await BindingCountryCreator.CreateAsync(ReadBindingCountries(mysqlconnection), connection);
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
    }
    private static async IAsyncEnumerable<BindingCountry> ReadBindingCountries(MySqlConnection mysqlconnection)
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
            var regionName = reader.GetString("second_level_region_name");

            var vocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        VocabularyId = TOPICS,
                        Name = name,
                        ParentNames = new List<string>{ regionName},
                    }
                };

            var country = new BindingCountry
            {
                Id = id,
                AccessRoleId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = name,
                NodeStatusId = reader.GetInt32("node_status_id"),
                NodeTypeId = 20,
                Description = "",
                VocabularyNames = vocabularyNames,
                Name = name,
                SecondLevelRegionId = reader.GetInt32("second_level_region_id"),
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
