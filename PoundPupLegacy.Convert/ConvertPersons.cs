using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{

    private static async Task MigratePersons(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        await using var tx = await connection.BeginTransactionAsync();
        try
        {
            await PersonCreator.CreateAsync(ReadPersons(mysqlconnection), connection);
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
    }
    private static DateTime? GetDateOfDeath(int id, DateTime? dateTime)
    {
        return id switch
        {
            60412 => DateTime.Parse("2022-03-18"),
            10329 => DateTime.Parse("2018-08-25"),
            _ => dateTime
        };
    }

    private static async IAsyncEnumerable<Person> ReadPersons(MySqlConnection mysqlconnection)
    {

        var sql = $"""
                SELECT
                n.nid id,
                n.uid access_role_id,
                n.title,
                n.`status` node_status_id,
                FROM_UNIXTIME(n.created) created_date_time, 
                FROM_UNIXTIME(n.changed) changed_date_time,
                24 node_type_id,
                c.title topic_name,
                CASE WHEN o.field_image_fid = 0 THEN null ELSE o.field_image_fid END file_id_portrait,
                STR_TO_DATE(field_born_value,'%Y-%m-%d') date_of_birth,
                STR_TO_DATE(field_died_value,'%Y-%m-%d') date_of_death
                FROM node n 
                JOIN content_type_adopt_person o ON o.nid = n.nid AND o.vid = n.vid
                LEFT JOIN node n2 ON n2.title = n.title AND n2.nid <> n.nid AND n2.`type` = 'category_cat'
                LEFT JOIN (
                select
                n.nid,
                n.title,
                cc.field_tile_image_title,
                cc.field_related_page_nid,
                p.nid parent_id,
                p.title parent_name
                FROM node n
                JOIN content_type_category_cat cc ON cc.nid = n.nid AND cc.vid = n.vid
                LEFT JOIN (
                    SELECT
                    n.nid, 
                    n.title,
                    ch.cid
                    FROM node n
                    JOIN category_hierarchy ch ON ch.parent = n.nid
                    WHERE n.`type` = 'category_cat'
                ) p ON p.cid = n.nid
                ) c ON c.field_related_page_nid = n.nid
                WHERE n.`type` = 'adopt_person' AND c.title IS NOT null
                """;
        using var readCommand = mysqlconnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var title = reader.GetString("title");
            var topicName = reader.GetString("topic_name");
            var vocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    VocabularyId = TOPICS,
                    Name = topicName,
                    ParentNames = new List<string>(),
                }
            };

            yield return new Person
            {
                Id = reader.GetInt32("id"),
                AccessRoleId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = title,
                NodeStatusId = reader.GetInt32("node_status_id"),
                NodeTypeId = reader.GetInt16("node_type_id"),
                Description = "",
                FileIdTileImage = null,
                VocabularyNames = vocabularyNames,
                DateOfBirth = reader.IsDBNull("date_of_birth") ? null : reader.GetDateTime("date_of_birth"),
                DateOfDeath = GetDateOfDeath(reader.GetInt32("id"), reader.IsDBNull("date_of_death") ? null : reader.GetDateTime("date_of_death")),
                FileIdPortrait = reader.IsDBNull("file_id_portrait") ? null : reader.GetInt32("file_id_portrait"),
            };

        }
        await reader.CloseAsync();
    }
}
