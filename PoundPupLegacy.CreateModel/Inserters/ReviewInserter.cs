namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class ReviewInserterFactory : SingleIdInserterFactory<EventuallyIdentifiableReview>
{
    protected override string TableName => "review";

    protected override bool AutoGenerateIdentity => false;

}
