namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CasePartyTypeInserterFactory : DatabaseInserterFactory<CasePartyType>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    public override async Task<IDatabaseInserter<CasePartyType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "case_party_type",
            new DatabaseParameter[] {
                Id
            }
        );
        return new CasePartyTypeInserter(command);

    }

}
internal sealed class CasePartyTypeInserter : DatabaseInserter<CasePartyType>
{
    internal CasePartyTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(CasePartyType casePartyType)
    {
        if (casePartyType.Id is null) {
            throw new ArgumentNullException(nameof(casePartyType));
        }
        Set(CasePartyTypeInserterFactory.Id, casePartyType.Id.Value);
        await _command.ExecuteNonQueryAsync();
    }
}
