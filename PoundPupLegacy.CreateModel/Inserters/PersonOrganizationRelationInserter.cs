namespace PoundPupLegacy.CreateModel.Inserters;

using Request = PersonOrganizationRelation;

internal sealed class PersonOrganizationRelationInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Request>
{
    internal static NullCheckingIntegerDatabaseParameter PersonId = new() { Name = "person_id" };
    internal static NonNullableIntegerDatabaseParameter OrganizationId = new() { Name = "organization_id" };
    internal static NullableIntegerDatabaseParameter GeographicalEntityId = new() { Name = "geographical_entity_id" };
    internal static NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    internal static NonNullableIntegerDatabaseParameter PersonOrganizationRelationTypeId = new() { Name = "person_organization_relation_type_id" };
    internal static NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };
    internal static NullableStringDatabaseParameter Description = new() { Name = "description" };

    public override string TableName => "person_organization_relation";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(PersonId, request.PersonId),
            ParameterValue.Create(OrganizationId, request.OrganizationId),
            ParameterValue.Create(GeographicalEntityId, request.GeographicalEntityId),
            ParameterValue.Create(DateRange, request.DateRange),
            ParameterValue.Create(PersonOrganizationRelationTypeId, request.PersonOrganizationRelationTypeId),
            ParameterValue.Create(DocumentIdProof, request.DocumentIdProof),
            ParameterValue.Create(Description, request.Description)
        };
    }
}
