namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class SenateTermInserterFactory : DatabaseInserterFactory<SenateTerm, SenateTermInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter SenatorId = new() { Name = "senator_id" };
    internal static NonNullableIntegerDatabaseParameter SubdivisionId = new() { Name = "subdivision_id" };
    internal static NonNullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };

    public override string TableName => "senate_term";
}
internal sealed class SenateTermInserter : DatabaseInserter<SenateTerm>
{
    public SenateTermInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(SenateTerm item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        if (item.SenatorId is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(SenateTermInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(SenateTermInserterFactory.SenatorId, item.SenatorId.Value),
            ParameterValue.Create(SenateTermInserterFactory.SubdivisionId, item.SubdivisionId),
            ParameterValue.Create(SenateTermInserterFactory.DateRange, item.DateTimeRange),
        };
    }
}
