namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CongressionalTermPoliticalPartyAffiliationInserterFactory : DatabaseInserterFactory<CongressionalTermPoliticalPartyAffiliation, CongressionalTermPoliticalPartyAffiliationInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter CongressionalTermId = new() { Name = "congressional_term_id" };
    internal static NonNullableIntegerDatabaseParameter UnitedStatesPoliticalPartyAffiliationId = new() { Name = "united_states_political_party_affiliation_id" };
    internal static NonNullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };

    public override string TableName => "congressional_term_political_party_affiliation";
}
internal sealed class CongressionalTermPoliticalPartyAffiliationInserter : DatabaseInserter<CongressionalTermPoliticalPartyAffiliation>
{

    public CongressionalTermPoliticalPartyAffiliationInserter(NpgsqlCommand command) : base(command)
    {
    }
    public override IEnumerable<ParameterValue> GetParameterValues(CongressionalTermPoliticalPartyAffiliation item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        if (item.CongressionalTermId is null)
            throw new NullReferenceException();

        return new ParameterValue[] {
            ParameterValue.Create(CongressionalTermPoliticalPartyAffiliationInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(CongressionalTermPoliticalPartyAffiliationInserterFactory.CongressionalTermId, item.CongressionalTermId.Value),
            ParameterValue.Create(CongressionalTermPoliticalPartyAffiliationInserterFactory.UnitedStatesPoliticalPartyAffiliationId, item.PoliticalPartyAffiliationId),
            ParameterValue.Create(CongressionalTermPoliticalPartyAffiliationInserterFactory.DateRange, item.DateTimeRange),
        };
    }
}
