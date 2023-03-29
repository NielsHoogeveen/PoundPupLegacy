namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CasePartiesPersonInserter : DatabaseInserter<CasePartiesPerson>, IDatabaseInserter<CasePartiesPerson>
{

    private const string CASE_PARTIES_ID = "case_parties_id";
    private const string PERSON_ID = "person_id";
    public static async Task<DatabaseInserter<CasePartiesPerson>> CreateAsync(NpgsqlConnection connection)
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
        return new CasePartiesPersonInserter(command);

    }

    internal CasePartiesPersonInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(CasePartiesPerson casePartiesPerson)
    {
        WriteValue(casePartiesPerson.CasePartiesId, CASE_PARTIES_ID);
        WriteValue(casePartiesPerson.PersonId, PERSON_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
