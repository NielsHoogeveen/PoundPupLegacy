namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class UnitedStatesPoliticalPartyAffliationInserterFactory : BasicDatabaseInserterFactory<UnitedStatesPoliticalPartyAffliation, UnitedStatesPoliticalPartyAffliationInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NullableIntegerDatabaseParameter UnitedStatsPoliticalPartyId = new() { Name = "united_states_political_party_id" };

    public override string TableName => "united_states_political_party_affiliation";
}
internal sealed class UnitedStatesPoliticalPartyAffliationInserter : BasicDatabaseInserter<UnitedStatesPoliticalPartyAffliation>
{
    public UnitedStatesPoliticalPartyAffliationInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(UnitedStatesPoliticalPartyAffliation item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(UnitedStatesPoliticalPartyAffliationInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(UnitedStatesPoliticalPartyAffliationInserterFactory.UnitedStatsPoliticalPartyId, item.UnitedStatesPoliticalPartyId),
        };
    }
}
