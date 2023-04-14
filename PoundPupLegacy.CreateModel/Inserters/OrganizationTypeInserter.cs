namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = OrganizationTypeInserterFactory;
using Request = OrganizationType;
using Inserter = OrganizationTypeInserter;

internal sealed class OrganizationTypeInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableBooleanDatabaseParameter HasConcreteSubtype = new() { Name = "has_concrete_subtype" };

    public override string TableName => "organization_type";

}
internal sealed class OrganizationTypeInserter : IdentifiableDatabaseInserter<Request>
{
    public OrganizationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.HasConcreteSubtype, request.HasConcreteSubtype),
        };
    }
}
