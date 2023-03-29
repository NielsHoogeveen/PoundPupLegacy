namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class CaseTypeCasePartyTypeWriter : DatabaseWriter<CaseTypeCasePartyType>, IDatabaseWriter<CaseTypeCasePartyType>
{

    private const string CASE_TYPE_ID = "case_type_id";
    private const string CASE_PARTY_TYPE_ID = "case_party_type_id";
    public static async Task<DatabaseWriter<CaseTypeCasePartyType>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "case_type_case_party_type",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = CASE_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = CASE_PARTY_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new CaseTypeCasePartyTypeWriter(command);

    }

    internal CaseTypeCasePartyTypeWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(CaseTypeCasePartyType caseTypeCasePartyType)
    {
        WriteValue(caseTypeCasePartyType.CaseTypeId, CASE_TYPE_ID);
        WriteNullableValue(caseTypeCasePartyType.CasePartyTypeId, CASE_PARTY_TYPE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
