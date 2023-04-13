namespace PoundPupLegacy.EditModel.Readers;

using Reader = ArticleCreateDocumentReader;

internal sealed class ArticleCreateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<Article, Reader>
{
}
internal sealed class ArticleCreateDocumentReader : SimpleTextNodeCreateDocumentReader<Article>
{
    public ArticleCreateDocumentReader(NpgsqlCommand command) : base(command, Constants.ARTICLE)
    {
    }
}



