namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class UnitedStatesPoliticalPartyInserterFactory : SingleIdInserterFactory<EventuallyIdentifiableUnitedStatesPoliticalParty>
{
    protected override string TableName => "united_states_political_party";

    protected override bool AutoGenerateIdentity => false;

}
