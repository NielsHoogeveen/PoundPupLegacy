namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CasePartiesPersonInserterFactory : DatabaseInserterFactory<CasePartiesPerson>
{
    internal static NonNullableIntegerDatabaseParameter CasePartiesId = new() { Name = "case_parties_id" };
    internal static NonNullableIntegerDatabaseParameter PersonId = new() { Name = "person_id" };

    public override async Task<IDatabaseInserter<CasePartiesPerson>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "case_parties_person",
            new DatabaseParameter[] {
                CasePartiesId,
                PersonId
            }
        );
        return new CasePartiesPersonInserter(command);

    }

}
internal sealed class CasePartiesPersonInserter : DatabaseInserter<CasePartiesPerson>
{
    internal CasePartiesPersonInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(CasePartiesPerson casePartiesPerson)
    {
        Set(CasePartiesPersonInserterFactory.CasePartiesId, casePartiesPerson.CasePartiesId);
        Set(CasePartiesPersonInserterFactory.PersonId, casePartiesPerson.PersonId);
        await _command.ExecuteNonQueryAsync();
    }
}
