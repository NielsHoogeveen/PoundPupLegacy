using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class BlogPostInserterFactory : SingleIdInserterFactory<BlogPost.ToCreate>
{
    protected override string TableName => "blog_post";

    protected override bool AutoGenerateIdentity => false;

}
