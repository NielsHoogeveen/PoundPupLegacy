namespace PoundPupLegacy.CreateModel.Inserters;

using Request = CongressionalTermPoliticalPartyAffiliation;
internal sealed class CongressionalTermPoliticalPartyAffiliationInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NullCheckingIntegerDatabaseParameter CongressionalTermId = new() { Name = "congressional_term_id" };
    internal static NonNullableIntegerDatabaseParameter UnitedStatesPoliticalPartyAffiliationId = new() { Name = "united_states_political_party_affiliation_id" };
    internal static NonNullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };

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
