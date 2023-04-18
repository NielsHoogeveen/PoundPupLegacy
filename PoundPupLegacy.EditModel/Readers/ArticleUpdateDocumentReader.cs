namespace PoundPupLegacy.EditModel.Readers;

internal sealed class ArticleUpdateDocumentReaderFactory : SimpleTextNodeUpdateDocumentReaderFactory<Article>
{
    protected override int NodeTypeId => Constants.ARTICLE;
}

