namespace PoundPupLegacy.EditModel.Readers;

using Reader = BlogPostUpdateDocumentReader;

public sealed class BlogPostUpdateDocumentReaderFactory : SimpleTextNodeUpdateDocumentReaderFactory<Reader>
{
}

public sealed class BlogPostUpdateDocumentReader : SimpleTextNodeUpdateDocumentReader<BlogPost>
{
    public BlogPostUpdateDocumentReader(NpgsqlCommand command) : base(command, Constants.BLOG_POST)
    {
    }
}