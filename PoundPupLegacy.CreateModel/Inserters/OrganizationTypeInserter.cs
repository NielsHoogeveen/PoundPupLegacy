namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class OrganizationTypeInserterFactory : DatabaseInserterFactory<OrganizationType>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableBooleanDatabaseParameter HasConcreteSubtype = new() { Name = "has_concrete_subtype" };

    public override async Task<IDatabaseInserter<OrganizationType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "organization_type",
            new DatabaseParameter[] {
                Id,
                HasConcreteSubtype
            }
        );
        return new OrganizationTypeInserter(command);
    }
}
internal sealed class OrganizationTypeInserter : DatabaseInserter<OrganizationType>
{
    internal OrganizationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(OrganizationType organizationType)
    {
        if (organizationType.Id is null)
            throw new NullReferenceException();

        Set(OrganizationTypeInserterFactory.Id,organizationType.Id.Value);
        Set(OrganizationTypeInserterFactory.HasConcreteSubtype,organizationType.HasConcreteSubtype);
        await _command.ExecuteNonQueryAsync();
    }
}
