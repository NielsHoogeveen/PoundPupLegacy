namespace PoundPupLegacy.CreateModel.Inserters;

using Request = NewCongressionalTermPoliticalPartyAffiliation;
internal sealed class CongressionalTermPoliticalPartyAffiliationInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NullCheckingIntegerDatabaseParameter CongressionalTermId = new() { Name = "congressional_term_id" };
    private static readonly NonNullableIntegerDatabaseParameter UnitedStatesPoliticalPartyAffiliationId = new() { Name = "united_states_political_party_affiliation_id" };
    private static readonly NonNullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };

    public override string TableName => "congressional_term_political_party_affiliation";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(CongressionalTermId, request.CongressionalTermId),
            ParameterValue.Create(UnitedStatesPoliticalPartyAffiliationId, request.PoliticalPartyAffiliationId),
            ParameterValue.Create(DateRange, request.DateTimeRange),
        };
    }
}
