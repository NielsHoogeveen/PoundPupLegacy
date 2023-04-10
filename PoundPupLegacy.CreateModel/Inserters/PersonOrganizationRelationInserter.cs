namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PersonOrganizationRelationInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<PersonOrganizationRelation, PersonOrganizationRelationInserter>
{
    internal static AutoGenerateIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter PersonId = new() { Name = "person_id" };
    internal static NonNullableIntegerDatabaseParameter OrganizationId = new() { Name = "organization_id" };
    internal static NullableIntegerDatabaseParameter GeographicalEntityId = new() { Name = "geographical_entity_id" };
    internal static NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    internal static NonNullableIntegerDatabaseParameter PersonOrganizationRelationTypeId = new() { Name = "person_organization_relation_type_id" };
    internal static NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };
    internal static NullableStringDatabaseParameter Description = new() { Name = "description" };

    public override string TableName => "person_organization_relation";

}
internal sealed class PersonOrganizationRelationInserter : ConditionalAutoGenerateIdDatabaseInserter<PersonOrganizationRelation>
{

    public PersonOrganizationRelationInserter(NpgsqlCommand command, NpgsqlCommand generateIdCommand) : base(command, generateIdCommand)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(PersonOrganizationRelation item)
    {
        if (item.PersonId is null) {
            throw new NullReferenceException(nameof(item.PersonId));
        }

        return new ParameterValue[] {
            ParameterValue.Create(PersonOrganizationRelationInserterFactory.Id, item.Id),
            ParameterValue.Create(PersonOrganizationRelationInserterFactory.PersonId, item.PersonId.Value),
            ParameterValue.Create(PersonOrganizationRelationInserterFactory.OrganizationId, item.OrganizationId),
            ParameterValue.Create(PersonOrganizationRelationInserterFactory.GeographicalEntityId, item.GeographicalEntityId),
            ParameterValue.Create(PersonOrganizationRelationInserterFactory.DateRange, item.DateRange),
            ParameterValue.Create(PersonOrganizationRelationInserterFactory.PersonOrganizationRelationTypeId, item.PersonOrganizationRelationTypeId),
            ParameterValue.Create(PersonOrganizationRelationInserterFactory.DocumentIdProof, item.DocumentIdProof),
            ParameterValue.Create(PersonOrganizationRelationInserterFactory.Description, item.Description)
        };
    }
}
