namespace PoundPupLegacy.CreateModel.Inserters;

using Request = Location;

internal sealed class LocationInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Request>
{
    internal static NullableStringDatabaseParameter Street = new() { Name = "street" };
    internal static NullableStringDatabaseParameter Additional = new() { Name = "additional" };
    internal static NullableStringDatabaseParameter City = new() { Name = "city" };
    internal static NullableStringDatabaseParameter PostalCode = new() { Name = "postal_code" };
    internal static NullableIntegerDatabaseParameter SubdivisionId = new() { Name = "subdivision_id" };
    internal static NonNullableIntegerDatabaseParameter CountryId = new() { Name = "country_id" };
    internal static NullableDecimalDatabaseParameter Latitude = new() { Name = "latitude" };
    internal static NullableDecimalDatabaseParameter Longitude = new() { Name = "longitude" };

    public override string TableName => "location";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Street, request.Street),
            ParameterValue.Create(Additional, request.Additional),
            ParameterValue.Create(City, request.City),
            ParameterValue.Create(PostalCode, request.PostalCode),
            ParameterValue.Create(SubdivisionId, request.SubdivisionId),
            ParameterValue.Create(CountryId, request.CountryId),
            ParameterValue.Create(Latitude, request.Latitude),
            ParameterValue.Create(Longitude, request.Longitude)
        };
    }
}

