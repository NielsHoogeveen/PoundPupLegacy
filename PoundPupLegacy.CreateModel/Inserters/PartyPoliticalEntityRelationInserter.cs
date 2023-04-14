namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PartyPoliticalEntityRelationInserterFactory : DatabaseInserterFactory<PartyPoliticalEntityRelation, PartyPoliticalEntityRelationInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter PoliticalEntityId = new() { Name = "political_entity_id" };
    internal static NonNullableIntegerDatabaseParameter PartyId = new() { Name = "party_id" };
    internal static NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    internal static NonNullableIntegerDatabaseParameter PartyPoliticalEntityRelationTypeId = new() { Name = "party_political_entity_relation_type_id" };
    internal static NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };

    public override string TableName => "party_political_entity_relation";

}
internal sealed class PartyPoliticalEntityRelationInserter : DatabaseInserter<PartyPoliticalEntityRelation>
{
    public PartyPoliticalEntityRelationInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(PartyPoliticalEntityRelation item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(PartyPoliticalEntityRelationInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(PartyPoliticalEntityRelationInserterFactory.PartyId, item.PartyId),
            ParameterValue.Create(PartyPoliticalEntityRelationInserterFactory.PoliticalEntityId, item.PoliticalEntityId),
            ParameterValue.Create(PartyPoliticalEntityRelationInserterFactory.PartyPoliticalEntityRelationTypeId, item.PartyPoliticalEntityRelationTypeId),
            ParameterValue.Create(PartyPoliticalEntityRelationInserterFactory.DateRange, item.DateRange),
            ParameterValue.Create(PartyPoliticalEntityRelationInserterFactory.DocumentIdProof,item.DocumentIdProof),
        };
    }
}
