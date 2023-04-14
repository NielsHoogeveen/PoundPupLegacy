namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = OrganizationOrganizationTypeInserterFactory;
using Request = OrganizationOrganizationType;
using Inserter = OrganizationOrganizationTypeInserter;

internal sealed class OrganizationOrganizationTypeInserterFactory : DatabaseInserterFactory<Request, Inserter>
{
    internal static NullCheckingIntegerDatabaseParameter OrganizationId = new() { Name = "organization_id" };
    internal static NonNullableIntegerDatabaseParameter OrganizationTypeId = new() { Name = "organization_type_id" };

    public override string TableName => "organization_organization_type";

}
internal sealed class OrganizationOrganizationTypeInserter : DatabaseInserter<Request>
{
    public OrganizationOrganizationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.OrganizationId, request.OrganizationId),
            ParameterValue.Create(Factory.OrganizationTypeId, request.OrganizationTypeId),
        };
    }
}
