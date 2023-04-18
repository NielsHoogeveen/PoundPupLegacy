namespace PoundPupLegacy.CreateModel.Inserters;

using Request = CountrySubdivisionType;

internal sealed class CountrySubdivisionTypeInserterFactory : BasicDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter CountryId = new() { Name = "country_id" };
    internal static NonNullableIntegerDatabaseParameter SubdivisionTypeId = new() { Name = "subdivision_type_id" };

    public override string TableName => "country_subdivision_type";

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(CountryId, request.CountryId),
            ParameterValue.Create(SubdivisionTypeId, request.SubdivisionTypeId),
        };
    }
}
