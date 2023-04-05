using System.Collections.Immutable;

namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PersonOrganizationRelationInserterFactory : DatabaseInserterFactory<PersonOrganizationRelation>
{
    public override async Task<IDatabaseInserter<PersonOrganizationRelation>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var columnDefinitions = new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = PersonOrganizationRelationInserter.PERSON_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PersonOrganizationRelationInserter.ORGANIZATION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PersonOrganizationRelationInserter.GEOGRAPHICAL_ENTITY_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PersonOrganizationRelationInserter.PERSON_ORGANIZATION_RELATION_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PersonOrganizationRelationInserter.DATE_RANGE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
                new ColumnDefinition{
                    Name = PersonOrganizationRelationInserter.DOCUMENT_ID_PROOF,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PersonOrganizationRelationInserter.DESCRIPTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            };

        var generateIdCommand = await CreateIdentityInsertStatementAsync(
            postgresConnection,
            "person_organization_relation",
            columnDefinitions
        );

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "person_organization_relation",
            columnDefinitions.ToImmutableList().Add(new ColumnDefinition {
                Name = PersonOrganizationRelationInserter.ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            })
        );
        return new PersonOrganizationRelationInserter(command, generateIdCommand);
    }
}
internal sealed class PersonOrganizationRelationInserter : DatabaseInserter<PersonOrganizationRelation>
{

    internal const string ID = "id";
    internal const string PERSON_ID = "person_id";
    internal const string ORGANIZATION_ID = "organization_id";
    internal const string GEOGRAPHICAL_ENTITY_ID = "geographical_entity_id";
    internal const string DATE_RANGE = "date_range";
    internal const string PERSON_ORGANIZATION_RELATION_TYPE_ID = "person_organization_relation_type_id";
    internal const string DOCUMENT_ID_PROOF = "document_id_proof";
    internal const string DESCRIPTION = "description";
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
