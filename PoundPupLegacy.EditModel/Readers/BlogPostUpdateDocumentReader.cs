namespace PoundPupLegacy.EditModel.Readers;

internal sealed class BlogPostUpdateDocumentReaderFactory : SimpleTextNodeUpdateDocumentReaderFactory<BlogPost.ToUpdate>
{
    protected override int NodeTypeId => Constants.BLOG_POST;
}

