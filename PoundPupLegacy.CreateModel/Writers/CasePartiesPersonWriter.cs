namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class CasePartiesPersonWriter : DatabaseWriter<CasePartiesPerson>, IDatabaseWriter<CasePartiesPerson>
{

    private const string CASE_PARTIES_ID = "case_parties_id";
    private const string PERSON_ID = "person_id";
    public static async Task<DatabaseWriter<CasePartiesPerson>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "case_parties_person",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = CASE_PARTIES_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PERSON_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new CasePartiesPersonWriter(command);

    }

    internal CasePartiesPersonWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(CasePartiesPerson casePartiesPerson)
    {
        WriteValue(casePartiesPerson.CasePartiesId, CASE_PARTIES_ID);
        WriteValue(casePartiesPerson.PersonId, PERSON_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
