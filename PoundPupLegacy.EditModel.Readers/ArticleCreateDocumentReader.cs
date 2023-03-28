using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Readers;

public class ArticleCreateDocumentReader : SimpleTextNodeCreateDocumentReader<Article>, ISingleItemDatabaseReader<ArticleCreateDocumentReader, NodeEditDocumentReader.NodeCreateDocumentRequest, Article>
{
    protected ArticleCreateDocumentReader(NpgsqlCommand command) : base(command, Constants.ARTICLE)
    {
    }

    public static async Task<ArticleCreateDocumentReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateCommand(connection, SQL);
        return new ArticleCreateDocumentReader(command);
    }

}


