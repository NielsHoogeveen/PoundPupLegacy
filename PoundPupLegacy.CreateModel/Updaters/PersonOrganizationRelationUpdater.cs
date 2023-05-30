namespace PoundPupLegacy.CreateModel.Updaters;

using Request = PersonOrganizationRelation.ToUpdate;

internal sealed class PersonOrganizationRelationUpdaterFactory : DatabaseUpdaterFactory<Request>
{

    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableStringDatabaseParameter Title = new() { Name = "title" };
    private static readonly NullCheckingIntegerDatabaseParameter PersonId = new() { Name = "person_id" };
    private static readonly NonNullableIntegerDatabaseParameter OrganizationId = new() { Name = "organization_id" };
    private static readonly NullableIntegerDatabaseParameter GeographicalEntityId = new() { Name = "geographical_entity_id" };
    private static readonly NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    private static readonly NonNullableIntegerDatabaseParameter PersonOrganizationRelationTypeId = new() { Name = "person_organization_relation_type_id" };
    private static readonly NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };
    private static readonly NullableStringDatabaseParameter Description = new() { Name = "description" };

    public override string Sql => $"""
        update node 
        set 
            title=@title
        where id = @node_id;
        update person_organization_relation 
        set 
            person_id=@person_id,
            organization_id=@organization_id,
            geographical_entity_id=@geographical_entity_id,
            date_range=@date_range,
            person_organization_relation_type_id=@person_organization_relation_type_id,
            document_id_proof=@document_id_proof,
            description=@description
        where id = @node_id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(NodeId, request.Identification.Id),
            ParameterValue.Create(Title, request.NodeDetails.Title),
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
