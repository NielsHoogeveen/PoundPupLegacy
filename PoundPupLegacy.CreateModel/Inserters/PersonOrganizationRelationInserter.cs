namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = PersonOrganizationRelationInserterFactory;
using Request = PersonOrganizationRelation;
using Inserter = PersonOrganizationRelationInserter;

internal sealed class PersonOrganizationRelationInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Request, Inserter>
{
    internal static NullCheckingIntegerDatabaseParameter PersonId = new() { Name = "person_id" };
    internal static NonNullableIntegerDatabaseParameter OrganizationId = new() { Name = "organization_id" };
    internal static NullableIntegerDatabaseParameter GeographicalEntityId = new() { Name = "geographical_entity_id" };
    internal static NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    internal static NonNullableIntegerDatabaseParameter PersonOrganizationRelationTypeId = new() { Name = "person_organization_relation_type_id" };
    internal static NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };
    internal static NullableStringDatabaseParameter Description = new() { Name = "description" };

    public override string TableName => "person_organization_relation";

}
internal sealed class PersonOrganizationRelationInserter : ConditionalAutoGenerateIdDatabaseInserter<Request>
{

    public PersonOrganizationRelationInserter(NpgsqlCommand command, NpgsqlCommand generateIdCommand) : base(command, generateIdCommand)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.PersonId, request.PersonId),
            ParameterValue.Create(Factory.OrganizationId, request.OrganizationId),
            ParameterValue.Create(Factory.GeographicalEntityId, request.GeographicalEntityId),
            ParameterValue.Create(Factory.DateRange, request.DateRange),
            ParameterValue.Create(Factory.PersonOrganizationRelationTypeId, request.PersonOrganizationRelationTypeId),
            ParameterValue.Create(Factory.DocumentIdProof, request.DocumentIdProof),
            ParameterValue.Create(Factory.Description, request.Description)
        };
    }
}
