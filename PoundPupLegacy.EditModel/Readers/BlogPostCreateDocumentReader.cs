namespace PoundPupLegacy.EditModel.Readers;

internal sealed class BlogPostCreateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<BlogPost.ToCreate>
{
    protected override int NodeTypeId => Constants.BLOG_POST;
}
