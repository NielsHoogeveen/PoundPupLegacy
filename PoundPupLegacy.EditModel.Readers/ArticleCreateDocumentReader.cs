using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Readers;

public class ArticleCreateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<ArticleCreateDocumentReader>
{
    public override async Task<ArticleCreateDocumentReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateCommand(connection, SQL);
        return new ArticleCreateDocumentReader(command);
    }

}
public class ArticleCreateDocumentReader : SimpleTextNodeCreateDocumentReader<Article>
{
    internal ArticleCreateDocumentReader(NpgsqlCommand command) : base(command, Constants.ARTICLE)
    {
    }
}



