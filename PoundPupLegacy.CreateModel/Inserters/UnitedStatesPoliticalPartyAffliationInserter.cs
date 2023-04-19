namespace PoundPupLegacy.CreateModel.Inserters;

using Request = UnitedStatesPoliticalPartyAffliation;

internal sealed class UnitedStatesPoliticalPartyAffliationInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NullableIntegerDatabaseParameter UnitedStatsPoliticalPartyId = new() { Name = "united_states_political_party_id" };

    public override string TableName => "united_states_political_party_affiliation";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(UnitedStatsPoliticalPartyId, request.UnitedStatesPoliticalPartyId),
        };
    }
}
