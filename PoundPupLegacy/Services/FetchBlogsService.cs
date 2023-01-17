﻿using Npgsql;
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
                    p.name "Name",
                    p.id "Id",
                    u.avatar "FilePathAvatar",
                    COUNT(n.id) "NumberOfEntries",
                    max(tn.url_id) "LatestEntryId",
                    (
                        select 
                            n2.title 
                        from node n2 
                        JOIN tenant_node tn2 on tn2.node_id = n2.id
                        where tn2.url_id = max( tn.url_id)
                    ) "LatestEntryTitle"
                from principal p
                left join "user" u on u.id = p.id
                join node n on n.publisher_id = p.id 
                join tenant_node tn on tn.node_id = n.id and tn.publication_status_id = 1
                join blog_post b on b.id = n.id
                group by p.name,
                p.id,
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