using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{

    private static async Task MigrateTypesOfAbusers(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        await using var tx = await connection.BeginTransactionAsync();
        try
        {
            await TypeOfAbuserCreator.CreateAsync(ReadTypesOfAbusers(mysqlconnection), connection);
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
    }
    private static async IAsyncEnumerable<TypeOfAbuser> ReadTypesOfAbusers(MySqlConnection mysqlconnection)
    {

        var sql = $"""
                SELECT
                t.id,
                1 access_role_id,
                NOW() created_date_time,
                NOW() changed_date_time,
                t.title,
                1 status,
                40 node_type_id,
                case 
                  when nr.body IS NULL then ''
                  ELSE nr.body
                	end description,
                t.topic_name,
                t.parent_topic_name,
                case 
                when cc.field_tile_image_fid = 0 then null
                ELSE cc.field_tile_image_fid 
                end file_id_tile_image,
                ua.dst url_path
                FROM(
                SELECT {ADOPTIVE_FATHER} AS id, 'Adoptive father' AS title, 'adoptive father' AS topic_name, 'adoptive parents' AS parent_topic_name
                UNION
                SELECT {FOSTER_FATHER}, 'Foster father', 'foster father', 'foster parents'
                UNION
                SELECT {ADOPTIVE_MOTHER}, 'Adoptive mother', 'adoptive mother', NULL
                UNION
                SELECT {FOSTER_MOTHER}, 'Foster mother', 'foster mother', 'foster parents'
                UNION
                SELECT {LEGAL_GUARDIAN}, 'Legal guardian', 'legal guardian', NULL
                UNION
                SELECT {ADOPTED_SIBLING}, 'Adopted sibling', NULL , NULL
                UNION
                SELECT {FOSTER_SIBLING}, 'Foster sibling', NULL , null
                UNION
                SELECT {NON_ADOPTED_SIBLING}, 'Non-adopted sibling', NULL , NULL
                UNION
                SELECT {NON_FOSTERED_SIBLING}, 'Non-fostered sibling', NULL , NULL
                UNION
                SELECT {OTHER_FAMILY_MEMBER}, 'Other family member', NULL , NULL
                UNION
                SELECT {OTHER_NON_FAMILY_MEMBER}, 'Other non-family member', NULL , NULL
                UNION
                SELECT {UNDETERMINED}, 'Undetermined', NULL , null
                ) AS t
                LEFT JOIN node n ON n.title = t.topic_name AND n.`type` = 'category_cat'
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                LEFT JOIN category c ON c.cid = n.nid AND c.cnid = 4126
                LEFT JOIN content_type_category_cat cc on cc.nid = n.nid AND cc.vid = n.vid
                LEFT JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
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
            var topicName = reader.IsDBNull("topic_name") ? null : reader.GetString("topic_name");
            var parentTopicName = reader.IsDBNull("parent_topic_name") ? null : reader.GetString("parent_topic_name");

            var vocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = OWNER_CASES,
                    Name = VOCABULARY_TYPE_OF_ABUSER,
                    TermName = name,
                    ParentNames = new List<string>(),
                }
            };
            if (topicName != null)
            {
                var lst = new List<string>();
                if (parentTopicName != null)
                {
                    lst.Add(parentTopicName);
                }

                vocabularyNames.Add(new VocabularyName
                {
                    OwnerId = PPL,
                    Name = VOCABULARY_TOPICS,
                    TermName = topicName,
                    ParentNames = lst
                });
            }

            yield return new TypeOfAbuser
            {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = name,
                OwnerId = OWNER_CASES,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        TenantId = 1,
                        PublicationStatusId = reader.GetInt32("status"),
                        UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                NodeTypeId = reader.GetInt32("node_type_id"),
                Description = reader.GetString("description"),
                FileIdTileImage = reader.IsDBNull("file_id_tile_image") ? null : reader.GetInt32("file_id_tile_image"),
                VocabularyNames = vocabularyNames,
            };

        }
        await reader.CloseAsync();
    }

}
