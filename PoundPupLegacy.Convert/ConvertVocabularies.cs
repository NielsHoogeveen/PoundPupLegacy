﻿using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;
using System.Reflection.PortableExecutable;

namespace PoundPupLegacy.Convert;

internal partial class Program
{
    private static IEnumerable<Vocabulary> GetVocabularies()
    {
        return new List<Vocabulary>
        {
            new Vocabulary
            {
                Id = null,
                Name = "Child Placement Type",
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = "Child Placement Type",
                OwnerId = null,
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
                Name = "Type of Abuse",
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = "Type of Abuse",
                OwnerId = null,
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
                Name = "Type of Abuser",
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = "Type of Abuser",
                OwnerId = null,
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
                Name = "Family Size",
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = "Family Size",
                OwnerId = null,
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
                OwnerId = null,
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
