using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Updaters;

internal sealed class LocationUpdaterFactory : DatabaseUpdaterFactory<LocationUpdater>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() {
        Name = "id"
    };
    internal static NullableStringDatabaseParameter Street = new() {
        Name = "street"
    };
    internal static NullableStringDatabaseParameter Additional = new() {
        Name = "additional"
    };
    internal static NullableStringDatabaseParameter City = new() {
        Name = "city"
    };
    internal static NullableStringDatabaseParameter PostalCode = new() {
        Name = "postal_code"
    };
    internal static NullableDecimalDatabaseParameter Latitude = new() {
        Name = "latitude"
    };
    internal static NullableDecimalDatabaseParameter Longitude = new() {
        Name = "longitude"
    };
    internal static NullableIntegerDatabaseParameter SubdivisionId = new() {
        Name = "subdivision_id"
    };
    internal static NonNullableIntegerDatabaseParameter CountryId = new() {
        Name = "country_id"
    };

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

}

internal sealed class LocationUpdater : DatabaseUpdater<LocationUpdater.Request>
{


    public LocationUpdater(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(LocationUpdaterFactory.Id, request.Id),
            ParameterValue.Create(LocationUpdaterFactory.Street, request.Street),
            ParameterValue.Create(LocationUpdaterFactory.Additional, request.Additional),
            ParameterValue.Create(LocationUpdaterFactory.City, request.City),
            ParameterValue.Create(LocationUpdaterFactory.PostalCode, request.PostalCode),
            ParameterValue.Create(LocationUpdaterFactory.Latitude, request.Latitude),
            ParameterValue.Create(LocationUpdaterFactory.Longitude, request.Longitude),
            ParameterValue.Create(LocationUpdaterFactory.SubdivisionId, request.SubdivisionId),
            ParameterValue.Create(LocationUpdaterFactory.CountryId, request.CountryId)
        };
    }
    public record Request
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
}
