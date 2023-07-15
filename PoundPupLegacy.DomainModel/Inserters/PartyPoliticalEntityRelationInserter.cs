namespace PoundPupLegacy.DomainModel.Inserters;

using Request = PartyPoliticalEntityRelation.ToCreate.ForExistingParty;

internal sealed class PartyPoliticalEntityRelationInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter PoliticalEntityId = new() { Name = "political_entity_id" };
    private static readonly NonNullableIntegerDatabaseParameter PartyId = new() { Name = "party_id" };
    private static readonly NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    private static readonly NonNullableIntegerDatabaseParameter PartyPoliticalEntityRelationTypeId = new() { Name = "party_political_entity_relation_type_id" };
    private static readonly NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };

    public override string TableName => "party_political_entity_relation";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(PartyId, request.PartyId),
            ParameterValue.Create(PoliticalEntityId, request.PartyPoliticalEntityRelationDetails.PoliticalEntityId),
            ParameterValue.Create(PartyPoliticalEntityRelationTypeId, request.PartyPoliticalEntityRelationDetails.PartyPoliticalEntityRelationTypeId),
            ParameterValue.Create(DateRange, request.PartyPoliticalEntityRelationDetails.DateRange),
            ParameterValue.Create(DocumentIdProof,request.PartyPoliticalEntityRelationDetails.DocumentIdProof),
        };
    }
}
