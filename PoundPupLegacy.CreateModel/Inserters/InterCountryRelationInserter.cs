namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class InterCountryRelationInserter : DatabaseInserter<InterCountryRelation>, IDatabaseInserter<InterCountryRelation>
{

    private const string ID = "id";
    private const string COUNTRY_ID_FROM = "country_id_from";
    private const string COUNTRY_ID_TO = "country_id_to";
    private const string DATE_RANGE = "date_range";
    private const string NUMBER_OF_CHILDREN_INVOLVED = "number_of_children_involved";
    private const string MONEY_INVOLVED = "money_involved";
    private const string INTER_COUNTRY_RELATION_TYPE_ID = "inter_country_relation_type_id";
    private const string DOCUMENT_ID_PROOF = "document_id_proof";
    public static async Task<DatabaseInserter<InterCountryRelation>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "inter_country_relation",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = COUNTRY_ID_FROM,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = COUNTRY_ID_TO,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = NUMBER_OF_CHILDREN_INVOLVED,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = MONEY_INVOLVED,
                    NpgsqlDbType = NpgsqlDbType.Numeric
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
                    Name = INTER_COUNTRY_RELATION_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new InterCountryRelationInserter(command);
    }

    internal InterCountryRelationInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(InterCountryRelation interCountryRelation)
    {
        if (interCountryRelation.Id is null)
            throw new NullReferenceException();
        WriteValue(interCountryRelation.Id, ID);
        WriteValue(interCountryRelation.CountryIdFrom, COUNTRY_ID_FROM);
        WriteValue(interCountryRelation.CountryIdTo, COUNTRY_ID_TO);
        WriteDateTimeRange(interCountryRelation.DateTimeRange, DATE_RANGE);
        WriteValue(interCountryRelation.InterCountryRelationTypeId, INTER_COUNTRY_RELATION_TYPE_ID);
        WriteNullableValue(interCountryRelation.NumberOfChildrenInvolved, NUMBER_OF_CHILDREN_INVOLVED);
        WriteNullableValue(interCountryRelation.MoneyInvolved, MONEY_INVOLVED);
        WriteNullableValue(interCountryRelation.DocumentIdProof, DOCUMENT_ID_PROOF);
        await _command.ExecuteNonQueryAsync();
    }

}
