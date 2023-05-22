namespace PoundPupLegacy.CreateModel.Inserters;

using Request = NewInterOrganizationalRelation;

internal sealed class InterOrganizationalRelationInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter OrganizationIdFrom = new() { Name = "organization_id_from" };
    private static readonly NonNullableIntegerDatabaseParameter OrganizationIdTo = new() { Name = "organization_id_to" };
    private static readonly NullableIntegerDatabaseParameter GeographicalEntityId = new() { Name = "geographical_entity_id" };
    private static readonly NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    private static readonly NonNullableIntegerDatabaseParameter InterOrganizationalRelationTypeId = new() { Name = "inter_organizational_relation_type_id" };
    private static readonly NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };
    private static readonly NullableStringDatabaseParameter Description = new() { Name = "description" };
    private static readonly NullableDecimalDatabaseParameter MoneyInvolved = new() { Name = "money_involved" };
    private static readonly NullableIntegerDatabaseParameter NumberOfChildrenInvolved = new() { Name = "number_of_children_involved" };

    public override string TableName => "inter_organizational_relation";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(OrganizationIdFrom, request.OrganizationIdFrom),
            ParameterValue.Create(OrganizationIdTo, request.OrganizationIdTo),
            ParameterValue.Create(GeographicalEntityId, request.GeographicalEntityId),
            ParameterValue.Create(InterOrganizationalRelationTypeId, request.InterOrganizationalRelationTypeId),
            ParameterValue.Create(DateRange, request.DateRange),
            ParameterValue.Create(DocumentIdProof, request.DocumentIdProof),
            ParameterValue.Create(Description, request.Description),
            ParameterValue.Create(MoneyInvolved, request.MoneyInvolved),
            ParameterValue.Create(NumberOfChildrenInvolved, request.NumberOfChildrenInvolved),
        };
    }
}
