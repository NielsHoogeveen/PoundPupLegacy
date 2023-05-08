namespace PoundPupLegacy.EditModel.Updaters;

using Request = InterOrganizationalRelationUpdaterRequest;

public record InterOrganizationalRelationUpdaterRequest : IRequest
{
    public required int NodeId { get; init; }
    public required string Title { get; init; }
    public required int OrganizationIdFrom { get; init; }
    public required int OrganizationIdTo { get; init; }
    public required int InterOrganizationalRelationTypeId { get; init; }
    public required DateTimeRange? DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required int? GeographicalEntityId { get; init; }
    public required string? Description { get; init; }
    public required decimal? MoneyInvolved { get; init; }
    public required int? NumberOfChildrenInvolved { get; init; }
}


internal sealed class InterOrganizationalRelationUpdaterFactory : DatabaseUpdaterFactory<Request>
{

    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableStringDatabaseParameter Title = new() { Name = "title" };
    private static readonly NonNullableIntegerDatabaseParameter OrganizationIdFrom = new() { Name = "organization_id_from" };
    private static readonly NonNullableIntegerDatabaseParameter OrganizationIdTo = new() { Name = "organization_id_to" };
    private static readonly NullableIntegerDatabaseParameter GeographicalEntityId = new() { Name = "geographical_entity_id" };
    private static readonly NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    private static readonly NonNullableIntegerDatabaseParameter InterOrganizationalRelationTypeId = new() { Name = "inter_organizational_relation_type_id" };
    private static readonly NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };
    private static readonly NullableStringDatabaseParameter Description = new() { Name = "description" };
    private static readonly NullableDecimalDatabaseParameter MoneyInvolved = new() { Name = "money_involved" };
    private static readonly NullableIntegerDatabaseParameter NumberOfChildrenInvolved = new() { Name = "number_of_children_involved" };

    public override string Sql => $"""
        update node 
        set 
            title=@title
        where id = @node_id;
        update inter_organizational_relation 
        set 
            organization_id_from=@organization_id_from,
            organization_id_to=@organization_id_to,
            geographical_entity_id=@geographical_entity_id,
            date_range=@date_range,
            inter_organizational_relation_type_id=@inter_organizational_relation_type_id,
            document_id_proof=@document_id_proof,
            description=@description,
            money_involved=@money_involved,
            number_of_children_involved=@number_of_children_involved
        where id = @node_id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(NodeId, request.NodeId),
            ParameterValue.Create(Title, request.Title),
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
