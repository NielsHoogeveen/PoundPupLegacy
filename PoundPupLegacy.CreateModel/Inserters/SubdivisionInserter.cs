namespace PoundPupLegacy.CreateModel.Inserters;

using Request = Subdivision;

internal sealed class SubdivisionInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableIntegerDatabaseParameter CountryId = new() { Name = "country_id" };
    internal static NonNullableIntegerDatabaseParameter SubdivisionTypeId = new() { Name = "subdivision_type_id" };

    public override string TableName => "subdivision";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Name, request.Name),
            ParameterValue.Create(CountryId, request.CountryId),
            ParameterValue.Create(SubdivisionTypeId, request.SubdivisionTypeId),
        };
    }
}
