namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class InterOrganizationalRelationInserterFactory : DatabaseInserterFactory<InterOrganizationalRelation>
{
    public override async Task<IDatabaseInserter<InterOrganizationalRelation>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "inter_organizational_relation",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = InterOrganizationalRelationInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = InterOrganizationalRelationInserter.ORGANIZATION_ID_FROM,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = InterOrganizationalRelationInserter.ORGANIZATION_ID_TO,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = InterOrganizationalRelationInserter.GEOGRAPHICAL_ENTITY_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = InterOrganizationalRelationInserter.INTER_ORGANIZATIONAL_RELATION_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = InterOrganizationalRelationInserter.DATE_RANGE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
                new ColumnDefinition{
                    Name = InterOrganizationalRelationInserter.DOCUMENT_ID_PROOF,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = InterOrganizationalRelationInserter.DESCRIPTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = InterOrganizationalRelationInserter.MONEY_INVOLVED,
                    NpgsqlDbType = NpgsqlDbType.Numeric
                },
                new ColumnDefinition{
                    Name = InterOrganizationalRelationInserter.NUMBER_OF_CHILDREN_INVOLVED,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new InterOrganizationalRelationInserter(command);

    }

}
internal sealed class InterOrganizationalRelationInserter : DatabaseInserter<InterOrganizationalRelation>
{

    internal const string ID = "id";
    internal const string ORGANIZATION_ID_FROM = "organization_id_from";
    internal const string ORGANIZATION_ID_TO = "organization_id_to";
    internal const string GEOGRAPHICAL_ENTITY_ID = "geographical_entity_id";
    internal const string DATE_RANGE = "date_range";
    internal const string INTER_ORGANIZATIONAL_RELATION_TYPE_ID = "inter_organizational_relation_type_id";
    internal const string DOCUMENT_ID_PROOF = "document_id_proof";
    internal const string DESCRIPTION = "description";
    internal const string MONEY_INVOLVED = "money_involved";
    internal const string NUMBER_OF_CHILDREN_INVOLVED = "number_of_children_involved";

    internal InterOrganizationalRelationInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(InterOrganizationalRelation interOrganizationalRelation)
    {
        SetParameter(interOrganizationalRelation.Id, ID);
        SetParameter(interOrganizationalRelation.OrganizationIdFrom, ORGANIZATION_ID_FROM);
        SetParameter(interOrganizationalRelation.OrganizationIdTo, ORGANIZATION_ID_TO);
        SetNullableParameter(interOrganizationalRelation.GeographicalEntityId, GEOGRAPHICAL_ENTITY_ID);
        SetParameter(interOrganizationalRelation.InterOrganizationalRelationTypeId, INTER_ORGANIZATIONAL_RELATION_TYPE_ID);
        SetDateTimeRangeParameter(interOrganizationalRelation.DateRange, DATE_RANGE);
        SetNullableParameter(interOrganizationalRelation.DocumentIdProof, DOCUMENT_ID_PROOF);
        SetNullableParameter(interOrganizationalRelation.Description, DESCRIPTION);
        SetNullableParameter(interOrganizationalRelation.MoneyInvolved, MONEY_INVOLVED);
        SetNullableParameter(interOrganizationalRelation.NumberOfChildrenInvolved, NUMBER_OF_CHILDREN_INVOLVED);
        await _command.ExecuteNonQueryAsync();
    }
}
