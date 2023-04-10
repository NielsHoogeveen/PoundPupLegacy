namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PersonOrganizationRelationTypeInserterFactory : DatabaseInserterFactory<PersonOrganizationRelationType, PersonOrganizationRelationTypeInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableBooleanDatabaseParameter HasConcreteSubtype = new() { Name = "has_concrete_subtype" };

    public override string TableName => "person_organization_relation_type";
}
internal sealed class PersonOrganizationRelationTypeInserter : DatabaseInserter<PersonOrganizationRelationType>
{
    public PersonOrganizationRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(PersonOrganizationRelationType item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(PersonOrganizationRelationTypeInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(PersonOrganizationRelationTypeInserterFactory.HasConcreteSubtype, item.HasConcreteSubtype),
        };
    }
}
