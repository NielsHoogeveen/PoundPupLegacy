namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class InterPersonalRelationInserter : DatabaseInserter<InterPersonalRelation>, IDatabaseInserter<InterPersonalRelation>
{

    private const string ID = "id";
    private const string PERSON_ID_FROM = "person_id_from";
    private const string PERSON_ID_TO = "person_id_to";
    private const string DATE_RANGE = "date_range";
    private const string INTER_ORGANIZATIONAL_RELATION_TYPE_ID = "inter_personal_relation_type_id";
    private const string DOCUMENT_ID_PROOF = "document_id_proof";
    public static async Task<DatabaseInserter<InterPersonalRelation>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "inter_personal_relation",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PERSON_ID_FROM,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PERSON_ID_TO,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = INTER_ORGANIZATIONAL_RELATION_TYPE_ID,
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
        return new InterPersonalRelationInserter(command);

    }

    internal InterPersonalRelationInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(InterPersonalRelation interPersonalRelation)
    {
        WriteValue(interPersonalRelation.Id, ID);
        WriteValue(interPersonalRelation.PersonIdFrom, PERSON_ID_FROM);
        WriteValue(interPersonalRelation.PersonIdTo, PERSON_ID_TO);
        WriteValue(interPersonalRelation.InterPersonalRelationTypeId, INTER_ORGANIZATIONAL_RELATION_TYPE_ID);
        WriteDateTimeRange(interPersonalRelation.DateRange, DATE_RANGE);
        WriteNullableValue(interPersonalRelation.DocumentIdProof, DOCUMENT_ID_PROOF);
        await _command.ExecuteNonQueryAsync();
    }
}
