namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class ReviewInserterFactory : SingleIdInserterFactory<Review.ToCreate>
{
    protected override string TableName => "review";

    protected override bool AutoGenerateIdentity => false;

}
