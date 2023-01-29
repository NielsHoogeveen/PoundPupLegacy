namespace PoundPupLegacy.Db.Writers;

internal sealed class PersonOrganizationRelationWriter : DatabaseWriter<PersonOrganizationRelation>, IDatabaseWriter<PersonOrganizationRelation>
{

    private const string ID = "id";
    private const string PERSON_ID = "person_id";
    private const string ORGANIZATION_ID = "organization_id";
    private const string GEOGRAPHICAL_ENTITY_ID = "geographical_entity_id";
    private const string DATE_RANGE = "date_range";
    private const string PERSON_ORGANIZATION_RELATION_TYPE_ID = "person_organization_relation_type_id";
    private const string DOCUMENT_ID_PROOF = "document_id_proof";
    private const string DESCRIPTION = "description";
    public static async Task<DatabaseWriter<PersonOrganizationRelation>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "person_organization_relation",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PERSON_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = ORGANIZATION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = GEOGRAPHICAL_ENTITY_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PERSON_ORGANIZATION_RELATION_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DATE_RANGE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
                new ColumnDefinition{
                    Name = DOCUMENT_ID_PROOF,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DESCRIPTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new PersonOrganizationRelationWriter(command);

    }

    internal PersonOrganizationRelationWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(PersonOrganizationRelation personOrganizationRelation)
    {
        WriteValue(personOrganizationRelation.Id, ID);
        WriteValue(personOrganizationRelation.PersonId, PERSON_ID);
        WriteValue(personOrganizationRelation.OrganizationId, ORGANIZATION_ID);
        WriteNullableValue(personOrganizationRelation.GeographicalEntityId, GEOGRAPHICAL_ENTITY_ID);
        WriteValue(personOrganizationRelation.PersonOrganizationRelationTypeId, PERSON_ORGANIZATION_RELATION_TYPE_ID);
        WriteDateTimeRange(personOrganizationRelation.DateRange, DATE_RANGE);
        WriteNullableValue(personOrganizationRelation.DocumentIdProof, DOCUMENT_ID_PROOF);
        WriteNullableValue(personOrganizationRelation.Description, DESCRIPTION);
        await _command.ExecuteNonQueryAsync();
    }
}
