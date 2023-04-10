namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class InterOrganizationalRelationTypeInserterFactory : DatabaseInserterFactory<InterOrganizationalRelationType>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableBooleanDatabaseParameter IsSymmetric = new() { Name = "is_symmetric" };

    public override async Task<IDatabaseInserter<InterOrganizationalRelationType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "inter_organizational_relation_type",
            new DatabaseParameter[] {
                Id,
                IsSymmetric
            }
        );
        return new InterOrganizationalRelationTypeInserter(command);
    }

}
internal sealed class InterOrganizationalRelationTypeInserter : DatabaseInserter<InterOrganizationalRelationType>
{
    internal InterOrganizationalRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(InterOrganizationalRelationType interOrganizationalRelationType)
    {
        if (interOrganizationalRelationType.Id is null)
            throw new NullReferenceException();
        Set(InterOrganizationalRelationTypeInserterFactory.Id, interOrganizationalRelationType.Id.Value);
        Set(InterOrganizationalRelationTypeInserterFactory.IsSymmetric, interOrganizationalRelationType.IsSymmetric);
        await _command.ExecuteNonQueryAsync();
    }

}
