namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PersonOrganizationRelationTypeInserter : DatabaseInserter<PersonOrganizationRelationType>, IDatabaseInserter<PersonOrganizationRelationType>
{
    private const string ID = "id";
    private const string HAS_CONCRETE_SUBTYPE = "has_concrete_subtype";
    public static async Task<DatabaseInserter<PersonOrganizationRelationType>> CreateAsync(NpgsqlConnection connection)
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
        return new PersonOrganizationRelationTypeInserter(command);

    }

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
