using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{

    private static async Task MigrateHagueStatuses(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        await using var tx = await connection.BeginTransactionAsync();
        try
        {
            await HagueStatusCreator.CreateAsync(ReadHagueStatuses(mysqlconnection), connection);
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    private static async IAsyncEnumerable<HagueStatus> ReadHagueStatuses(MySqlConnection mysqlconnection)
    {

        var sql = $"""
                    SELECT
                       n.nid id,
                       n.uid access_role_id,
                       n.title,
                       n.`status` node_status_id,
                       FROM_UNIXTIME(n.created) created_date_time, 
                       FROM_UNIXTIME(n.changed) changed_date_time,
                       ua.dst url_path
                    FROM node n
                    LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                    JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                    JOIN category c ON c.cid = n.nid AND c.cnid = 41212
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
            var vocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        OwnerId = OWNER_PARTIES,
                        Name = VOCABULARY_HAGUE_STATUS,
                        TermName = name,
                        ParentNames = new List<string>(),
                    }
                };

            yield return new HagueStatus
            {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = name,
                OwnerId = OWNER_PARTIES,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        TenantId = 1,
                        PublicationStatusId = reader.GetInt32("node_status_id"),
                        UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                NodeTypeId = 8,
                Description = "",
                FileIdTileImage = null,
                VocabularyNames = vocabularyNames,
            };

        }
        await reader.CloseAsync();
    }
}
