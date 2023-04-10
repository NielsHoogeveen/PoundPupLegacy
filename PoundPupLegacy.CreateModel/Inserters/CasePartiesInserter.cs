namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CasePartiesInserterFactory : DatabaseInserterFactory<CaseParties>
{
    internal static NullableStringDatabaseParameter Organizations = new() { Name = "organizations" };
    internal static NullableStringDatabaseParameter Persons = new() { Name = "persons" };

    public override async Task<IDatabaseInserter<CaseParties>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateAutoGenerateIdentityInsertStatementAsync(
            postgresConnection,
            "case_parties",
            new DatabaseParameter[] {
                Organizations,
                Persons
            }
        );
        return new CasePartiesInserter(command);

    }

}
internal sealed class CasePartiesInserter : DatabaseInserter<CaseParties>
{


    internal CasePartiesInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(CaseParties caseParties)
    {
        if (caseParties.Id.HasValue) {
            throw new Exception($"case parties id should be null upon creation");
        }
        Set(CasePartiesInserterFactory.Organizations, caseParties.Organizations);
        Set(CasePartiesInserterFactory.Persons, caseParties.Persons);
        caseParties.Id = await _command.ExecuteScalarAsync() switch {
            long i => (int)i,
            _ => throw new Exception("Insert of case parties does not return an id.")
        };
    }
}
