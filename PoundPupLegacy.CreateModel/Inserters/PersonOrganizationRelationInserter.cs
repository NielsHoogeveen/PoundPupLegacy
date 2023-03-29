using System.Collections.Immutable;

namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PersonOrganizationRelationInserter : DatabaseInserter<PersonOrganizationRelation>, IDatabaseInserter<PersonOrganizationRelation>
{

    private const string ID = "id";
    private const string PERSON_ID = "person_id";
    private const string ORGANIZATION_ID = "organization_id";
    private const string GEOGRAPHICAL_ENTITY_ID = "geographical_entity_id";
    private const string DATE_RANGE = "date_range";
    private const string PERSON_ORGANIZATION_RELATION_TYPE_ID = "person_organization_relation_type_id";
    private const string DOCUMENT_ID_PROOF = "document_id_proof";
    private const string DESCRIPTION = "description";
    public static async Task<DatabaseInserter<PersonOrganizationRelation>> CreateAsync(NpgsqlConnection connection)
    {
        var columnDefinitions = new ColumnDefinition[] {
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
            };

        var generateIdCommand = await CreateIdentityInsertStatementAsync(
            connection,
            "person_organization_relation",
            columnDefinitions
        );

        var command = await CreateInsertStatementAsync(
            connection,
            "person_organization_relation",
            columnDefinitions.ToImmutableList().Add(new ColumnDefinition {
                Name = ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            })
        );
        return new PersonOrganizationRelationInserter(command, generateIdCommand);

    }
    private readonly NpgsqlCommand _generateIdCommand;
    internal PersonOrganizationRelationInserter(NpgsqlCommand command, NpgsqlCommand generateIdCommand) : base(command)
    {
        _generateIdCommand = generateIdCommand;
    }


    private void DoWrites(PersonOrganizationRelation personOrganizationRelation, NpgsqlCommand command)
    {
        if (personOrganizationRelation.PersonId is null) {
            throw new NullReferenceException(nameof(personOrganizationRelation.PersonId));
        }
        WriteValue(personOrganizationRelation.PersonId, PERSON_ID, command);
        WriteValue(personOrganizationRelation.OrganizationId, ORGANIZATION_ID, command);
        WriteNullableValue(personOrganizationRelation.GeographicalEntityId, GEOGRAPHICAL_ENTITY_ID, command);
        WriteValue(personOrganizationRelation.PersonOrganizationRelationTypeId, PERSON_ORGANIZATION_RELATION_TYPE_ID, command);
        WriteDateTimeRange(personOrganizationRelation.DateRange, DATE_RANGE, command);
        WriteNullableValue(personOrganizationRelation.DocumentIdProof, DOCUMENT_ID_PROOF, command);
        WriteNullableValue(personOrganizationRelation.Description, DESCRIPTION, command);
    }
    public override async Task InsertAsync(PersonOrganizationRelation personOrganizationRelation)
    {
        if (personOrganizationRelation.Id is null) {
            DoWrites(personOrganizationRelation, _generateIdCommand);
            personOrganizationRelation.Id = await _command.ExecuteScalarAsync() switch {
                long i => (int)i,
                _ => throw new Exception("Insert of senator senate bill action does not return an id.")
            };
        }
        else {
            WriteValue(personOrganizationRelation.Id, ID);
            DoWrites(personOrganizationRelation, _command);
            await _command.ExecuteNonQueryAsync();
        }
    }

    public override async ValueTask DisposeAsync()
    {
        await _generateIdCommand.DisposeAsync();
        await base.DisposeAsync();
    }
}
