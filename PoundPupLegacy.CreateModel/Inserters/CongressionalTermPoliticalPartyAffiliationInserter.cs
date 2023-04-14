namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = CongressionalTermPoliticalPartyAffiliationInserterFactory;
using Request = CongressionalTermPoliticalPartyAffiliation;
using Inserter = CongressionalTermPoliticalPartyAffiliationInserter;
internal sealed class CongressionalTermPoliticalPartyAffiliationInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NullCheckingIntegerDatabaseParameter CongressionalTermId = new() { Name = "congressional_term_id" };
    internal static NonNullableIntegerDatabaseParameter UnitedStatesPoliticalPartyAffiliationId = new() { Name = "united_states_political_party_affiliation_id" };
    internal static NonNullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };

    public override string TableName => "congressional_term_political_party_affiliation";
}
internal sealed class CongressionalTermPoliticalPartyAffiliationInserter : IdentifiableDatabaseInserter<Request>
{

    public CongressionalTermPoliticalPartyAffiliationInserter(NpgsqlCommand command) : base(command)
    {
    }
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.CongressionalTermId, request.CongressionalTermId),
            ParameterValue.Create(Factory.UnitedStatesPoliticalPartyAffiliationId, request.PoliticalPartyAffiliationId),
            ParameterValue.Create(Factory.DateRange, request.DateTimeRange),
        };
    }
}
