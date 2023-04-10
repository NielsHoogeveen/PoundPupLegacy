namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class OrganizationTypeInserterFactory : DatabaseInserterFactory<OrganizationType, OrganizationTypeInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableBooleanDatabaseParameter HasConcreteSubtype = new() { Name = "has_concrete_subtype" };

    public override string TableName => "organization_type";

}
internal sealed class OrganizationTypeInserter : DatabaseInserter<OrganizationType>
{
    public OrganizationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(OrganizationType item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(OrganizationTypeInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(OrganizationTypeInserterFactory.HasConcreteSubtype, item.HasConcreteSubtype),
        };
    }
}
