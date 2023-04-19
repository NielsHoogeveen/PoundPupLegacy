using PoundPupLegacy.Common;

namespace PoundPupLegacy.Updaters;

using Request = LocationUpdaterRequest;

public record LocationUpdaterRequest: IRequest
{
    public required int Id { get; init; }
    public required string? Street { get; init; }
    public required string? Additional { get; init; }
    public required string? City { get; init; }
    public required string? PostalCode { get; init; }
    public required int? SubdivisionId { get; init; }
    public required int CountryId { get; init; }
    public required decimal? Latitude { get; init; }
    public required decimal? Longitude { get; init; }
}


internal sealed class LocationUpdaterFactory : DatabaseUpdaterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    private static readonly NullableStringDatabaseParameter Street = new() { Name = "street" };
    private static readonly NullableStringDatabaseParameter Additional = new() { Name = "additional" };
    private static readonly NullableStringDatabaseParameter City = new() { Name = "city" };
    private static readonly NullableStringDatabaseParameter PostalCode = new() { Name = "postal_code" };
    private static readonly NullableDecimalDatabaseParameter Latitude = new() { Name = "latitude" };
    private static readonly NullableDecimalDatabaseParameter Longitude = new() { Name = "longitude" };
    private static readonly NullableIntegerDatabaseParameter SubdivisionId = new() { Name = "subdivision_id" };
    private static readonly NonNullableIntegerDatabaseParameter CountryId = new() { Name = "country_id" };

    public override string Sql => $"""
        update location 
        set 
        street = @street, 
        additional = @additional, 
        city = @city,
        postal_code = @postal_code,
        latitude = @latitude,
        longitude = @longitude,
        subdivision_id = @subdivision_id,
        country_id = @country_id
        where id = @id
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(Id, request.Id),
            ParameterValue.Create(Street, request.Street),
            ParameterValue.Create(Additional, request.Additional),
            ParameterValue.Create(City, request.City),
            ParameterValue.Create(PostalCode, request.PostalCode),
            ParameterValue.Create(Latitude, request.Latitude),
            ParameterValue.Create(Longitude, request.Longitude),
            ParameterValue.Create(SubdivisionId, request.SubdivisionId),
            ParameterValue.Create(CountryId, request.CountryId)
        };
    }
}
