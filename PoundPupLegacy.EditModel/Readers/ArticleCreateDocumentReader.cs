namespace PoundPupLegacy.EditModel.Readers;

using Reader = ArticleCreateDocumentReader;

public sealed class ArticleCreateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<Reader>
{

}
public sealed class ArticleCreateDocumentReader : SimpleTextNodeCreateDocumentReader<Article>
{
    internal ArticleCreateDocumentReader(NpgsqlCommand command) : base(command, Constants.ARTICLE)
    {
    }
}



