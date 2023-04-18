namespace PoundPupLegacy.EditModel.Readers;

internal sealed class BlogPostUpdateDocumentReaderFactory : SimpleTextNodeUpdateDocumentReaderFactory<BlogPost>
{
    protected override int NodeTypeId => Constants.BLOG_POST;
}

