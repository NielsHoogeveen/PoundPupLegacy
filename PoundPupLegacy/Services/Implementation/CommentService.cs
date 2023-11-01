using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Services.Implementation;

public class CommentService(
    NpgsqlDataSource dataSource,
    ILogger<CommentService> logger, 
    ISiteDataService siteDataService
) : DatabaseService(dataSource, logger), ICommentService
{
    public async Task<bool> CanCreateComment(int userId)
    {
        var user = await siteDataService.GetUser(userId);
        if(user is null || user.Id == 0) {
            return false;
        }
        return user.NamedActions.Any(x => x.Name == "create_comment");
    }

    public async Task<bool> CanEditComment(int userId)
    {
        var user = await siteDataService.GetUser(userId);
        if (user is null || user.Id == 0) {
            return false;
        }
        return user.NamedActions.Any(x => x.Name == "edit_comment");
    }

    public async Task<bool> CanEditOwnComment(int userId)
    {
        var user = await siteDataService.GetUser(userId);
        if (user is null || user.Id == 0) {
            return false;
        }
        return user.NamedActions.Any(x => x.Name == "edit_own_comment");
    }

    public async Task<int> Save(Comment.ToCreate comment)
    {
        return await WithConnection(async connection => {
            var command = connection.CreateCommand();
            command.CommandText = """
                INSERT INTO comment (
                    node_id, 
                    comment_id_parent, 
                    text, 
                    publisher_id,
                    node_status_id,
                    created_date_time,
                    title
                ) 
                VALUES (
                    @node_id, 
                    @comment_id_parent, 
                    @text, 
                    @publisher_id,
                    @node_status_id,
                    @created_date_time,
                    @title
                );
                select lastval();
                """;
            command.Parameters.Add("node_id", NpgsqlTypes.NpgsqlDbType.Integer);
            command.Parameters.Add("comment_id_parent", NpgsqlTypes.NpgsqlDbType.Integer);
            command.Parameters.Add("text", NpgsqlTypes.NpgsqlDbType.Text);
            command.Parameters.Add("publisher_id", NpgsqlTypes.NpgsqlDbType.Integer);
            command.Parameters.Add("node_status_id", NpgsqlTypes.NpgsqlDbType.Integer);
            command.Parameters.Add("created_date_time", NpgsqlTypes.NpgsqlDbType.Timestamp);
            command.Parameters.Add("title", NpgsqlTypes.NpgsqlDbType.Varchar);
            await command.PrepareAsync();
            command.Parameters["node_id"].Value = comment.NodeId;
            if (comment.CommentIdParent.HasValue) {
                command.Parameters["comment_id_parent"].Value = comment.CommentIdParent;
            }
            else {
                command.Parameters["comment_id_parent"].Value = DBNull.Value;
            }
            command.Parameters["text"].Value = comment.Text;
            command.Parameters["publisher_id"].Value = comment.PublisherId;
            command.Parameters["node_status_id"].Value = comment.NodeStatusId;
            command.Parameters["created_date_time"].Value = comment.CreatedDataTime;
            command.Parameters["title"].Value = comment.Title;
            var res = await command.ExecuteScalarAsync();
            return Convert.ToInt32(res);
        });
    }
    public async Task Save(Comment.ToUpdate comment)
    {
        await WithConnection(async connection => {
            var command = connection.CreateCommand();
            command.CommandText = """
                UPDATE comment
                set 
                text = @text,
                node_status_id = @node_status_id,
                publisher_id = @publisher_id,
                title = @title
                WHERE id = @id
                """;
            command.Parameters.Add("id", NpgsqlTypes.NpgsqlDbType.Integer);
            command.Parameters.Add("text", NpgsqlTypes.NpgsqlDbType.Text);
            command.Parameters.Add("node_status_id", NpgsqlTypes.NpgsqlDbType.Integer);
            command.Parameters.Add("publisher_id", NpgsqlTypes.NpgsqlDbType.Integer);
            command.Parameters.Add("title", NpgsqlTypes.NpgsqlDbType.Varchar);
            await command.PrepareAsync();
            command.Parameters["id"].Value = comment.Id;
            command.Parameters["text"].Value = comment.Text;
            command.Parameters["publisher_id"].Value = comment.PublisherId;
            command.Parameters["node_status_id"].Value = comment.NodeStatusId;
            command.Parameters["title"].Value = comment.Title;
            await command.ExecuteNonQueryAsync();
            return Unit.Instance;
        });

    }

}
