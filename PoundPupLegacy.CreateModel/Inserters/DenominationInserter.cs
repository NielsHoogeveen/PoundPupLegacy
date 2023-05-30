namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class DenominationInserterFactory : SingleIdInserterFactory<Denomination.ToCreate>
{
    protected override string TableName => "denomination";

    protected override bool AutoGenerateIdentity => false;

}
