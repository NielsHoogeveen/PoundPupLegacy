namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class RepresentativeInserterFactory : SingleIdInserterFactory<Representative>
{
    protected override string TableName => "representative";

    protected override bool AutoGenerateIdentity => false;

}
