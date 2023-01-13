namespace PoundPupLegacy.Db.Writers;

internal sealed class OrganizationTypeWriter : DatabaseWriter<OrganizationType>, IDatabaseWriter<OrganizationType>
{
    private const string ID = "id";
    private const string HAS_CONCRETE_SUBTYPE = "has_concrete_subtype";
    public static async Task<DatabaseWriter<OrganizationType>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "organization_type",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = HAS_CONCRETE_SUBTYPE,
                    NpgsqlDbType = NpgsqlDbType.Boolean
                },
            }
        );
        return new OrganizationTypeWriter(command);

    }

    internal OrganizationTypeWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(OrganizationType organizationType)
    {
        if (organizationType.Id is null)
            throw new NullReferenceException();

        WriteValue(organizationType.Id, ID);
        WriteValue(organizationType.HasConcreteSubtype, HAS_CONCRETE_SUBTYPE);
        await _command.ExecuteNonQueryAsync();
    }
}
