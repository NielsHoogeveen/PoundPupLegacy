namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PartyPoliticalEntityRelationInserterFactory : DatabaseInserterFactory<PartyPoliticalEntityRelation>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter PoliticalEntityId = new() { Name = "political_entity_id" };
    internal static NonNullableIntegerDatabaseParameter PartyId = new() { Name = "party_id" };
    internal static NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    internal static NonNullableIntegerDatabaseParameter PartyPoliticalEntityRelationTypeId = new() { Name = "party_political_entity_relation_type_id" };
    internal static NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };

    public override async Task<IDatabaseInserter<PartyPoliticalEntityRelation>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "party_political_entity_relation",
            new DatabaseParameter[] {
                Id,
                PoliticalEntityId,
                PartyId,
                DateRange,
                PartyPoliticalEntityRelationTypeId,
                DocumentIdProof
            }
        );
        return new PartyPoliticalEntityRelationInserter(command);

    }

}
internal sealed class PartyPoliticalEntityRelationInserter : DatabaseInserter<PartyPoliticalEntityRelation>
{
    internal PartyPoliticalEntityRelationInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(PartyPoliticalEntityRelation partyPoliticalEntityRelation)
    {
        if (partyPoliticalEntityRelation.Id is null)
            throw new NullReferenceException();
        Set(PartyPoliticalEntityRelationInserterFactory.Id, partyPoliticalEntityRelation.Id.Value);
        Set(PartyPoliticalEntityRelationInserterFactory.PartyId, partyPoliticalEntityRelation.PartyId);
        Set(PartyPoliticalEntityRelationInserterFactory.PoliticalEntityId, partyPoliticalEntityRelation.PoliticalEntityId);
        Set(PartyPoliticalEntityRelationInserterFactory.PartyPoliticalEntityRelationTypeId, partyPoliticalEntityRelation.PartyPoliticalEntityRelationTypeId);
        Set(PartyPoliticalEntityRelationInserterFactory.DateRange, partyPoliticalEntityRelation.DateRange);
        Set(PartyPoliticalEntityRelationInserterFactory.DocumentIdProof,partyPoliticalEntityRelation.DocumentIdProof);
        await _command.ExecuteNonQueryAsync();
    }
}
