using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {
        private static void MigrateBoundCountries(MySqlConnection mysqlconnection, NpgsqlConnection connection)
        {
            BoundCountryCreator.Create(ReadBoundCountries(mysqlconnection), connection);
        }

        private static IEnumerable<BoundCountry> ReadBoundCountries(MySqlConnection mysqlconnection)
        {
            var sql = $"""
                SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`,
                    n2.nid binding_country_id
                FROM node n 
                JOIN content_type_country_type cou ON cou.nid = n.nid
                JOIN category_hierarchy ch ON ch.cid = n.nid
                JOIN node n2 ON n2.nid = ch.parent
                WHERE n.`type` = 'country_type'
                AND n2.`type` = 'country_type'
                AND n.nid <> 11572
                """;
            using var readCommand = mysqlconnection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;

            var reader = readCommand.ExecuteReader();

            while (reader.Read())
            {
                var id = reader.GetInt32("id");
                var name = reader.GetString("title");
                yield return new BoundCountry
                {
                    Id = id,
                    AccessRoleId = reader.GetInt32("user_id"),
                    CreatedDateTime = reader.GetDateTime("created"),
                    ChangedDateTime = reader.GetDateTime("changed"),
                    Title = name,
                    NodeStatusId = reader.GetInt32("status"),
                    NodeTypeId = 14,
                    Description = "",
                    VocabularyNames = GetVocabularyNames(TOPICS, id, name, new Dictionary<int, List<VocabularyName>>()),
                    BindingCountryId = reader.GetInt32("binding_country_id"),
                    Name = name,
                    ISO3166_2_Code = GetISO3166Code2ForCountry(reader.GetInt32("id")),
                    CountryId = reader.GetInt32("binding_country_id"),
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

            }
            reader.Close();
        }
    }
}
