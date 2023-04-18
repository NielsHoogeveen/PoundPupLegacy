namespace PoundPupLegacy.CreateModel.Inserters;

using Request = PartyPoliticalEntityRelationType;

internal sealed class PartyPoliticalEntityRelationTypeInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NonNullableBooleanDatabaseParameter HasConcreteSubtype = new() { Name = "has_concrete_subtype" };

    public override string TableName => "party_political_entity_relation_type";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(HasConcreteSubtype, request.HasConcreteSubtype),
        };
    }
}
