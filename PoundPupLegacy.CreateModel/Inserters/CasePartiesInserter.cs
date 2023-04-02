namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CasePartiesInserter : DatabaseInserter<CaseParties>, IDatabaseInserter<CaseParties>
{

    private const string ORGANIZATIONS = "organizations";
    private const string PERSONS = "persons";
    public static async Task<DatabaseInserter<CaseParties>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateIdentityInsertStatementAsync(
            postgresConnection,
            "case_parties",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ORGANIZATIONS,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = PERSONS,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new CasePartiesInserter(command);

    }

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
