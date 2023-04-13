namespace PoundPupLegacy.EditModel.Readers;

using Reader = BlogPostUpdateDocumentReader;

internal sealed class BlogPostUpdateDocumentReaderFactory : SimpleTextNodeUpdateDocumentReaderFactory<BlogPost, Reader>
{
}

internal sealed class BlogPostUpdateDocumentReader : SimpleTextNodeUpdateDocumentReader<BlogPost>
{
    public BlogPostUpdateDocumentReader(NpgsqlCommand command) : base(command, Constants.BLOG_POST)
    {
    }
}