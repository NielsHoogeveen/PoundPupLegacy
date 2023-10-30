using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Net.Http.Json;
using System.Text;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class LocationService(
    NpgsqlDataSource dataSource,
    ILogger<LocationService> logger,
    IConfiguration configuration,
    IEnumerableDatabaseReaderFactory<SubdivisionListItemsReaderRequest, SubdivisionListItem> subdivisionListItemReaderFactory,
    IEnumerableDatabaseReaderFactory<CountryListItemsReaderRequest, CountryListItem> countryListItemReaderFactory
) : DatabaseService(dataSource, logger), ILocationService
{
    public async IAsyncEnumerable<SubdivisionListItem> SubdivisionsOfCountry(int countryId)
    {
        var connection = dataSource.CreateConnection();
        try {
            await connection.OpenAsync();
            await using var reader = await subdivisionListItemReaderFactory.CreateAsync(connection);
            await foreach (var subdivision in reader.ReadAsync(new SubdivisionListItemsReaderRequest { CountryId = countryId })) {
                yield return subdivision;
            }
        }
        finally {
            if (connection.State == ConnectionState.Open) {
                await connection.CloseAsync();
            }
        }
    }
    public async IAsyncEnumerable<CountryListItem> Countries()
    {
        var connection = dataSource.CreateConnection();
        try {
            await connection.OpenAsync();
            await using var reader = await countryListItemReaderFactory.CreateAsync(connection);
            await foreach (var country in reader.ReadAsync(new CountryListItemsReaderRequest())) {
                yield return country;
            }
        }
        finally {
            await connection.CloseAsync();
        }
    }

    public async Task<Location> ValidateLocationAsync(Location location)
    {
        HttpClient client = new();
        var addressBuilder = new StringBuilder();
        if (location.Street is not null) {
            addressBuilder.Append(location.Street.Replace(" ", "%20"));
            addressBuilder.Append("%20");
        }
        if (location.City is not null) {
            addressBuilder.Append(location.City.Replace(" ", "%20"));
            addressBuilder.Append("%20");
        }
        if (location.PostalCode is not null) {
            addressBuilder.Append(location.PostalCode.Replace(" ", "%20"));
            addressBuilder.Append("%20");
        }
        if (location.SubdivisionName is not null) {
            addressBuilder.Append(location.SubdivisionName.Replace(" ", "%20"));
            addressBuilder.Append("%20");
        }
        addressBuilder.Append(location.CountryName.Replace(" ", "$20"));

        var address = addressBuilder.ToString();
        var url = $"{configuration.GetValue<string>("GoogleApiAddress")}address={address}&key={configuration.GetValue<string>("GoogleApiKey")}";
        var response = await client.GetAsync(url);
        var responseLocation = new Location.ToUpdate {
            Street = null,
            Addition = null,
            City = null,
            PostalCode = null,
            Subdivisions = new(),
            SubdivisionId = location.SubdivisionId,
            SubdivisionName = location.SubdivisionName,
            CountryId = location.CountryId,
            CountryName = location.CountryName
        };
        if (response.IsSuccessStatusCode) {
            var content = response.Content;
            var json = await content.ReadFromJsonAsync<Root>();
            if (json is not null && json.results is not null && json.results.Count > 0) {
                var result = json.results[0];
                if (result is not null && result.address_components is not null) {
                    var country = result.address_components.FirstOrDefault(x => x.types!.Contains("country"));
                    if (country is not null && country.short_name == "US") {
                        var streetNumber = result.address_components.FirstOrDefault(x => x.types!.Contains("street_number"))?.long_name;
                        var streetName = result.address_components.FirstOrDefault(x => x.types!.Contains("route"))?.long_name;
                        if (streetNumber is not null && streetName is not null) {
                            responseLocation.Street = $"{streetNumber} {streetName}";
                        }
                    }
                    var postalCode = result.address_components?.FirstOrDefault(x => x.types!.Contains("postal_code"));
                    if (postalCode is not null) {
                        responseLocation.PostalCode = postalCode.long_name;
                    }
                    var neighborhood = result.address_components?.FirstOrDefault(x => x.types!.Contains("neighborhood"));
                    var locality = result.address_components?.FirstOrDefault(x => x.types!.Contains("locality"));
                    if (locality is not null) {
                        if (neighborhood is not null) {
                            responseLocation.City = neighborhood.long_name;
                        }
                        else {
                            responseLocation.City = locality.long_name;
                        }
                    }
                    if (result is not null && result.geometry is not null && result.geometry.location is not null) {
                        responseLocation.Latitude = new decimal(result.geometry.location.lat);
                        responseLocation.Longitude = new decimal(result.geometry.location.lng);
                    }
                    return responseLocation;
                }
            }
        }
        return location;

    }
    public sealed class AddressComponent
    {
        public string? long_name { get; set; }
        public string? short_name { get; set; }
        public List<string>? types { get; set; }
    }

    public sealed class Bounds
    {
        public Northeast? northeast { get; set; }
        public Southwest? southwest { get; set; }
    }

    public sealed class Geometry
    {
        public Bounds? bounds { get; set; }
        public Location2? location { get; set; }
        public string? location_type { get; set; }
        public Viewport? viewport { get; set; }
    }

    public sealed class Location2
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public sealed class Northeast
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public sealed class Result
    {
        public List<AddressComponent>? address_components { get; set; }
        public string? formatted_address { get; set; }
        public Geometry? geometry { get; set; }
        public bool partial_match { get; set; }
        public string? place_id { get; set; }
        public List<string>? types { get; set; }
    }

    public sealed class Root
    {
        public List<Result>? results { get; set; }
        public string? status { get; set; }
    }

    public sealed class Southwest
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public sealed class Viewport
    {
        public Northeast? northeast { get; set; }
        public Southwest? southwest { get; set; }
    }


}
