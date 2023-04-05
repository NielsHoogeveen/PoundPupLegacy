namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PartyInserterFactory : SingleIdInserterFactory<Party>
{
    protected override string TableName => "party";

    protected override bool AutoGenerateIdentity => false;

}
