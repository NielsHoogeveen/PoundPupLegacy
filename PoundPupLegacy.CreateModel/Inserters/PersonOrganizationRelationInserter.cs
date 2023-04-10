using System.Collections.Immutable;

namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PersonOrganizationRelationInserterFactory : DatabaseInserterFactory<PersonOrganizationRelation>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter PersonId = new() { Name = "person_id" };
    internal static NonNullableIntegerDatabaseParameter OrganizationId = new() { Name = "organization_id" };
    internal static NullableIntegerDatabaseParameter GeographicalEntityId = new() { Name = "geographical_entity_id" };
    internal static NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    internal static NonNullableIntegerDatabaseParameter PersonOrganizationRelationTypeId = new() { Name = "person_organization_relation_type_id" };
    internal static NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };
    internal static NullableStringDatabaseParameter Description = new() { Name = "description" };

    public override async Task<IDatabaseInserter<PersonOrganizationRelation>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var databaseParameters = new DatabaseParameter[] {
            PersonId,
            OrganizationId,
            GeographicalEntityId,
            DateRange,
            PersonOrganizationRelationTypeId,
            DocumentIdProof,
            Description
            };

        var generateIdCommand = await CreateAutoGenerateIdentityInsertStatementAsync(
            postgresConnection,
            "person_organization_relation",
            databaseParameters
        );

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "person_organization_relation",
            databaseParameters.ToImmutableList().Add(Id)
        );
        return new PersonOrganizationRelationInserter(command, generateIdCommand);
    }
}
internal sealed class PersonOrganizationRelationInserter : DatabaseInserter<PersonOrganizationRelation>
{

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
        Set(PersonOrganizationRelationInserterFactory.PersonId, personOrganizationRelation.PersonId.Value, command);
        Set(PersonOrganizationRelationInserterFactory.OrganizationId, personOrganizationRelation.OrganizationId, command);
        Set(PersonOrganizationRelationInserterFactory.GeographicalEntityId, personOrganizationRelation.GeographicalEntityId, command);
        Set(PersonOrganizationRelationInserterFactory.PersonOrganizationRelationTypeId, personOrganizationRelation.PersonOrganizationRelationTypeId, command);
        Set(PersonOrganizationRelationInserterFactory.DateRange, personOrganizationRelation.DateRange, command);
        Set(PersonOrganizationRelationInserterFactory.DocumentIdProof, personOrganizationRelation.DocumentIdProof, command);
        Set(PersonOrganizationRelationInserterFactory.Description, personOrganizationRelation.Description, command);
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
            Set(PersonOrganizationRelationInserterFactory.Id, personOrganizationRelation.Id.Value);
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
