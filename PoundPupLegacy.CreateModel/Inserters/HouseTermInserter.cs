namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class HouseTermInserterFactory : DatabaseInserterFactory<HouseTerm, HouseTermInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter RepresentativeId = new() { Name = "representative_id" };
    internal static NonNullableIntegerDatabaseParameter SubdivisionId = new() { Name = "subdivision_id" };
    internal static NullableIntegerDatabaseParameter District = new() { Name = "district" };
    internal static NonNullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };

    public override string TableName => "house_term";
}
internal sealed class HouseTermInserter : DatabaseInserter<HouseTerm>
{

    public HouseTermInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(HouseTerm item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        if (item.RepresentativeId is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(HouseTermInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(HouseTermInserterFactory.RepresentativeId, item.RepresentativeId.Value),
            ParameterValue.Create(HouseTermInserterFactory.SubdivisionId, item.SubdivisionId),
            ParameterValue.Create(HouseTermInserterFactory.District, item.District),
            ParameterValue.Create(HouseTermInserterFactory.DateRange, item.DateTimeRange),
        };
    }
}
