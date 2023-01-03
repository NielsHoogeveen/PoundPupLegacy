using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{


    private static void MigrateBasicNameables(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        BasicNameableCreator.Create(ReadBasicNameables(mysqlconnection), connection);
    }
    private static IEnumerable<BasicNameable> ReadBasicNameables(MySqlConnection mysqlconnection)
    {

        var sql = $"""
                SELECT
                 n.nid id,
                 n.uid user_id,
                 n.title,
                 n.`status`,
                 FROM_UNIXTIME(n.created) created, 
                 FROM_UNIXTIME(n.changed) `changed`,
                 41 node_type_id,
                 nr.body description,
                 n2.`type`,
                case 
                	when field_tile_image_fid = 0 then null
                	ELSE field_tile_image_fid
                END file_id_tile_image
                FROM node n
                JOIN category c ON c.cid = n.nid AND c.cnid = 4126
                JOIN content_type_category_cat cc ON cc.nid = n.nid AND cc.vid = n.vid
                JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                LEFT JOIN node n2 ON n2.title = n.title AND n2.nid <> n.nid AND n2.`type` IN ('adopt_person','country_type', 'adopt_orgs', 'case')
                WHERE cc.field_related_page_nid = 0
                AND n.title NOT IN 
                (
                   'adoption',
                   'foster care',
                   'guardianship',
                   'residential care',
                   'adoption agencies',
                   'church',
                   'boot camp',
                   'media',
                   'blog',
                   'orphanages',
                   'adoption advocates',
                   'boarding school',
                   'adoption facilitators',
                   'maternity homes'
                )
                AND n.nid NOT IN (
                	22589
                )
                AND n2.nid IS  NULL
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
            var country = new BasicNameable
            {
                Id = id,
                AccessRoleId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = name,
                NodeStatusId = reader.GetInt32("status"),
                NodeTypeId = reader.GetInt32("node_type_id"),
                Description = reader.GetString("description"),
                FileIdTileImage = null,
                VocabularyNames = GetVocabularyNames(TOPICS, id, name, new Dictionary<int, List<VocabularyName>>()),
            };
            yield return country;

        }
        reader.Close();
    }
}
