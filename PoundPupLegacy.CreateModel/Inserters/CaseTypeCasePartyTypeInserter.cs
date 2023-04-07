namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CaseTypeCasePartyTypeInserterFactory : DatabaseInserterFactory<CaseTypeCasePartyType>
{
    public override async Task<IDatabaseInserter<CaseTypeCasePartyType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "case_type_case_party_type",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = CaseTypeCasePartyTypeInserter.CASE_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = CaseTypeCasePartyTypeInserter.CASE_PARTY_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new CaseTypeCasePartyTypeInserter(command);

    }

}
internal sealed class CaseTypeCasePartyTypeInserter : DatabaseInserter<CaseTypeCasePartyType>
{

    internal const string CASE_TYPE_ID = "case_type_id";
    internal const string CASE_PARTY_TYPE_ID = "case_party_type_id";

    internal CaseTypeCasePartyTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(CaseTypeCasePartyType caseTypeCasePartyType)
    {
        SetParameter(caseTypeCasePartyType.CaseTypeId, CASE_TYPE_ID);
        SetNullableParameter(caseTypeCasePartyType.CasePartyTypeId, CASE_PARTY_TYPE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
