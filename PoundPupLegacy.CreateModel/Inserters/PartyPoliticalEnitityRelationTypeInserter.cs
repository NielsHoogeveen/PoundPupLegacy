namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PartyPoliticalEntityRelationTypeInserterFactory : DatabaseInserterFactory<PartyPoliticalEntityRelationType, PartyPoliticalEntityRelationTypeInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableBooleanDatabaseParameter HasConcreteSubtype = new() { Name = "has_concrete_subtype" };

    public override string TableName => "party_political_entity_relation_type";
}
internal sealed class PartyPoliticalEntityRelationTypeInserter : DatabaseInserter<PartyPoliticalEntityRelationType>
{
    public PartyPoliticalEntityRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(PartyPoliticalEntityRelationType item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(PartyPoliticalEntityRelationTypeInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(PartyPoliticalEntityRelationTypeInserterFactory.HasConcreteSubtype, item.HasConcreteSubtype),
        };
    }
}
