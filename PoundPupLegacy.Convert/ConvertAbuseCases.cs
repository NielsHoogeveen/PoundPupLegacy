using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{


    private static void MigrateAbuseCases(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        AbuseCaseCreator.Create(ReadAbuseCases(mysqlconnection), connection);
    }
    private static IEnumerable<AbuseCase> ReadAbuseCases(MySqlConnection mysqlconnection)
    {

        var sql = $"""
                SELECT
                n.nid id,
                n.uid user_id,
                n.title,
                n.`status`,
                FROM_UNIXTIME(n.created) created, 
                FROM_UNIXTIME(n.changed) `changed`,
                26 node_type_id,
                cc.nid IS NOT null is_topic,
                c.field_discovery_date_value `date`,
                c.field_body_0_value description,
                case 
                	when field_child_placement_type_value = 'Adoption' then 106
                	when field_child_placement_type_value = 'Foster care' then 107
                	when field_child_placement_type_value = 'To be adopted' then 108
                	when field_child_placement_type_value = 'Legal Guardianship' then 109
                	when field_child_placement_type_value = 'Institution' then 110
                END child_placement_type_id,
                case 
                	when c.field_family_size_value = '1 to 4' then 111
                	when c.field_family_size_value = '4 to 8' then 112
                	when c.field_family_size_value = '8 to 12' then 113
                	when c.field_family_size_value = 'more than 12' then 114
                	when field_child_placement_type_value = '' then null
                	when field_child_placement_type_value = null then null
                END family_size_id,
                case when c.field_home_schooling_value = 'yes' then true else null END home_schooling_involved,
                case when field_fundamentalist_faith_value = 'yes' then TRUE ELSE NULL END fundamental_faith_involved,
                case when field_disabilities_value = 'yes' then TRUE ELSE NULL END disabilities_involved
                FROM node n
                JOIN content_type_case c ON c.nid = n.nid AND c.vid = n.vid
                LEFT JOIN content_type_category_cat cc ON cc.field_related_page_nid = n.nid AND cc.nid <> 44518
                LEFT JOIN node n2 ON n2.nid = cc.nid AND n2.vid = cc.vid
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
            var country = new AbuseCase
            {
                Id = id,
                AccessRoleId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = name,
                NodeStatusId = reader.GetInt32("status"),
                NodeTypeId = reader.GetInt32("node_type_id"),
                Date = reader.IsDBNull("date") ? null : StringToDateTimeRange(reader.GetString("date")),
                Description = reader.GetString("description"),
                ChildPlacementTypeId = reader.GetInt32("child_placement_type_id"),
                FamilySizeId = reader.IsDBNull("family_size_id") ? null : reader.GetInt32("family_size_id"),
                HomeschoolingInvolved = reader.IsDBNull("home_schooling_involved") ? null : reader.GetBoolean("home_schooling_involved"),
                FundamentalFaithInvolved = reader.IsDBNull("fundamental_faith_involved") ? null : reader.GetBoolean("fundamental_faith_involved"),
                DisabilitiesInvolved = reader.IsDBNull("disabilities_involved") ? null : reader.GetBoolean("disabilities_involved"),
                VocabularyNames = GetVocabularyNames(TOPICS, id, name, new Dictionary<int, List<VocabularyName>>()),
            };
            yield return country;

        }
        reader.Close();
    }
}
