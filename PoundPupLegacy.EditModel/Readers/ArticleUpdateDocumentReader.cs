using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.EditModel.Readers;

public class ArticleUpdateDocumentReaderFactory : SimpleTextNodeUpdateDocumentReaderFactory<ArticleUpdateDocumentReader>
{
}

public class ArticleUpdateDocumentReader : SimpleTextNodeUpdateDocumentReader<Article>
{
    internal ArticleUpdateDocumentReader(NpgsqlCommand command) : base(command, Constants.ARTICLE)
    {
    }
}


