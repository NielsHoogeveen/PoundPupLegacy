using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Readers;

public class BlogPostUpdateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<BlogPostUpdateDocumentReader>
{
    public override async Task<BlogPostUpdateDocumentReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateCommand(connection, SQL);
        return new BlogPostUpdateDocumentReader(command);
    }

}

public class BlogPostUpdateDocumentReader : SimpleTextNodeUpdateDocumentReader<BlogPost>
{
    internal BlogPostUpdateDocumentReader(NpgsqlCommand command) : base(command, Constants.ARTICLE)
    {
    }
}