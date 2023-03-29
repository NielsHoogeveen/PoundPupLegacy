using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Readers;

public class ArticleUpdateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<ArticleUpdateDocumentReader>
{
    public override async Task<ArticleUpdateDocumentReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateCommand(connection, SQL);
        return new ArticleUpdateDocumentReader(command);
    }

}

public class ArticleUpdateDocumentReader : SimpleTextNodeUpdateDocumentReader<Article>
{
    internal ArticleUpdateDocumentReader(NpgsqlCommand command) : base(command, Constants.ARTICLE)
    {
    }
}


