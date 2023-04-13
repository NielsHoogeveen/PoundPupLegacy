namespace PoundPupLegacy.EditModel.Readers;

using Reader = ArticleUpdateDocumentReader;
internal sealed class ArticleUpdateDocumentReaderFactory : SimpleTextNodeUpdateDocumentReaderFactory<Article, Reader>
{
}

internal sealed class ArticleUpdateDocumentReader : SimpleTextNodeUpdateDocumentReader<Article>
{
    public ArticleUpdateDocumentReader(NpgsqlCommand command) : base(command, Constants.ARTICLE)
    {
    }
}


