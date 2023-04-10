namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class ProfessionInserterFactory : DatabaseInserterFactory<Profession, ProfessionInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableBooleanDatabaseParameter HasConcreteSubtype = new() { Name = "has_concrete_subtype" };

    public override string TableName => "profession";

}
internal sealed class ProfessionInserter : DatabaseInserter<Profession>
{

    public ProfessionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(Profession item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(ProfessionInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(ProfessionInserterFactory.HasConcreteSubtype, item.HasConcreteSubtype),
        };
    }
}
