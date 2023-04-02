using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.EditModel.Readers;

public class ArticleUpdateDocumentReaderFactory : SimpleTextNodeUpdateDocumentReaderFactory<ArticleUpdateDocumentReader>
{
    public override async Task<ArticleUpdateDocumentReader> CreateAsync(IDbConnection connection)
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


