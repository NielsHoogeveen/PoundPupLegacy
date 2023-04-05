namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CaseCasePartiesInserterFactory : DatabaseInserterFactory<CaseCaseParties>
{
    public override async Task<IDatabaseInserter<CaseCaseParties>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "case_case_parties",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = CaseCasePartiesInserter.CASE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = CaseCasePartiesInserter.CASE_PARTIES_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = CaseCasePartiesInserter.CASE_PARTY_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new CaseCasePartiesInserter(command);

    }

}
internal sealed class CaseCasePartiesInserter : DatabaseInserter<CaseCaseParties>
{

    internal const string CASE_ID = "case_id";
    internal const string CASE_PARTIES_ID = "case_parties_id";
    internal const string CASE_PARTY_TYPE_ID = "case_party_type_id";

    internal CaseCasePartiesInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(CaseCaseParties caseCaseParties)
    {
        WriteValue(caseCaseParties.CaseId, CASE_ID);
        WriteValue(caseCaseParties.CaseParties.Id, CASE_PARTIES_ID);
        WriteValue(caseCaseParties.CasePartyTypeId, CASE_PARTY_TYPE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
