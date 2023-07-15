namespace PoundPupLegacy.DomainModel.Updaters;

using Request = InterOrganizationalRelation.ToUpdate;

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
            ParameterValue.Create(NodeId, request.Identification.Id),
            ParameterValue.Create(Title, request.NodeDetails.Title),
            ParameterValue.Create(OrganizationIdFrom, request.OrganizationIdFrom),
            ParameterValue.Create(OrganizationIdTo, request.OrganizationIdTo),
            ParameterValue.Create(GeographicalEntityId, request.InterOrganizationalRelationDetails.GeographicalEntityId),
            ParameterValue.Create(InterOrganizationalRelationTypeId, request.InterOrganizationalRelationDetails.InterOrganizationalRelationTypeId),
            ParameterValue.Create(DateRange, request.InterOrganizationalRelationDetails.DateRange),
            ParameterValue.Create(DocumentIdProof, request.InterOrganizationalRelationDetails.DocumentIdProof),
            ParameterValue.Create(Description, request.InterOrganizationalRelationDetails.Description),
            ParameterValue.Create(MoneyInvolved, request.InterOrganizationalRelationDetails.MoneyInvolved),
            ParameterValue.Create(NumberOfChildrenInvolved, request.InterOrganizationalRelationDetails.NumberOfChildrenInvolved),
        };
    }
}
