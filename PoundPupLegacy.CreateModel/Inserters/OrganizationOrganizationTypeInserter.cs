namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class OrganizationOrganizationTypeInserterFactory : DatabaseInserterFactory<OrganizationOrganizationType>
{
    public override async Task<IDatabaseInserter<OrganizationOrganizationType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "organization_organization_type",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = OrganizationOrganizationTypeInserter.ORGANIZATION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = OrganizationOrganizationTypeInserter.ORGANIZATION_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new OrganizationOrganizationTypeInserter(command);
    }
}
internal sealed class OrganizationOrganizationTypeInserter : DatabaseInserter<OrganizationOrganizationType>
{

    internal const string ORGANIZATION_ID = "organization_id";
    internal const string ORGANIZATION_TYPE_ID = "organization_type_id";

    internal OrganizationOrganizationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(OrganizationOrganizationType organizationOrganizationType)
    {
        if (organizationOrganizationType.OrganizationId is null) {
            throw new NullReferenceException(nameof(organizationOrganizationType.OrganizationTypeId));
        }
        WriteValue(organizationOrganizationType.OrganizationId, ORGANIZATION_ID);
        WriteValue(organizationOrganizationType.OrganizationTypeId, ORGANIZATION_TYPE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
