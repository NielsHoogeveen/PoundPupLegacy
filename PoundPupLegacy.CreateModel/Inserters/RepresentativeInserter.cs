namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class RepresentativeInserterFactory : SingleIdInserterFactory<Representative.RepresentativeToCreateForExistingPerson>
{
    protected override string TableName => "representative";

    protected override bool AutoGenerateIdentity => false;

}
