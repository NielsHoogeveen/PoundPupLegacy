namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PartyInserterFactory : SingleIdInserterFactory<EventuallyIdentifiableParty>
{
    protected override string TableName => "party";

    protected override bool AutoGenerateIdentity => false;

}
