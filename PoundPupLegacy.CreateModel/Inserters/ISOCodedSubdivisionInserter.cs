namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class ISOCodedSubdivisionInserterFactory : DatabaseInserterFactory<ISOCodedSubdivision, ISOCodedSubdivisionInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableFixedStringDatabaseParameter ISO31661_2_Code = new() { Name = "iso_3166_2_code" };

    public override string TableName => "iso_coded_subdivision";
}
internal sealed class ISOCodedSubdivisionInserter : DatabaseInserter<ISOCodedSubdivision>
{
    public ISOCodedSubdivisionInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(ISOCodedSubdivision item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(ISOCodedSubdivisionInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(ISOCodedSubdivisionInserterFactory.ISO31661_2_Code, item.ISO3166_2_Code),
        };
    }
}
