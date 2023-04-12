using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.EditModel.Readers;

public class BlogPostUpdateDocumentReaderFactory : SimpleTextNodeUpdateDocumentReaderFactory<BlogPostUpdateDocumentReader>
{
}

public class BlogPostUpdateDocumentReader : SimpleTextNodeUpdateDocumentReader<BlogPost>
{
    internal BlogPostUpdateDocumentReader(NpgsqlCommand command) : base(command, Constants.BLOG_POST)
    {
    }
}