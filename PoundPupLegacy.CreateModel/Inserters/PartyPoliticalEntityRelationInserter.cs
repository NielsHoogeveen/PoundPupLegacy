namespace PoundPupLegacy.CreateModel.Inserters;

using Request = PartyPoliticalEntityRelation;

internal sealed class PartyPoliticalEntityRelationInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter PoliticalEntityId = new() { Name = "political_entity_id" };
    internal static NonNullableIntegerDatabaseParameter PartyId = new() { Name = "party_id" };
    internal static NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    internal static NonNullableIntegerDatabaseParameter PartyPoliticalEntityRelationTypeId = new() { Name = "party_political_entity_relation_type_id" };
    internal static NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };

    public override string TableName => "party_political_entity_relation";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(PartyId, request.PartyId),
            ParameterValue.Create(PoliticalEntityId, request.PoliticalEntityId),
            ParameterValue.Create(PartyPoliticalEntityRelationTypeId, request.PartyPoliticalEntityRelationTypeId),
            ParameterValue.Create(DateRange, request.DateRange),
            ParameterValue.Create(DocumentIdProof,request.DocumentIdProof),
        };
    }
}
