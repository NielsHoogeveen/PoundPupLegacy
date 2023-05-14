namespace PoundPupLegacy.EditModel.Readers;

internal sealed class BlogPostCreateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<NewBlogPost>
{
    protected override int NodeTypeId => Constants.BLOG_POST;
}
