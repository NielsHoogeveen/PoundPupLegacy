namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class InterOrganizationalRelationInserterFactory : BasicDatabaseInserterFactory<InterOrganizationalRelation, InterOrganizationalRelationInserter>
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

    public override string TableName => "inter_organizational_relation";

}
internal sealed class InterOrganizationalRelationInserter : BasicDatabaseInserter<InterOrganizationalRelation>
{
    public InterOrganizationalRelationInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(InterOrganizationalRelation item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(InterOrganizationalRelationInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(InterOrganizationalRelationInserterFactory.OrganizationIdFrom, item.OrganizationIdFrom),
            ParameterValue.Create(InterOrganizationalRelationInserterFactory.OrganizationIdTo, item.OrganizationIdTo),
            ParameterValue.Create(InterOrganizationalRelationInserterFactory.GeographicalEntityId, item.GeographicalEntityId),
            ParameterValue.Create(InterOrganizationalRelationInserterFactory.InterOrganizationalRelationTypeId, item.InterOrganizationalRelationTypeId),
            ParameterValue.Create(InterOrganizationalRelationInserterFactory.DateRange, item.DateRange),
            ParameterValue.Create(InterOrganizationalRelationInserterFactory.DocumentIdProof, item.DocumentIdProof),
            ParameterValue.Create(InterOrganizationalRelationInserterFactory.Description, item.Description),
            ParameterValue.Create(InterOrganizationalRelationInserterFactory.MoneyInvolved, item.MoneyInvolved),
            ParameterValue.Create(InterOrganizationalRelationInserterFactory.NumberOfChildrenInvolved, item.NumberOfChildrenInvolved),
        };
    }
}
