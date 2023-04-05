namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BlogPostInserterFactory : SingleIdInserterFactory<BlogPost>
{
    protected override string TableName => "blog_post";

    protected override bool AutoGenerateIdentity => false;

}
