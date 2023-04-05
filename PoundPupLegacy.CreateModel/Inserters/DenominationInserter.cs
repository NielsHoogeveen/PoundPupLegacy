namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class DenominationInserterFactory : SingleIdInserterFactory<Denomination>
{
    protected override string TableName => "denomination";

    protected override bool AutoGenerateIdentity => false;

}
