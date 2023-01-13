using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{

    const string VOCABULARY_CHILD_PLACEMENT_TYPE = "Child Placement Type";
    const string VOCABULARY_TYPE_OF_ABUSE = "Type of Abuse";
    const string VOCABULARY_TYPE_OF_ABUSER = "Type of Abuser";
    const string VOCABULARY_FAMILY_SIZE = "Family Size";

    private static IEnumerable<Vocabulary> GetVocabularies()
    {
        return new List<Vocabulary>
        {
            new Vocabulary
            {
                Id = null,
                Name = VOCABULARY_CHILD_PLACEMENT_TYPE,
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = VOCABULARY_CHILD_PLACEMENT_TYPE,
                OwnerId = OWNER_CASES,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = CHILD_PLACEMENT_TYPE
                    }
                },
                NodeTypeId = 36,
                Description = ""
            },
            new Vocabulary
            {
                Id = null,
                Name = VOCABULARY_TYPE_OF_ABUSE,
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = VOCABULARY_TYPE_OF_ABUSE,
                OwnerId = OWNER_CASES,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = TYPE_OF_ABUSE
                    }
                },
                NodeTypeId = 36,
                Description = ""
            },
            new Vocabulary
            {
                Id = null,
                Name = VOCABULARY_TYPE_OF_ABUSER,
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = VOCABULARY_TYPE_OF_ABUSER,
                OwnerId = OWNER_CASES,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = TYPE_OF_ABUSER
                    }
                },
                NodeTypeId = 36,
                Description = ""
            },
            new Vocabulary
            {
                Id = null,
                Name = VOCABULARY_FAMILY_SIZE,
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = VOCABULARY_FAMILY_SIZE,
                OwnerId = OWNER_CASES,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = FAMILY_SIZE
                    }
                },
                NodeTypeId = 36,
                Description = ""
            },
         };
    }

    const string VOCABULARY_GEOGRAPHICAL_ENTITY = "Geographical Entity";
    const string VOCABULARY_ORGANIZATION_TYPE = "Organization Type";
    const string VOCABULARY_INTERORGANIZATIONAL_RELATION_TYPE = "Interorganizational Relation Type";
    const string VOCABULARY_POLITICAL_ENTITY_RELATION_TYPE = "Political Entity Relation Type";
    const string VOCABULARY_PERSON_ORGANIZATION_RELATION_TYPE = "Person Organization Relation Type";
    const string VOCABULARY_INTERPERSONAL_RELATION_TYPE = "Interpersonal Relation Type";
    const string VOCABULARY_PROFESSION = "Profession";
    const string VOCABULARY_DENOMINATION = "Denomination";
    const string VOCABULARY_HAGUE_STATUS = "Hague status";
    const string VOCABULARY_DOCUMENT_TYPE = "Document type";
    const string VOCABULARY_TOPICS = "Topics";

    private static string GetVocabularyName(int id, string name)
    {
        return id switch
        {
            3797 => VOCABULARY_GEOGRAPHICAL_ENTITY,
            4126 => VOCABULARY_TOPICS,
            12622 => VOCABULARY_ORGANIZATION_TYPE,
            12637 => VOCABULARY_INTERORGANIZATIONAL_RELATION_TYPE,
            12652 => VOCABULARY_POLITICAL_ENTITY_RELATION_TYPE,
            12663 => VOCABULARY_PERSON_ORGANIZATION_RELATION_TYPE,
            16900 => VOCABULARY_INTERPERSONAL_RELATION_TYPE,
            27213 => VOCABULARY_PROFESSION,
            39428 => VOCABULARY_DENOMINATION,
            41212 => VOCABULARY_HAGUE_STATUS,
            42416 => VOCABULARY_DOCUMENT_TYPE,
            _ => name
        };
    }
    private static int GetOwner(int id)
    {
        return id switch
        {
            3797 => OWNER_GEOGRAPHY,
            12622 => OWNER_PARTIES,
            12637 => OWNER_PARTIES,
            12652 => OWNER_PARTIES,
            12663 => OWNER_PARTIES,
            16900 => OWNER_PARTIES,
            27213 => OWNER_PARTIES,
            39428 => OWNER_PARTIES,
            41212 => OWNER_PARTIES,
            42416 => OWNER_DOCUMENTATION,
            _ => PPL
        };
    }
    private static async Task MigrateVocabularies(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        await using var tx = await connection.BeginTransactionAsync();
        try
        {
            await VocabularyCreator.CreateAsync(GetVocabularies().ToAsyncEnumerable(), connection);
            await VocabularyCreator.CreateAsync(ReadVocabularies(mysqlconnection), connection);
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
    }
    private static async IAsyncEnumerable<Vocabulary> ReadVocabularies(MySqlConnection mysqlconnection)
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


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var id = reader.GetInt32("id");
            var name = reader.GetString("name");
            yield return new Vocabulary
            {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = GetVocabularyName(id, name),
                OwnerId = GetOwner(id),
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        TenantId = 1,
                        PublicationStatusId = reader.GetInt32("node_status_id"),
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                NodeTypeId = 38,
                Name = GetVocabularyName(id, name),
                Description = reader.GetString("description"),
            };

        }
        await reader.CloseAsync();
    }
}
