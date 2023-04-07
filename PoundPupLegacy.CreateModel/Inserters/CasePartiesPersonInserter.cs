namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CasePartiesPersonInserterFactory : DatabaseInserterFactory<CasePartiesPerson>
{
    public override async Task<IDatabaseInserter<CasePartiesPerson>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "case_parties_person",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = CasePartiesPersonInserter.CASE_PARTIES_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = CasePartiesPersonInserter.PERSON_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new CasePartiesPersonInserter(command);

    }

}
internal sealed class CasePartiesPersonInserter : DatabaseInserter<CasePartiesPerson>
{

    internal const string CASE_PARTIES_ID = "case_parties_id";
    internal const string PERSON_ID = "person_id";

    internal CasePartiesPersonInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(CasePartiesPerson casePartiesPerson)
    {
        SetParameter(casePartiesPerson.CasePartiesId, CASE_PARTIES_ID);
        SetParameter(casePartiesPerson.PersonId, PERSON_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
