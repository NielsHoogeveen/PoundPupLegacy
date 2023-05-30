namespace PoundPupLegacy.CreateModel.Updaters;

using Request = PartyPoliticalEntityRelation.ToUpdate;

internal sealed class PartyPoliticalEntityRelationUpdaterFactory : DatabaseUpdaterFactory<Request>
{

    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableStringDatabaseParameter Title = new() { Name = "title" };
    private static readonly NonNullableIntegerDatabaseParameter PoliticalEntityId = new() { Name = "political_entity_id" };
    private static readonly NonNullableIntegerDatabaseParameter PartyId = new() { Name = "party_id" };
    private static readonly NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    private static readonly NonNullableIntegerDatabaseParameter PartyPoliticalEntityRelationTypeId = new() { Name = "party_political_entity_relation_type_id" };
    private static readonly NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };

    public override string Sql => $"""
        update node 
        set 
            title=@title
        where id = @node_id;
        update party_political_entity_relation 
        set 
            political_entity_id=@political_entity_id,
            party_id=@party_id,
            date_range=@date_range,
            party_political_entity_relation_type_id=@party_political_entity_relation_type_id,
            document_id_proof=@document_id_proof
        where id = @node_id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(NodeId, request.Identification.Id),
            ParameterValue.Create(Title, request.NodeDetails.Title),
            ParameterValue.Create(PartyId, request.PartyId),
            ParameterValue.Create(PoliticalEntityId, request.PartyPoliticalEntityRelationDetails.PoliticalEntityId),
            ParameterValue.Create(PartyPoliticalEntityRelationTypeId, request.PartyPoliticalEntityRelationDetails.PartyPoliticalEntityRelationTypeId),
            ParameterValue.Create(DateRange, request.PartyPoliticalEntityRelationDetails.DateRange),
            ParameterValue.Create(DocumentIdProof,request.PartyPoliticalEntityRelationDetails.DocumentIdProof),
        };
    }
}
