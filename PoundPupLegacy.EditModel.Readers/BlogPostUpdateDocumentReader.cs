using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Readers;

public class BlogPostUpdateDocumentReader : SimpleTextNodeUpdateDocumentReader<BlogPost>, ISingleItemDatabaseReader<BlogPostUpdateDocumentReader, NodeEditDocumentReader.NodeUpdateDocumentRequest, BlogPost>
{
    protected BlogPostUpdateDocumentReader(NpgsqlCommand command) : base(command, Constants.BLOG_POST)
    {
    }

    public static async Task<BlogPostUpdateDocumentReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateCommand(connection, SQL);
        return new BlogPostUpdateDocumentReader(command);
    }
}


