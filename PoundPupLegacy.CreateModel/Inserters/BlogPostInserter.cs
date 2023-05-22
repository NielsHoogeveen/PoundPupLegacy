namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BlogPostInserterFactory : SingleIdInserterFactory<NewBlogPost>
{
    protected override string TableName => "blog_post";

    protected override bool AutoGenerateIdentity => false;

}
