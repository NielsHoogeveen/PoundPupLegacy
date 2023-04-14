namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = HouseTermInserterFactory;
using Request = HouseTerm;
using Inserter = HouseTermInserter;

internal sealed class HouseTermInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NullCheckingIntegerDatabaseParameter RepresentativeId = new() { Name = "representative_id" };
    internal static NonNullableIntegerDatabaseParameter SubdivisionId = new() { Name = "subdivision_id" };
    internal static NullableIntegerDatabaseParameter District = new() { Name = "district" };
    internal static NonNullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };

    public override string TableName => "house_term";
}
internal sealed class HouseTermInserter : IdentifiableDatabaseInserter<Request>
{

    public HouseTermInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.RepresentativeId, request.RepresentativeId),
            ParameterValue.Create(Factory.SubdivisionId, request.SubdivisionId),
            ParameterValue.Create(Factory.District, request.District),
            ParameterValue.Create(Factory.DateRange, request.DateTimeRange),
        };
    }
}
