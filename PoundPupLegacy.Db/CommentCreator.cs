﻿using System.Runtime.InteropServices;

namespace PoundPupLegacy.Db;

public class CommentCreator : IEntityCreator<Comment>
{
    public static async Task CreateAsync(IAsyncEnumerable<Comment> comments, NpgsqlConnection connection)
    {

        await using var commentWriter = await CommentWriter.CreateAsync(connection);

        await foreach (var comment in comments)
        {
            await commentWriter.WriteAsync(comment);
        }
    }
}