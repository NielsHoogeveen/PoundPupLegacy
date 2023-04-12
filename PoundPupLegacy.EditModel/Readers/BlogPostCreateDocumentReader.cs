using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.EditModel.Readers;

public class BlogPostCreateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<BlogPostCreateDocumentReader>
{
}
public class BlogPostCreateDocumentReader : SimpleTextNodeCreateDocumentReader<BlogPost>
{
    internal BlogPostCreateDocumentReader(NpgsqlCommand command) : base(command, Constants.BLOG_POST)
    {
    }

}


