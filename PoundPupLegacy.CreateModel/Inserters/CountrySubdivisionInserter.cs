namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = CountrySubdivisionTypeInserterFactory;
using Request = CountrySubdivisionType;
using Inserter = CountrySubdivisionTypeInserter;

internal sealed class CountrySubdivisionTypeInserterFactory : DatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter CountryId = new() { Name = "country_id" };
    internal static NonNullableIntegerDatabaseParameter SubdivisionTypeId = new() { Name = "subdivision_type_id" };

    public override string TableName => "country_subdivision_type";
}
internal sealed class CountrySubdivisionTypeInserter : DatabaseInserter<Request>
{

    public CountrySubdivisionTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(CountrySubdivisionTypeInserterFactory.CountryId, request.CountryId),
            ParameterValue.Create(CountrySubdivisionTypeInserterFactory.SubdivisionTypeId, request.SubdivisionTypeId),
        };
    }
}
