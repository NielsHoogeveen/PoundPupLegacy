namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class InterPersonalRelationTypeInserterFactory : BasicDatabaseInserterFactory<InterPersonalRelationType, InterPersonalRelationTypeInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableBooleanDatabaseParameter IsSymmetric = new() { Name = "is_symmetric" };

    public override string TableName => "inter_personal_relation_type";
}
internal sealed class InterPersonalRelationTypeInserter : BasicDatabaseInserter<InterPersonalRelationType>
{
    public InterPersonalRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(InterPersonalRelationType item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
                   ParameterValue.Create(InterPersonalRelationTypeInserterFactory.Id, item.Id.Value),
                   ParameterValue.Create(InterPersonalRelationTypeInserterFactory.IsSymmetric, item.IsSymmetric),
               };
    }
}
