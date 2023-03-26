using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Readers;

public class BlogPostEditDocumentReader : SimpleTextNodeEditDocumentReader<BlogPost>, ISingleItemDatabaseReader<BlogPostEditDocumentReader, NodeEditDocumentReader.NodeEditDocumentRequest, BlogPost>
{
    protected BlogPostEditDocumentReader(NpgsqlCommand command) : base(command, Constants.BLOG_POST)
    {
    }

    public static async Task<BlogPostEditDocumentReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateCommand(connection, SIMPLE_TEXT_NODE_DOCUMENT);
        return new BlogPostEditDocumentReader(command);
    }
}


