using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{
    private static List<Organization> GetOrganizations()
    {
        return new List<Organization>
        {
            new Organization
            {
                Id = COLORADO_ADOPTION_CENTER,
                AccessRoleId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = "Colorado Adoption Center",
                NodeStatusId = 1,
                NodeTypeId = 23,
                WebsiteURL = null,
                EmailAddress = null,
                Established = null,
                Terminated = null,
                Description = "",
                FileIdTileImage = null,
                VocabularyNames = new List<VocabularyName>(),
            }
        };
    }

    private static async Task MigrateOrganizations(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        var organizations = GetOrganizations();
        foreach (var org in organizations)
        {
            if (org.Id == 0)
            {
                NodeId++;
                org.Id = NodeId;
            }
        }
        await OrganizationCreator.CreateAsync(organizations, connection);
        await OrganizationCreator.CreateAsync(ReadOrganizations(mysqlconnection), connection);
    }
    private static IEnumerable<Organization> ReadOrganizations(MySqlConnection mysqlconnection)
    {

        var sql = $"""
                SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`,
                    23 node_type_id,
                    CASE WHEN field_related_topic_nid IS NULL THEN FALSE ELSE TRUE END is_topic,
                    o.field_website_2_url website_url,
                    FROM_UNIXTIME(o.field_start_date_0_value) established, 
                    FROM_UNIXTIME(UNIX_TIMESTAMP(o.field_end_date_value)) `terminated`,
                    o.field_email_address_email email_address,
                    o.field_description_3_value description
                FROM node n 
                JOIN content_type_adopt_orgs o ON o.nid = n.nid AND o.vid = n.vid
                WHERE n.`type` = 'adopt_orgs'
                """;
        using var readCommand = mysqlconnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = readCommand.ExecuteReader();

        while (reader.Read())
        {
            yield return new Organization
            {
                Id = reader.GetInt32("id"),
                AccessRoleId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = reader.GetString("title"),
                NodeStatusId = reader.GetInt32("status"),
                NodeTypeId = reader.GetInt16("node_type_id"),
                WebsiteURL = reader.IsDBNull("website_url") ? null : reader.GetString("website_url"),
                EmailAddress = reader.IsDBNull("email_address") ? null : reader.GetString("email_address"),
                Description = reader.IsDBNull("description") ? "" : reader.GetString("description"),
                Established = reader.IsDBNull("established") ? null : reader.GetDateTime("established"),
                Terminated = reader.IsDBNull("terminated") ? null : reader.GetDateTime("terminated"),
                FileIdTileImage = null,
                VocabularyNames = new List<VocabularyName>(),
            };

        }
        reader.Close();
    }


}
