namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class OrganizationTypeInserterFactory : DatabaseInserterFactory<OrganizationType>
{
    public override async Task<IDatabaseInserter<OrganizationType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "organization_type",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = OrganizationTypeInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = OrganizationTypeInserter.HAS_CONCRETE_SUBTYPE,
                    NpgsqlDbType = NpgsqlDbType.Boolean
                },
            }
        );
        return new OrganizationTypeInserter(command);
    }
}
internal sealed class OrganizationTypeInserter : DatabaseInserter<OrganizationType>
{
    internal const string ID = "id";
    internal const string HAS_CONCRETE_SUBTYPE = "has_concrete_subtype";

    internal OrganizationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(OrganizationType organizationType)
    {
        if (organizationType.Id is null)
            throw new NullReferenceException();

        SetParameter(organizationType.Id, ID);
        SetParameter(organizationType.HasConcreteSubtype, HAS_CONCRETE_SUBTYPE);
        await _command.ExecuteNonQueryAsync();
    }
}
