using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Readers;

public class BlogPostCreateDocumentReader : SimpleTextNodeCreateDocumentReader<BlogPost>, ISingleItemDatabaseReader<BlogPostCreateDocumentReader, NodeEditDocumentReader.NodeCreateDocumentRequest, BlogPost>
{
    protected BlogPostCreateDocumentReader(NpgsqlCommand command) : base(command, Constants.BLOG_POST)
    {
    }

    public static async Task<BlogPostCreateDocumentReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateCommand(connection, SQL);
        return new BlogPostCreateDocumentReader(command);
    }
}


