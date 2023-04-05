namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CasePartiesInserterFactory : DatabaseInserterFactory<CaseParties>
{
    public override async Task<IDatabaseInserter<CaseParties>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateIdentityInsertStatementAsync(
            postgresConnection,
            "case_parties",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = CasePartiesInserter.ORGANIZATIONS,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = CasePartiesInserter.PERSONS,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new CasePartiesInserter(command);

    }

}
internal sealed class CasePartiesInserter : DatabaseInserter<CaseParties>
{

    internal const string ORGANIZATIONS = "organizations";
    internal const string PERSONS = "persons";

    internal CasePartiesInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(CaseParties caseParties)
    {
        if (caseParties.Id.HasValue) {
            throw new Exception($"case parties id should be null upon creation");
        }
        WriteNullableValue(caseParties.Organizations, ORGANIZATIONS);
        WriteNullableValue(caseParties.Persons, PERSONS);
        caseParties.Id = await _command.ExecuteScalarAsync() switch {
            long i => (int)i,
            _ => throw new Exception("Insert of case parties does not return an id.")
        };
    }
}
