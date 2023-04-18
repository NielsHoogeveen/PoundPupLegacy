namespace PoundPupLegacy.EditModel.Readers;

internal sealed class BlogPostCreateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<BlogPost>
{
    protected override int NodeTypeId => Constants.BLOG_POST;
}
