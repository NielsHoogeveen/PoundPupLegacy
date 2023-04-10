namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class InterOrganizationalRelationTypeInserterFactory : BasicDatabaseInserterFactory<InterOrganizationalRelationType, InterOrganizationalRelationTypeInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableBooleanDatabaseParameter IsSymmetric = new() { Name = "is_symmetric" };
    public override string TableName => "inter_organizational_relation_type";
}
internal sealed class InterOrganizationalRelationTypeInserter : BasicDatabaseInserter<InterOrganizationalRelationType>
{
    public InterOrganizationalRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(InterOrganizationalRelationType item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(InterOrganizationalRelationTypeInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(InterOrganizationalRelationTypeInserterFactory.IsSymmetric, item.IsSymmetric),
        };
    }
}
