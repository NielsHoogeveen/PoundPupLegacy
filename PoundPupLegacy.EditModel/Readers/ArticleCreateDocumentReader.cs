using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.EditModel.Readers;

public class ArticleCreateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<ArticleCreateDocumentReader>
{

}
public class ArticleCreateDocumentReader : SimpleTextNodeCreateDocumentReader<Article>
{
    internal ArticleCreateDocumentReader(NpgsqlCommand command) : base(command, Constants.ARTICLE)
    {
    }
}



