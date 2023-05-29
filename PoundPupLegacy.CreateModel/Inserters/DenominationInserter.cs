namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class DenominationInserterFactory : SingleIdInserterFactory<Denomination.DenominationToCreate>
{
    protected override string TableName => "denomination";

    protected override bool AutoGenerateIdentity => false;

}
