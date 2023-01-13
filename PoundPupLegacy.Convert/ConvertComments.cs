using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Db.Readers;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{
    private static async Task MigrateComments(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        await using var nodeIdReader = await NodeIdByUrlIdReader.CreateAsync(connection);
        await using var tx = await connection.BeginTransactionAsync();
        try
        {
            await CommentCreator.CreateAsync(ReadComments(mysqlconnection, nodeIdReader), connection);
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }

    }
    private static async IAsyncEnumerable<Comment> ReadComments(MySqlConnection mysqlconnection, NodeIdByUrlIdReader nodeIdReader)
    {

        var sql = $"""
                SELECT 
                    c.cid id,
                    c.nid node_id,
                    c.pid comment_id_parent,
                    c.uid access_role_id,
                    c.`status` node_status_id,
                    FROM_UNIXTIME(c.`timestamp`) created_date_time, 
                    c.hostname ip_address,
                    c.subject title,
                    c.`comment` `text`
                FROM comments c
                JOIN node n ON n.nid = c.nid AND n.`type` NOT IN (
                    'poll', 
                    'book_page', 
                    'message', 
                    'video', 
                    'map', 
                    'amazon', 
                    'image', 
                    'amazon_node', 
                    'adopt_affiliation', 
                    'adopt_positions', 
                    'award_poll', 
                    'quotations', 
                    'event', 
                    'adopt_country_link', 
                    'usernode',
                    'panel',
                    'viewnode',
                    'website'
                ) AND n.uid <> 0
                ORDER BY c.cid
                """;
        using var readCommand = mysqlconnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var discussion = new Comment
            {
                Id = reader.GetInt32("id"),
                NodeId = await nodeIdReader.ReadAsync(PPL, reader.GetInt32("node_id")),
                CommentIdParent = reader.GetInt32("comment_id_parent") == 0 ? null: reader.GetInt32("comment_id_parent"),
                AccessRoleId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                NodeStatusId = reader.GetInt32("node_status_id"),
                IPAddress = reader.GetString("ip_address"),
                Title = reader.GetString("title"),
                Text = reader.GetString("text"),
            };
            yield return discussion;

        }
        await reader.CloseAsync();
    }

}
