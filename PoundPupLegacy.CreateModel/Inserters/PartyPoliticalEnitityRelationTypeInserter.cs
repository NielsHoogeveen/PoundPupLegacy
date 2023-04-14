namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = PartyPoliticalEntityRelationTypeInserterFactory;
using Request = PartyPoliticalEntityRelationType;
using Inserter = PartyPoliticalEntityRelationTypeInserter;

internal sealed class PartyPoliticalEntityRelationTypeInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableBooleanDatabaseParameter HasConcreteSubtype = new() { Name = "has_concrete_subtype" };

    public override string TableName => "party_political_entity_relation_type";
}
internal sealed class PartyPoliticalEntityRelationTypeInserter : IdentifiableDatabaseInserter<Request>
{
    public PartyPoliticalEntityRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.HasConcreteSubtype, request.HasConcreteSubtype),
        };
    }
}
