using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

using Request = Location.ToCreate;

internal sealed class LocationInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Request>
{
    private static readonly NullableStringDatabaseParameter Street = new() { Name = "street" };
    private static readonly NullableStringDatabaseParameter Additional = new() { Name = "additional" };
    private static readonly NullableStringDatabaseParameter City = new() { Name = "city" };
    private static readonly NullableStringDatabaseParameter PostalCode = new() { Name = "postal_code" };
    private static readonly NullableIntegerDatabaseParameter SubdivisionId = new() { Name = "subdivision_id" };
    private static readonly NonNullableIntegerDatabaseParameter CountryId = new() { Name = "country_id" };
    private static readonly NullableDecimalDatabaseParameter Latitude = new() { Name = "latitude" };
    private static readonly NullableDecimalDatabaseParameter Longitude = new() { Name = "longitude" };

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

