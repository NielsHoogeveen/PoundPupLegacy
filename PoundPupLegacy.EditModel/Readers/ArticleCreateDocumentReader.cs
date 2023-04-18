namespace PoundPupLegacy.EditModel.Readers;

internal sealed class ArticleCreateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<Article>
{
    protected override int NodeTypeId => Constants.ARTICLE;
}
