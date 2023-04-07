namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PartyPoliticalEntityRelationInserterFactory : DatabaseInserterFactory<PartyPoliticalEntityRelation>
{
    public override async Task<IDatabaseInserter<PartyPoliticalEntityRelation>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "party_political_entity_relation",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = PartyPoliticalEntityRelationInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PartyPoliticalEntityRelationInserter.PARTY_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PartyPoliticalEntityRelationInserter.POLITICAL_ENTITY_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PartyPoliticalEntityRelationInserter.PARTY_POLITICAL_ENTITY_RELATION_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PartyPoliticalEntityRelationInserter.DATE_RANGE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
                new ColumnDefinition{
                    Name = PartyPoliticalEntityRelationInserter.DOCUMENT_ID_PROOF,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new PartyPoliticalEntityRelationInserter(command);

    }

}
internal sealed class PartyPoliticalEntityRelationInserter : DatabaseInserter<PartyPoliticalEntityRelation>
{

    internal const string ID = "id";
    internal const string POLITICAL_ENTITY_ID = "political_entity_id";
    internal const string PARTY_ID = "party_id";
    internal const string DATE_RANGE = "date_range";
    internal const string PARTY_POLITICAL_ENTITY_RELATION_TYPE_ID = "party_political_entity_relation_type_id";
    internal const string DOCUMENT_ID_PROOF = "document_id_proof";

    internal PartyPoliticalEntityRelationInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(PartyPoliticalEntityRelation partyPoliticalEntityRelation)
    {
        SetParameter(partyPoliticalEntityRelation.Id, ID);
        SetParameter(partyPoliticalEntityRelation.PartyId, PARTY_ID);
        SetParameter(partyPoliticalEntityRelation.PoliticalEntityId, POLITICAL_ENTITY_ID);
        SetParameter(partyPoliticalEntityRelation.PartyPoliticalEntityRelationTypeId, PARTY_POLITICAL_ENTITY_RELATION_TYPE_ID);
        SetDateTimeRangeParameter(partyPoliticalEntityRelation.DateRange, DATE_RANGE);
        SetNullableParameter(partyPoliticalEntityRelation.DocumentIdProof, DOCUMENT_ID_PROOF);
        await _command.ExecuteNonQueryAsync();
    }
}
