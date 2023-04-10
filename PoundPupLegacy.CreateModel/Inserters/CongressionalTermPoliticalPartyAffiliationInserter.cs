namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CongressionalTermPoliticalPartyAffiliationInserterFactory : DatabaseInserterFactory<CongressionalTermPoliticalPartyAffiliation>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter CongressionalTermId = new() { Name = "congressional_term_id" };
    internal static NonNullableIntegerDatabaseParameter UnitedStatesPoliticalPartyAffiliationId = new() { Name = "united_states_political_party_affiliation_id" };
    internal static NonNullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };

    public override async Task<IDatabaseInserter<CongressionalTermPoliticalPartyAffiliation>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "congressional_term_political_party_affiliation",
            new DatabaseParameter[] {
                Id,
                CongressionalTermId,
                UnitedStatesPoliticalPartyAffiliationId,
                DateRange
            }
        );
        return new CongressionalTermPoliticalPartyAffiliationInserter(command);

    }

}
internal sealed class CongressionalTermPoliticalPartyAffiliationInserter : DatabaseInserter<CongressionalTermPoliticalPartyAffiliation>
{

    internal CongressionalTermPoliticalPartyAffiliationInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(CongressionalTermPoliticalPartyAffiliation congressionalTermPoliticalPartyAffiliation)
    {
        if (congressionalTermPoliticalPartyAffiliation.Id is null)
            throw new NullReferenceException();
        if(congressionalTermPoliticalPartyAffiliation.CongressionalTermId is null)
            throw new NullReferenceException();

        Set(CongressionalTermPoliticalPartyAffiliationInserterFactory.Id, congressionalTermPoliticalPartyAffiliation.Id.Value);
        Set(CongressionalTermPoliticalPartyAffiliationInserterFactory.CongressionalTermId, congressionalTermPoliticalPartyAffiliation.CongressionalTermId.Value);
        Set(CongressionalTermPoliticalPartyAffiliationInserterFactory.UnitedStatesPoliticalPartyAffiliationId, congressionalTermPoliticalPartyAffiliation.PoliticalPartyAffiliationId);
        Set(CongressionalTermPoliticalPartyAffiliationInserterFactory.DateRange, congressionalTermPoliticalPartyAffiliation.DateTimeRange);
        await _command.ExecuteNonQueryAsync();
    }
}
