namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class CasePartiesWriter : DatabaseWriter<CaseParties>, IDatabaseWriter<CaseParties>
{

    private const string ORGANIZATIONS = "organizations";
    private const string PERSONS = "persons";
    public static async Task<DatabaseWriter<CaseParties>> CreateAsync(NpgsqlConnection connection)
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
        return new CasePartiesWriter(command);

    }

    internal CasePartiesWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(CaseParties caseParties)
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
