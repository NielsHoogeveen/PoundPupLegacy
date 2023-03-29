using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Readers;

public class BlogPostCreateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<BlogPostCreateDocumentReader>
{
    public override async Task<BlogPostCreateDocumentReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateCommand(connection, SQL);
        return new BlogPostCreateDocumentReader(command);
    }

}
public class BlogPostCreateDocumentReader : SimpleTextNodeCreateDocumentReader<BlogPost>
{
    internal BlogPostCreateDocumentReader(NpgsqlCommand command) : base(command, Constants.BLOG_POST)
    {
    }

}


