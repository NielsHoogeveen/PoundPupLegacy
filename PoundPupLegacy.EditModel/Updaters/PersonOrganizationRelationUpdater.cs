namespace PoundPupLegacy.EditModel.Updaters;

using Request = PersonOrganizationRelationUpdaterRequest;

public record PersonOrganizationRelationUpdaterRequest : IRequest
{
    public required int NodeId { get; init; }
    public required string Title { get; init; }
    public required int PersonId { get; set; }
    public required int OrganizationId { get; init; }
    public required int PersonOrganizationRelationTypeId { get; init; }
    public required DateTimeRange DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required int? GeographicalEntityId { get; init; }
    public required string? Description { get; init; }

}

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
            ParameterValue.Create(NodeId, request.NodeId),
            ParameterValue.Create(Title, request.Title),
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
