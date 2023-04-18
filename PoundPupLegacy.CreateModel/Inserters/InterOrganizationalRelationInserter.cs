namespace PoundPupLegacy.CreateModel.Inserters;

using Request = InterOrganizationalRelation;

internal sealed class InterOrganizationalRelationInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
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
