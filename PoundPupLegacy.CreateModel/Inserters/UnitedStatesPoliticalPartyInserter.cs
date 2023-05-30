namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class UnitedStatesPoliticalPartyInserterFactory : SingleIdInserterFactory<UnitedStatesPoliticalParty.ToCreate>
{
    protected override string TableName => "united_states_political_party";

    protected override bool AutoGenerateIdentity => false;

}
