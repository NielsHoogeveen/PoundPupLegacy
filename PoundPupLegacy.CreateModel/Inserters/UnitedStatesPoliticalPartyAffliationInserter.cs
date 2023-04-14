namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = UnitedStatesPoliticalPartyAffliationInserterFactory;
using Request = UnitedStatesPoliticalPartyAffliation;
using Inserter = UnitedStatesPoliticalPartyAffliationInserter;

internal sealed class UnitedStatesPoliticalPartyAffliationInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NullableIntegerDatabaseParameter UnitedStatsPoliticalPartyId = new() { Name = "united_states_political_party_id" };

    public override string TableName => "united_states_political_party_affiliation";
}
internal sealed class UnitedStatesPoliticalPartyAffliationInserter : IdentifiableDatabaseInserter<Request>
{
    public UnitedStatesPoliticalPartyAffliationInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.UnitedStatsPoliticalPartyId, request.UnitedStatesPoliticalPartyId),
        };
    }
}
