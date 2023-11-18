namespace PoundPupLegacy.DomainModel.Inserters;

using Request = UnitedStatesCounty.ToCreate;

internal sealed class UnitedStatesCountyInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter StateId = new() { Name = "united_states_state_id" };
    private static readonly NonNullableIntegerDatabaseParameter Fips = new() { Name = "fips" };

    public override string TableName => "united_states_county";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(StateId, request.UnitedStatesStateId),
            ParameterValue.Create(Fips, request.Fips)
        };
    }
}
