namespace PoundPupLegacy.CreateModel.Inserters;

using Request = PersonOrganizationRelation.PersonOrganizationRelationToCreateForExistingParticipants;

internal sealed class PersonOrganizationRelationInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter PersonId = new() { Name = "person_id" };
    private static readonly NonNullableIntegerDatabaseParameter OrganizationId = new() { Name = "organization_id" };
    private static readonly NullableIntegerDatabaseParameter GeographicalEntityId = new() { Name = "geographical_entity_id" };
    private static readonly NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    private static readonly NonNullableIntegerDatabaseParameter PersonOrganizationRelationTypeId = new() { Name = "person_organization_relation_type_id" };
    private static readonly NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };
    private static readonly NullableStringDatabaseParameter Description = new() { Name = "description" };

    public override string TableName => "person_organization_relation";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(PersonId, request.PersonId),
            ParameterValue.Create(OrganizationId, request.OrganizationId),
            ParameterValue.Create(GeographicalEntityId, request.PersonOrganizationRelationDetails.GeographicalEntityId),
            ParameterValue.Create(DateRange, request.PersonOrganizationRelationDetails.DateRange),
            ParameterValue.Create(PersonOrganizationRelationTypeId, request.PersonOrganizationRelationDetails.PersonOrganizationRelationTypeId),
            ParameterValue.Create(DocumentIdProof, request.PersonOrganizationRelationDetails.DocumentIdProof),
            ParameterValue.Create(Description, request.PersonOrganizationRelationDetails.Description)
        };
    }
}
