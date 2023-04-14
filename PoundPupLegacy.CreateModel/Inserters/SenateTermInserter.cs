namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = SenateTermInserterFactory;
using Request = SenateTerm;
using Inserter = SenateTermInserter;
internal sealed class SenateTermInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NullCheckingIntegerDatabaseParameter SenatorId = new() { Name = "senator_id" };
    internal static NonNullableIntegerDatabaseParameter SubdivisionId = new() { Name = "subdivision_id" };
    internal static NonNullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };

    public override string TableName => "senate_term";
}
internal sealed class SenateTermInserter : IdentifiableDatabaseInserter<Request>
{
    public SenateTermInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.SenatorId, request.SenatorId),
            ParameterValue.Create(Factory.SubdivisionId, request.SubdivisionId),
            ParameterValue.Create(Factory.DateRange, request.DateTimeRange),
        };
    }
}
