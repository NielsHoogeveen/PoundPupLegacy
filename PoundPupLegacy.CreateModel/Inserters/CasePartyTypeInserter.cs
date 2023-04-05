namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CasePartyTypeInserterFactory : DatabaseInserterFactory<CasePartyType>
{
    public override async Task<IDatabaseInserter<CasePartyType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "case_party_type",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = CasePartyTypeInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new CasePartyTypeInserter(command);

    }

}
internal sealed class CasePartyTypeInserter : DatabaseInserter<CasePartyType>
{

    internal const string ID = "id";

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
