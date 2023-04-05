namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class ArticleInserterFactory : SingleIdInserterFactory<Article>
{
    protected override string TableName => "article";

    protected override bool AutoGenerateIdentity => false;

}
