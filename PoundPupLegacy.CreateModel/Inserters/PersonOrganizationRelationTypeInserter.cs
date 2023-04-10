namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PersonOrganizationRelationTypeInserterFactory : DatabaseInserterFactory<PersonOrganizationRelationType>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableBooleanDatabaseParameter HasConcreteSubtype = new() { Name = "has_concrete_subtype" };

    public override async Task<IDatabaseInserter<PersonOrganizationRelationType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "person_organization_relation_type",
            new DatabaseParameter[] {
                Id,
                HasConcreteSubtype
            }
        );
        return new PersonOrganizationRelationTypeInserter(command);
    }
}
internal sealed class PersonOrganizationRelationTypeInserter : DatabaseInserter<PersonOrganizationRelationType>
{
    internal PersonOrganizationRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(PersonOrganizationRelationType personOrganizationRelationType)
    {
        if (personOrganizationRelationType.Id is null)
            throw new NullReferenceException();

        Set(PersonOrganizationRelationTypeInserterFactory.Id, personOrganizationRelationType.Id.Value);
        Set(PersonOrganizationRelationTypeInserterFactory.HasConcreteSubtype, personOrganizationRelationType.HasConcreteSubtype);
        await _command.ExecuteNonQueryAsync();
    }
}
