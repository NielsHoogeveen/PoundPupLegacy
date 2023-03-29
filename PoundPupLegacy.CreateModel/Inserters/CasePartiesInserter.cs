namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CasePartiesInserter : DatabaseInserter<CaseParties>, IDatabaseInserter<CaseParties>
{

    private const string ORGANIZATIONS = "organizations";
    private const string PERSONS = "persons";
    public static async Task<DatabaseInserter<CaseParties>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateIdentityInsertStatementAsync(
            connection,
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
