using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{

    private static IEnumerable<Vocabulary> GetVocabularies()
    {
        return new List<Vocabulary>
        {
            new Vocabulary
            {
                Id = CHILD_PLACEMENT_TYPE,
                Name = "Child Placement Type",
                AccessRoleId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = "Child Placement Type",
                NodeStatusId = 1,
                NodeTypeId = 36,
                Description = ""
            },
            new Vocabulary
            {
                Id = TYPE_OF_ABUSE,
                Name = "Type of Abuse",
                AccessRoleId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = "Type of Abuse",
                NodeStatusId = 1,
                NodeTypeId = 36,
                Description = ""
            },
            new Vocabulary
            {
                Id = TYPE_OF_ABUSER,
                Name = "Type of Abuser",
                AccessRoleId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = "Type of Abuser",
                NodeStatusId = 1,
                NodeTypeId = 36,
                Description = ""
            },
            new Vocabulary
            {
                Id = FAMILY_SIZE,
                Name = "Family Size",
                AccessRoleId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = "Family Size",
                NodeStatusId = 1,
                NodeTypeId = 36,
                Description = ""
            },
         };
    }

    private static string GetVocabularyName(int id, string name)
    {
        return id switch
        {
            3797 => "Geographical Entity",
            12622 => "Organization Type",
            12637 => "Interorganizational Relation Type",
            12652 => "Political Entity Relation Type",
            12663 => "Person Organization Relation Type",
            16900 => "Interpersonal Relation Type",
            27213 => "Profession",
            39428 => "Denomination",
            41212 => "Hague status",
            42416 => "Document type",
            _ => name
        };
    }

    private static async Task MigrateVocabularies(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        await VocabularyCreator.CreateAsync(GetVocabularies(), connection);
        await VocabularyCreator.CreateAsync(ReadVocabularies(mysqlconnection), connection);
    }
    private static IEnumerable<Vocabulary> ReadVocabularies(MySqlConnection mysqlconnection)
    {

        var sql = $"""
            SELECT
                n.nid id,
                n.uid access_role_id,
                n.title,
                n.`status` node_status_id,
                FROM_UNIXTIME(n.created) created_date_time, 
                FROM_UNIXTIME(n.changed) changed_date_time,
                n.title `name`,
                nr.body description
            FROM node n
            JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
            WHERE n.`type` = 'category_cont' AND n.nid not in (220, 12707, 42422)
            """;
        using var readCommand = mysqlconnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = readCommand.ExecuteReader();

        while (reader.Read())
        {
            var id = reader.GetInt32("id");
            var name = reader.GetString("name");
            yield return new Vocabulary
            {
                Id = id,
                AccessRoleId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = GetVocabularyName(id, name),
                NodeStatusId = reader.GetInt32("node_status_id"),
                NodeTypeId = 38,
                Name = GetVocabularyName(id, name),
                Description = reader.GetString("description"),
            };

        }
        reader.Close();
    }
}
