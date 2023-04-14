namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = InterOrganizationalRelationInserterFactory;
using Request = InterOrganizationalRelation;
using Inserter = InterOrganizationalRelationInserter;


internal sealed class InterOrganizationalRelationInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter OrganizationIdFrom = new() { Name = "organization_id_from" };
    internal static NonNullableIntegerDatabaseParameter OrganizationIdTo = new() { Name = "organization_id_to" };
    internal static NullableIntegerDatabaseParameter GeographicalEntityId = new() { Name = "geographical_entity_id" };
    internal static NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    internal static NonNullableIntegerDatabaseParameter InterOrganizationalRelationTypeId = new() { Name = "inter_organizational_relation_type_id" };
    internal static NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };
    internal static NullableStringDatabaseParameter Description = new() { Name = "description" };
    internal static NullableDecimalDatabaseParameter MoneyInvolved = new() { Name = "money_involved" };
    internal static NullableIntegerDatabaseParameter NumberOfChildrenInvolved = new() { Name = "number_of_children_involved" };

    public override string TableName => "inter_organizational_relation";

}
internal sealed class InterOrganizationalRelationInserter : IdentifiableDatabaseInserter<Request>
{
    public InterOrganizationalRelationInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.OrganizationIdFrom, request.OrganizationIdFrom),
            ParameterValue.Create(Factory.OrganizationIdTo, request.OrganizationIdTo),
            ParameterValue.Create(Factory.GeographicalEntityId, request.GeographicalEntityId),
            ParameterValue.Create(Factory.InterOrganizationalRelationTypeId, request.InterOrganizationalRelationTypeId),
            ParameterValue.Create(Factory.DateRange, request.DateRange),
            ParameterValue.Create(Factory.DocumentIdProof, request.DocumentIdProof),
            ParameterValue.Create(Factory.Description, request.Description),
            ParameterValue.Create(Factory.MoneyInvolved, request.MoneyInvolved),
            ParameterValue.Create(Factory.NumberOfChildrenInvolved, request.NumberOfChildrenInvolved),
        };
    }
}
