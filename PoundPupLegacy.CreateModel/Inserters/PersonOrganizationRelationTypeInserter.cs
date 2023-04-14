namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = PersonOrganizationRelationTypeInserterFactory;
using Request = PersonOrganizationRelationType;
using Inserter = PersonOrganizationRelationTypeInserter;

internal sealed class PersonOrganizationRelationTypeInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableBooleanDatabaseParameter HasConcreteSubtype = new() { Name = "has_concrete_subtype" };

    public override string TableName => "person_organization_relation_type";
}
internal sealed class PersonOrganizationRelationTypeInserter : IdentifiableDatabaseInserter<Request>
{
    public PersonOrganizationRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.HasConcreteSubtype, request.HasConcreteSubtype),
        };
    }
}
