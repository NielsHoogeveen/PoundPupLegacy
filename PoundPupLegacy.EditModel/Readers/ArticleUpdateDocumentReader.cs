namespace PoundPupLegacy.EditModel.Readers;

using Reader = ArticleUpdateDocumentReader;
public sealed class ArticleUpdateDocumentReaderFactory : SimpleTextNodeUpdateDocumentReaderFactory<Reader>
{
}

public sealed class ArticleUpdateDocumentReader : SimpleTextNodeUpdateDocumentReader<Article>
{
    internal ArticleUpdateDocumentReader(NpgsqlCommand command) : base(command, Constants.ARTICLE)
    {
    }
}


