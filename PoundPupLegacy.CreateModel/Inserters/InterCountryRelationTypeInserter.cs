namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class InterCountryRelationTypeInserterFactory : DatabaseInserterFactory<InterCountryRelationType, InterCountryRelationTypeInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableBooleanDatabaseParameter IsSymmetric = new() { Name = "is_symmetric" };
    public override string TableName => "inter_country_relation_type";
}
internal sealed class InterCountryRelationTypeInserter : DatabaseInserter<InterCountryRelationType>
{
    public InterCountryRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(InterCountryRelationType item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(InterCountryRelationTypeInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(InterCountryRelationTypeInserterFactory.IsSymmetric, item.IsSymmetric),
        };
    }
}
