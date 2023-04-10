namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class OrganizationOrganizationTypeInserterFactory : DatabaseInserterFactory<OrganizationOrganizationType>
{
    internal static NonNullableIntegerDatabaseParameter OrganizationId = new() { Name = "organization_id" };
    internal static NonNullableIntegerDatabaseParameter OrganizationTypeId = new() { Name = "organization_type_id" };

    public override async Task<IDatabaseInserter<OrganizationOrganizationType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "organization_organization_type",
            new DatabaseParameter[] {
                OrganizationId,
                OrganizationTypeId
            }
        );
        return new OrganizationOrganizationTypeInserter(command);
    }
}
internal sealed class OrganizationOrganizationTypeInserter : DatabaseInserter<OrganizationOrganizationType>
{
    internal OrganizationOrganizationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(OrganizationOrganizationType organizationOrganizationType)
    {
        if (organizationOrganizationType.OrganizationId is null) {
            throw new NullReferenceException(nameof(organizationOrganizationType.OrganizationTypeId));
        }
        Set(OrganizationOrganizationTypeInserterFactory.OrganizationId,organizationOrganizationType.OrganizationId.Value);
        Set(OrganizationOrganizationTypeInserterFactory.OrganizationTypeId,organizationOrganizationType.OrganizationTypeId);
        await _command.ExecuteNonQueryAsync();
    }
}
