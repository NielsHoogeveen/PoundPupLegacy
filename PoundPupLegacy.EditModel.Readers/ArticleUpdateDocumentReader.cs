using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Readers;

public class ArticleUpdateDocumentReader : SimpleTextNodeUpdateDocumentReader<Article>, ISingleItemDatabaseReader<ArticleUpdateDocumentReader, NodeEditDocumentReader.NodeUpdateDocumentRequest, Article>
{
    protected ArticleUpdateDocumentReader(NpgsqlCommand command) : base(command, Constants.ARTICLE)
    {
    }

    public static async Task<ArticleUpdateDocumentReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateCommand(connection, SQL);
        return new ArticleUpdateDocumentReader(command);
    }

}


