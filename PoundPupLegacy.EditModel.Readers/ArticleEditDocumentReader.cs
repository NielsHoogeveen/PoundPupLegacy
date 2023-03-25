using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Readers;

public class ArticleEditDocumentReader : SimpleTextNodeEditDocumentReader<Article>, IDatabaseReader<ArticleEditDocumentReader>
{
    protected ArticleEditDocumentReader(NpgsqlCommand command) : base(command, Constants.ARTICLE)
    {
    }

    public static async Task<ArticleEditDocumentReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateCommand(connection, SIMPLE_TEXT_NODE_DOCUMENT);
        return new ArticleEditDocumentReader(command);
    }
}


