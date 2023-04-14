namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class InterPersonalRelationTypeInserterFactory : DatabaseInserterFactory<InterPersonalRelationType, InterPersonalRelationTypeInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableBooleanDatabaseParameter IsSymmetric = new() { Name = "is_symmetric" };

    public override string TableName => "inter_personal_relation_type";
}
internal sealed class InterPersonalRelationTypeInserter : DatabaseInserter<InterPersonalRelationType>
{
    public InterPersonalRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(InterPersonalRelationType item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
                   ParameterValue.Create(InterPersonalRelationTypeInserterFactory.Id, item.Id.Value),
                   ParameterValue.Create(InterPersonalRelationTypeInserterFactory.IsSymmetric, item.IsSymmetric),
               };
    }
}
