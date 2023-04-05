namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class InterCountryRelationInserterFactory : DatabaseInserterFactory<InterCountryRelation>
{
    public override async Task<IDatabaseInserter<InterCountryRelation>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "inter_country_relation",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = InterCountryRelationInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = InterCountryRelationInserter.COUNTRY_ID_FROM,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = InterCountryRelationInserter.COUNTRY_ID_TO,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = InterCountryRelationInserter.NUMBER_OF_CHILDREN_INVOLVED,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = InterCountryRelationInserter.MONEY_INVOLVED,
                    NpgsqlDbType = NpgsqlDbType.Numeric
                },
                new ColumnDefinition{
                    Name = InterCountryRelationInserter.DATE_RANGE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
                new ColumnDefinition{
                    Name = InterCountryRelationInserter.DOCUMENT_ID_PROOF,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = InterCountryRelationInserter.INTER_COUNTRY_RELATION_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new InterCountryRelationInserter(command);
    }

}
internal sealed class InterCountryRelationInserter : DatabaseInserter<InterCountryRelation>
{

    internal const string ID = "id";
    internal const string COUNTRY_ID_FROM = "country_id_from";
    internal const string COUNTRY_ID_TO = "country_id_to";
    internal const string DATE_RANGE = "date_range";
    internal const string NUMBER_OF_CHILDREN_INVOLVED = "number_of_children_involved";
    internal const string MONEY_INVOLVED = "money_involved";
    internal const string INTER_COUNTRY_RELATION_TYPE_ID = "inter_country_relation_type_id";
    internal const string DOCUMENT_ID_PROOF = "document_id_proof";

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
