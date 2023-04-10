namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class InterOrganizationalRelationInserterFactory : DatabaseInserterFactory<InterOrganizationalRelation>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter OrganizationIdFrom = new() { Name = "organization_id_from" };
    internal static NonNullableIntegerDatabaseParameter OrganizationIdTo = new() { Name = "organization_id_to" };
    internal static NullableIntegerDatabaseParameter GeographicalEntityId = new() { Name = "geographical_entity_id" };
    internal static NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    internal static NonNullableIntegerDatabaseParameter InterOrganizationalRelationTypeId = new() { Name = "inter_organizational_relation_type_id" };
    internal static NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };
    internal static NullableStringDatabaseParameter Description = new() { Name = "description" };
    internal static NullableDecimalDatabaseParameter MoneyInvolved = new() { Name = "money_involved" };
    internal static NullableIntegerDatabaseParameter NumberOfChildrenInvolved = new() { Name = "number_of_children_involved" };

    public override async Task<IDatabaseInserter<InterOrganizationalRelation>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "inter_organizational_relation",
            new DatabaseParameter[] {
                Id,
                OrganizationIdFrom,
                OrganizationIdTo,
                GeographicalEntityId,
                DateRange,
                InterOrganizationalRelationTypeId,
                DocumentIdProof,
                Description,
                MoneyInvolved,
                NumberOfChildrenInvolved
            }
        );
        return new InterOrganizationalRelationInserter(command);

    }

}
internal sealed class InterOrganizationalRelationInserter : DatabaseInserter<InterOrganizationalRelation>
{
    internal InterOrganizationalRelationInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(InterOrganizationalRelation interOrganizationalRelation)
    {
        if (interOrganizationalRelation.Id is null)
            throw new NullReferenceException();
        Set(InterOrganizationalRelationInserterFactory.Id, interOrganizationalRelation.Id.Value);
        Set(InterOrganizationalRelationInserterFactory.OrganizationIdFrom, interOrganizationalRelation.OrganizationIdFrom);
        Set(InterOrganizationalRelationInserterFactory.OrganizationIdTo, interOrganizationalRelation.OrganizationIdTo);
        Set(InterOrganizationalRelationInserterFactory.GeographicalEntityId, interOrganizationalRelation.GeographicalEntityId);
        Set(InterOrganizationalRelationInserterFactory.InterOrganizationalRelationTypeId, interOrganizationalRelation.InterOrganizationalRelationTypeId);
        Set(InterOrganizationalRelationInserterFactory.DateRange, interOrganizationalRelation.DateRange);
        Set(InterOrganizationalRelationInserterFactory.DocumentIdProof, interOrganizationalRelation.DocumentIdProof);
        Set(InterOrganizationalRelationInserterFactory.Description, interOrganizationalRelation.Description);
        Set(InterOrganizationalRelationInserterFactory.MoneyInvolved, interOrganizationalRelation.MoneyInvolved);
        Set(InterOrganizationalRelationInserterFactory.NumberOfChildrenInvolved, interOrganizationalRelation.NumberOfChildrenInvolved);
        await _command.ExecuteNonQueryAsync();
    }
}
