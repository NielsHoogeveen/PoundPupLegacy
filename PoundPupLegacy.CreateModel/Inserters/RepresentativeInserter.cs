namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class RepresentativeInserterFactory : SingleIdInserterFactory<NewRepresentativeAsNewPerson>
{
    protected override string TableName => "representative";

    protected override bool AutoGenerateIdentity => false;

}
