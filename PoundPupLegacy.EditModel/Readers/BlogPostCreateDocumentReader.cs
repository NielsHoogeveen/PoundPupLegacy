namespace PoundPupLegacy.EditModel.Readers;

using Reader = BlogPostCreateDocumentReader;

internal sealed class BlogPostCreateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<BlogPost, Reader>
{
}
internal sealed class BlogPostCreateDocumentReader : SimpleTextNodeCreateDocumentReader<BlogPost>
{
    public BlogPostCreateDocumentReader(NpgsqlCommand command) : base(command, Constants.BLOG_POST)
    {
    }

}


