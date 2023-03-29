namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class CaseCasePartiesWriter : DatabaseWriter<CaseCaseParties>, IDatabaseWriter<CaseCaseParties>
{

    private const string CASE_ID = "case_id";
    private const string CASE_PARTIES_ID = "case_parties_id";
    private const string CASE_PARTY_TYPE_ID = "case_party_type_id";
    public static async Task<DatabaseWriter<CaseCaseParties>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "case_case_parties",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = CASE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = CASE_PARTIES_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = CASE_PARTY_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new CaseCasePartiesWriter(command);

    }

    internal CaseCasePartiesWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(CaseCaseParties caseCaseParties)
    {
        WriteValue(caseCaseParties.CaseId, CASE_ID);
        WriteValue(caseCaseParties.CaseParties.Id, CASE_PARTIES_ID);
        WriteValue(caseCaseParties.CasePartyTypeId, CASE_PARTY_TYPE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
