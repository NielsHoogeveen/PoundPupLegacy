namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class PersonOrganizationRelationTypeWriter : DatabaseWriter<PersonOrganizationRelationType>, IDatabaseWriter<PersonOrganizationRelationType>
{
    private const string ID = "id";
    private const string HAS_CONCRETE_SUBTYPE = "has_concrete_subtype";
    public static async Task<DatabaseWriter<PersonOrganizationRelationType>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "person_organization_relation_type",
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
        return new PersonOrganizationRelationTypeWriter(command);

    }

    internal PersonOrganizationRelationTypeWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(PersonOrganizationRelationType personOrganizationRelationType)
    {
        if (personOrganizationRelationType.Id is null)
            throw new NullReferenceException();

        WriteValue(personOrganizationRelationType.Id, ID);
        WriteValue(personOrganizationRelationType.HasConcreteSubtype, HAS_CONCRETE_SUBTYPE);
        await _command.ExecuteNonQueryAsync();
    }
}
