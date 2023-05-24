namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class DiscussionInserterFactory : SingleIdInserterFactory<EventuallyIdentifiableDiscussion>
{
    protected override string TableName => "discussion";

    protected override bool AutoGenerateIdentity => false;

}
