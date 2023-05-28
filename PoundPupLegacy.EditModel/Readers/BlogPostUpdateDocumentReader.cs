namespace PoundPupLegacy.EditModel.Readers;

internal sealed class BlogPostUpdateDocumentReaderFactory : SimpleTextNodeUpdateDocumentReaderFactory<BlogPost.ExistingBlogPost>
{
    protected override int NodeTypeId => Constants.BLOG_POST;
}

