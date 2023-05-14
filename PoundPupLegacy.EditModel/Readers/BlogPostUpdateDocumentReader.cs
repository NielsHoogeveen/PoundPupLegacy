namespace PoundPupLegacy.EditModel.Readers;

internal sealed class BlogPostUpdateDocumentReaderFactory : SimpleTextNodeUpdateDocumentReaderFactory<ExistingBlogPost>
{
    protected override int NodeTypeId => Constants.BLOG_POST;
}

