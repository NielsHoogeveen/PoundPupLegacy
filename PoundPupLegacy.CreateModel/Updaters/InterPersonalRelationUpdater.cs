namespace PoundPupLegacy.CreateModel.Updaters;

using Request = InterPersonalRelation.InterPersonalRelationToUpdate;

internal sealed class InterPersonalRelationUpdaterFactory : DatabaseUpdaterFactory<Request>
{

    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableStringDatabaseParameter Title = new() { Name = "title" };
    private static readonly NonNullableIntegerDatabaseParameter PersonIdFrom = new() { Name = "person_id_from" };
    private static readonly NonNullableIntegerDatabaseParameter PersonIdTo = new() { Name = "person_id_to" };
    private static readonly NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    private static readonly NonNullableIntegerDatabaseParameter InterPersonalRelationTypeId = new() { Name = "inter_personal_relation_type_id" };
    private static readonly NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };
    private static readonly NullableStringDatabaseParameter Description = new() { Name = "description" };

    public override string Sql => $"""
        update node 
        set 
            title=@title
        where id = @node_id;
        update inter_personal_relation 
        set 
            person_id_from = @person_id_from,
            person_id_to = @person_id_to,
            date_range = @date_range,
            inter_personal_relation_type_id = @inter_personal_relation_type_id,
            document_id_proof = @document_id_proof,
            description = @description
        where id = @node_id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(NodeId, request.Identification.Id),
            ParameterValue.Create(Title, request.NodeDetails.Title),
            ParameterValue.Create(PersonIdFrom, request.PersonIdFrom),
            ParameterValue.Create(PersonIdTo, request.PersonIdTo),
            ParameterValue.Create(InterPersonalRelationTypeId, request.InterPersonalRelationDetails.InterPersonalRelationTypeId),
            ParameterValue.Create(DateRange, request.InterPersonalRelationDetails.DateRange),
            ParameterValue.Create(DocumentIdProof, request.InterPersonalRelationDetails.DocumentIdProof),
            ParameterValue.Create(Description, request.InterPersonalRelationDetails.Description),
        };
    }
}
