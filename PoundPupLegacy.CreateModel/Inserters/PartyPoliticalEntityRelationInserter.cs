namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PartyPoliticalEntityRelationInserter : DatabaseInserter<PartyPoliticalEntityRelation>, IDatabaseInserter<PartyPoliticalEntityRelation>
{

    private const string ID = "id";
    private const string POLITICAL_ENTITY_ID = "political_entity_id";
    private const string PARTY_ID = "party_id";
    private const string DATE_RANGE = "date_range";
    private const string PARTY_POLITICAL_ENTITY_RELATION_TYPE_ID = "party_political_entity_relation_type_id";
    private const string DOCUMENT_ID_PROOF = "document_id_proof";
    public static async Task<DatabaseInserter<PartyPoliticalEntityRelation>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "party_political_entity_relation",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PARTY_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = POLITICAL_ENTITY_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PARTY_POLITICAL_ENTITY_RELATION_TYPE_ID,
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
            }
        );
        return new PartyPoliticalEntityRelationInserter(command);

    }

    internal PartyPoliticalEntityRelationInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(PartyPoliticalEntityRelation partyPoliticalEntityRelation)
    {
        WriteValue(partyPoliticalEntityRelation.Id, ID);
        WriteValue(partyPoliticalEntityRelation.PartyId, PARTY_ID);
        WriteValue(partyPoliticalEntityRelation.PoliticalEntityId, POLITICAL_ENTITY_ID);
        WriteValue(partyPoliticalEntityRelation.PartyPoliticalEntityRelationTypeId, PARTY_POLITICAL_ENTITY_RELATION_TYPE_ID);
        WriteDateTimeRange(partyPoliticalEntityRelation.DateRange, DATE_RANGE);
        WriteNullableValue(partyPoliticalEntityRelation.DocumentIdProof, DOCUMENT_ID_PROOF);
        await _command.ExecuteNonQueryAsync();
    }
}
