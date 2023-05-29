namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class DiscussionInserterFactory : SingleIdInserterFactory<Discussion.DiscussionToCreate>
{
    protected override string TableName => "discussion";

    protected override bool AutoGenerateIdentity => false;

}
