namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = PartyPoliticalEntityRelationInserterFactory;
using Request = PartyPoliticalEntityRelation;
using Inserter = PartyPoliticalEntityRelationInserter;

internal sealed class PartyPoliticalEntityRelationInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter PoliticalEntityId = new() { Name = "political_entity_id" };
    internal static NonNullableIntegerDatabaseParameter PartyId = new() { Name = "party_id" };
    internal static NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    internal static NonNullableIntegerDatabaseParameter PartyPoliticalEntityRelationTypeId = new() { Name = "party_political_entity_relation_type_id" };
    internal static NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };

    public override string TableName => "party_political_entity_relation";

}
internal sealed class PartyPoliticalEntityRelationInserter : IdentifiableDatabaseInserter<Request>
{
    public PartyPoliticalEntityRelationInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.PartyId, request.PartyId),
            ParameterValue.Create(Factory.PoliticalEntityId, request.PoliticalEntityId),
            ParameterValue.Create(Factory.PartyPoliticalEntityRelationTypeId, request.PartyPoliticalEntityRelationTypeId),
            ParameterValue.Create(Factory.DateRange, request.DateRange),
            ParameterValue.Create(Factory.DocumentIdProof,request.DocumentIdProof),
        };
    }
}
