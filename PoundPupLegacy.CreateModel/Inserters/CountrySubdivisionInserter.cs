namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CountrySubdivisionTypeInserterFactory : BasicDatabaseInserterFactory<CountrySubdivisionType, CountrySubdivisionTypeInserter>
{
    internal static NonNullableIntegerDatabaseParameter CountryId = new() { Name = "country_id" };
    internal static NonNullableIntegerDatabaseParameter SubdivisionTypeId = new() { Name = "subdivision_type_id" };

    public override string TableName => "country_subdivision_type";
}
internal sealed class CountrySubdivisionTypeInserter : BasicDatabaseInserter<CountrySubdivisionType>
{

    public CountrySubdivisionTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(CountrySubdivisionType item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(CountrySubdivisionTypeInserterFactory.CountryId, item.CountryId),
            ParameterValue.Create(CountrySubdivisionTypeInserterFactory.SubdivisionTypeId, item.SubdivisionTypeId),
        };
    }
}
