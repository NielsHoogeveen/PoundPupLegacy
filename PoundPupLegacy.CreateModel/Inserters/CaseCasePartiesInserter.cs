namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CaseCasePartiesInserterFactory : DatabaseInserterFactory<CaseCaseParties>
{
    internal static NonNullableIntegerDatabaseParameter CaseId = new() { Name = "case_id" };
    internal static NonNullableIntegerDatabaseParameter CasePartiesId = new() { Name = "case_parties_id" };
    internal static NonNullableIntegerDatabaseParameter CasePartyTypeId = new() { Name = "case_party_type_id" };

    public override async Task<IDatabaseInserter<CaseCaseParties>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "case_case_parties",
            new DatabaseParameter[] {
                CaseId,
                CasePartiesId,
                CasePartyTypeId
            }
        );
        return new CaseCasePartiesInserter(command);

    }

}
internal sealed class CaseCasePartiesInserter : DatabaseInserter<CaseCaseParties>
{

    internal CaseCasePartiesInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(CaseCaseParties caseCaseParties)
    {
        if (caseCaseParties.CaseParties.Id is null)
            throw new NullReferenceException();
        Set(CaseCasePartiesInserterFactory.CaseId, caseCaseParties.CaseId);
        Set(CaseCasePartiesInserterFactory.CasePartiesId, caseCaseParties.CaseParties.Id.Value);
        Set(CaseCasePartiesInserterFactory.CasePartyTypeId, caseCaseParties.CasePartyTypeId);
        
        await _command.ExecuteNonQueryAsync();
    }
}
