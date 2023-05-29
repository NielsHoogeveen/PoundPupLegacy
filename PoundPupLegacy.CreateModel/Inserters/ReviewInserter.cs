namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class ReviewInserterFactory : SingleIdInserterFactory<Review.ReviewToCreate>
{
    protected override string TableName => "review";

    protected override bool AutoGenerateIdentity => false;

}
