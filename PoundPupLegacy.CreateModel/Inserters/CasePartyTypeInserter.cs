namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CasePartyTypeInserter : DatabaseInserter<CasePartyType>, IDatabaseInserter<CasePartyType>
{

    private const string ID = "id";
    public static async Task<DatabaseInserter<CasePartyType>> CreateAsync(NpgsqlConnection connection)
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
        return new CasePartyTypeInserter(command);

    }

    internal CasePartyTypeInserter(NpgsqlCommand command) : base(command)
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
