namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CasePartiesOrganizationInserterFactory : DatabaseInserterFactory<CasePartiesOrganization>
{
    public override async Task<IDatabaseInserter<CasePartiesOrganization>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "case_parties_organization",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = CasePartiesOrganizationInserter.CASE_PARTIES_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = CasePartiesOrganizationInserter.ORGANIZATION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new CasePartiesOrganizationInserter(command);

    }

}
internal sealed class CasePartiesOrganizationInserter : DatabaseInserter<CasePartiesOrganization>
{

    internal const string CASE_PARTIES_ID = "case_parties_id";
    internal const string ORGANIZATION_ID = "organization_id";

    internal CasePartiesOrganizationInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(CasePartiesOrganization casePartiesOrganization)
    {
        SetParameter(casePartiesOrganization.CasePartiesId, CASE_PARTIES_ID);
        SetParameter(casePartiesOrganization.OrganizationId, ORGANIZATION_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
