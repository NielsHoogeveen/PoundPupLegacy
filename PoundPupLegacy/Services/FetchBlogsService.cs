using Npgsql;
using PoundPupLegacy.ViewModel;
using System.Data;

namespace PoundPupLegacy.Web.Services;

public class FetchBlogsService
{
    private NpgsqlConnection _connection;

    public FetchBlogsService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<List<BlogListEntry>> FetchBlogs()
    {
        _connection.Open();
        var sql = $"""
            select 
                json_agg(to_jsonb(b))
            from(
                select 
                    ar.name "Name",
                    ar.id "Id",
                    u.avatar "FilePathAvatar",
                    COUNT(n.id) "NumberOfEntries",
                    max(n.id) "LatestEntryId",
                    (select n2.title from node n2 where n2.id = max( n.id)) "LatestEntryTitle"
                from access_role ar
                left join "user" u on u.id = ar.id
                join node n on n.access_role_id = ar.id and n.node_status_id = 1
                join blog_post b on b.id = n.id
                group by ar.name,
                ar.id,
                u.created_date_time,
                u.avatar
                order by COUNT(n.id) desc, u.created_date_time 
            ) b
            """;

        using var readCommand = _connection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;
        await readCommand.PrepareAsync();
        await using var reader = await readCommand.ExecuteReaderAsync();
        await reader.ReadAsync();
        var blogs = reader.GetFieldValue<List<BlogListEntry>>(0);
        _connection.Close();
        return blogs!;
    }

}
