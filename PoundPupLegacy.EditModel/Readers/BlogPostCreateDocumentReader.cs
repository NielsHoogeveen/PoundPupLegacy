namespace PoundPupLegacy.EditModel.Readers;

using Reader = BlogPostCreateDocumentReader;

public sealed class BlogPostCreateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<Reader>
{
}
public sealed class BlogPostCreateDocumentReader : SimpleTextNodeCreateDocumentReader<BlogPost>
{
    internal BlogPostCreateDocumentReader(NpgsqlCommand command) : base(command, Constants.BLOG_POST)
    {
    }

}


