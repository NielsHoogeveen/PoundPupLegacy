namespace PoundPupLegacy.Db.Writers;

internal sealed class CasePartyTypeWriter : DatabaseWriter<CasePartyType>, IDatabaseWriter<CasePartyType>
{

    private const string ID = "id";
    public static async Task<DatabaseWriter<CasePartyType>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "case_party_type",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new CasePartyTypeWriter(command);

    }

    internal CasePartyTypeWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(CasePartyType casePartyType)
    {
        if (casePartyType.Id is null) {
            throw new ArgumentNullException(nameof(casePartyType));
        }
        WriteValue(casePartyType.Id, ID);
        await _command.ExecuteNonQueryAsync();
    }
}
