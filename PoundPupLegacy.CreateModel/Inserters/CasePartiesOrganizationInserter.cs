namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CasePartiesOrganizationInserterFactory : DatabaseInserterFactory<CasePartiesOrganization>
{
    internal static NonNullableIntegerDatabaseParameter CasePartiesId = new() { Name = "case_parties_id" };
    internal static NonNullableIntegerDatabaseParameter OrganizationId = new() { Name = "organization_id" };

    public override async Task<IDatabaseInserter<CasePartiesOrganization>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "case_parties_organization",
            new DatabaseParameter[] {
                CasePartiesId,
                OrganizationId
            }
        );
        return new CasePartiesOrganizationInserter(command);

    }

}
internal sealed class CasePartiesOrganizationInserter : DatabaseInserter<CasePartiesOrganization>
{
    internal CasePartiesOrganizationInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(CasePartiesOrganization casePartiesOrganization)
    {
        Set(CasePartiesOrganizationInserterFactory.CasePartiesId, casePartiesOrganization.CasePartiesId);
        Set(CasePartiesOrganizationInserterFactory.OrganizationId, casePartiesOrganization.OrganizationId);
        await _command.ExecuteNonQueryAsync();
    }
}
