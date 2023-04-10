namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CaseTypeCasePartyTypeInserterFactory : DatabaseInserterFactory<CaseTypeCasePartyType>
{
    internal static NonNullableIntegerDatabaseParameter CaseTypeId = new() { Name = "case_type_id" };
    internal static NonNullableIntegerDatabaseParameter CasePartyTypeId = new() { Name = "case_party_type_id" };

    public override async Task<IDatabaseInserter<CaseTypeCasePartyType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "case_type_case_party_type",
            new DatabaseParameter[] {
                CaseTypeId,
                CasePartyTypeId
            }
        );
        return new CaseTypeCasePartyTypeInserter(command);
    }
}
internal sealed class CaseTypeCasePartyTypeInserter : DatabaseInserter<CaseTypeCasePartyType>
{


    internal CaseTypeCasePartyTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(CaseTypeCasePartyType caseTypeCasePartyType)
    {
        Set(CaseTypeCasePartyTypeInserterFactory.CaseTypeId, caseTypeCasePartyType.CaseTypeId);
        Set(CaseTypeCasePartyTypeInserterFactory.CasePartyTypeId, caseTypeCasePartyType.CasePartyTypeId);
        await _command.ExecuteNonQueryAsync();
    }
}
