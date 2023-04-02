namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CasePartyTypeInserter : DatabaseInserter<CasePartyType>, IDatabaseInserter<CasePartyType>
{

    private const string ID = "id";
    public static async Task<DatabaseInserter<CasePartyType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
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

    public override async Task InsertAsync(CasePartyType casePartyType)
    {
        if (casePartyType.Id is null) {
            throw new ArgumentNullException(nameof(casePartyType));
        }
        WriteValue(casePartyType.Id, ID);
        await _command.ExecuteNonQueryAsync();
    }
}
