using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class CommentMigrator : PPLMigrator
{
    public CommentMigrator(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {
    }

    protected override string Name => "comments";

    protected override async Task MigrateImpl()
    {
        await CommentCreator.CreateAsync(ReadComments(), _postgresConnection);

    }
    private static int GetUid(int uid)
    {
        return uid switch
        {
            965 => 954,
            1233 => 1196,
            1655 => 1531,
            1745 => 1776,
            6197 => 6196,
            _ => uid
        };
    }
    private async IAsyncEnumerable<Comment> ReadComments()
    {

        var sql = $"""
            SELECT 
                distinct
                c.cid id,
                c.nid node_id,
                c.pid comment_id_parent,
                c.uid publisher_id,
                c.`status` node_status_id,
                FROM_UNIXTIME(c.`timestamp`) created_date_time, 
                c.hostname ip_address,
                c.subject title,
                c.`comment` `text`
            FROM comments c
            LEFT JOIN comments c2 ON c2.pid = c.cid
            JOIN node n ON n.nid = c.nid AND n.`type` NOT IN (
                'poll', 
                'book_page', 
                'message', 
                'video', 
                'map', 
                'amazon', 
                'image', 
                'amazon_node', 
                'quotations', 
                'event', 
                'usernode',
                'panel',
                'viewnode',
                'website'
            ) AND n.uid <> 0
            AND NOT (c.uid = 0 AND c.`status` = 1 AND c2.cid IS null)
            ORDER BY c.cid
            """;
        using var readCommand = MysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var discussion = new Comment
            {
                Id = reader.GetInt32("id"),
                NodeId = await _nodeIdReader.ReadAsync(Constants.PPL, reader.GetInt32("node_id")),
                CommentIdParent = reader.GetInt32("comment_id_parent") == 0 ? null : reader.GetInt32("comment_id_parent"),
                PublisherId = GetUid(reader.GetInt32("publisher_id")),
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
