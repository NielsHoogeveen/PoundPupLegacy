namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class InterOrganizationalRelationInserter : DatabaseInserter<InterOrganizationalRelation>, IDatabaseInserter<InterOrganizationalRelation>
{

    private const string ID = "id";
    private const string ORGANIZATION_ID_FROM = "organization_id_from";
    private const string ORGANIZATION_ID_TO = "organization_id_to";
    private const string GEOGRAPHICAL_ENTITY_ID = "geographical_entity_id";
    private const string DATE_RANGE = "date_range";
    private const string INTER_ORGANIZATIONAL_RELATION_TYPE_ID = "inter_organizational_relation_type_id";
    private const string DOCUMENT_ID_PROOF = "document_id_proof";
    private const string DESCRIPTION = "description";
    private const string MONEY_INVOLVED = "money_involved";
    private const string NUMBER_OF_CHILDREN_INVOLVED = "number_of_children_involved";
    public static async Task<DatabaseInserter<InterOrganizationalRelation>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "inter_organizational_relation",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = ORGANIZATION_ID_FROM,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = ORGANIZATION_ID_TO,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = GEOGRAPHICAL_ENTITY_ID,
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
                new ColumnDefinition{
                    Name = DESCRIPTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = MONEY_INVOLVED,
                    NpgsqlDbType = NpgsqlDbType.Numeric
                },
                new ColumnDefinition{
                    Name = NUMBER_OF_CHILDREN_INVOLVED,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new InterOrganizationalRelationInserter(command);

    }

    internal InterOrganizationalRelationInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(InterOrganizationalRelation interOrganizationalRelation)
    {
        WriteValue(interOrganizationalRelation.Id, ID);
        WriteValue(interOrganizationalRelation.OrganizationIdFrom, ORGANIZATION_ID_FROM);
        WriteValue(interOrganizationalRelation.OrganizationIdTo, ORGANIZATION_ID_TO);
        WriteNullableValue(interOrganizationalRelation.GeographicalEntityId, GEOGRAPHICAL_ENTITY_ID);
        WriteValue(interOrganizationalRelation.InterOrganizationalRelationTypeId, INTER_ORGANIZATIONAL_RELATION_TYPE_ID);
        WriteDateTimeRange(interOrganizationalRelation.DateRange, DATE_RANGE);
        WriteNullableValue(interOrganizationalRelation.DocumentIdProof, DOCUMENT_ID_PROOF);
        WriteNullableValue(interOrganizationalRelation.Description, DESCRIPTION);
        WriteNullableValue(interOrganizationalRelation.MoneyInvolved, MONEY_INVOLVED);
        WriteNullableValue(interOrganizationalRelation.NumberOfChildrenInvolved, NUMBER_OF_CHILDREN_INVOLVED);
        await _command.ExecuteNonQueryAsync();
    }
}
