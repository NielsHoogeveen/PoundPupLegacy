namespace PoundPupLegacy.Db.Writers;

internal sealed class OrganizationOrganizationTypeWriter : DatabaseWriter<OrganizationOrganizationType>, IDatabaseWriter<OrganizationOrganizationType>
{

    private const string ORGANIZATION_ID = "organization_id";
    private const string ORGANIZATION_TYPE_ID = "organization_type_id";
    public static async Task<DatabaseWriter<OrganizationOrganizationType>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "organization_organization_type",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ORGANIZATION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = ORGANIZATION_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new OrganizationOrganizationTypeWriter(command);

    }

    internal OrganizationOrganizationTypeWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(OrganizationOrganizationType organizationOrganizationType)
    {
        if (organizationOrganizationType.OrganizationId is null) {
            throw new NullReferenceException(nameof(organizationOrganizationType.OrganizationTypeId));
        }
        WriteValue(organizationOrganizationType.OrganizationId, ORGANIZATION_ID);
        WriteValue(organizationOrganizationType.OrganizationTypeId, ORGANIZATION_TYPE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
