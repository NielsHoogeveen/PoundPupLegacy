namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class SubdivisionInserterFactory : DatabaseInserterFactory<Subdivision, SubdivisionInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableIntegerDatabaseParameter CountryId = new() { Name = "country_id" };
    internal static NonNullableIntegerDatabaseParameter SubdivisionTypeId = new() { Name = "subdivision_type_id" };

    public override string TableName => "subdivision";
}
internal sealed class SubdivisionInserter : DatabaseInserter<Subdivision>
{
    public SubdivisionInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Subdivision item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(SubdivisionInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(SubdivisionInserterFactory.Name, item.Name),
            ParameterValue.Create(SubdivisionInserterFactory.CountryId, item.CountryId),
            ParameterValue.Create(SubdivisionInserterFactory.SubdivisionTypeId, item.SubdivisionTypeId),
        };
    }
}
