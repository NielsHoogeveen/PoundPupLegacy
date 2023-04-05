namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PersonOrganizationRelationTypeInserterFactory : DatabaseInserterFactory<PersonOrganizationRelationType>
{
    public override async Task<IDatabaseInserter<PersonOrganizationRelationType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "person_organization_relation_type",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = PersonOrganizationRelationTypeInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PersonOrganizationRelationTypeInserter.HAS_CONCRETE_SUBTYPE,
                    NpgsqlDbType = NpgsqlDbType.Boolean
                },
            }
        );
        return new PersonOrganizationRelationTypeInserter(command);
    }
}
internal sealed class PersonOrganizationRelationTypeInserter : DatabaseInserter<PersonOrganizationRelationType>
{
    internal const string ID = "id";
    internal const string HAS_CONCRETE_SUBTYPE = "has_concrete_subtype";

    internal PersonOrganizationRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(PersonOrganizationRelationType personOrganizationRelationType)
    {
        if (personOrganizationRelationType.Id is null)
            throw new NullReferenceException();

        WriteValue(personOrganizationRelationType.Id, ID);
        WriteValue(personOrganizationRelationType.HasConcreteSubtype, HAS_CONCRETE_SUBTYPE);
        await _command.ExecuteNonQueryAsync();
    }
}
