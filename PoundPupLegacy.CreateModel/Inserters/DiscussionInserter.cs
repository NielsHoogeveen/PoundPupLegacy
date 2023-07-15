namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class DiscussionInserterFactory : SingleIdInserterFactory<Discussion.ToCreate>
{
    protected override string TableName => "discussion";

    protected override bool AutoGenerateIdentity => false;

}
