using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.EditModel;
using PoundPupLegacy.EditModel.Readers;
using System.Data;
using System.Text;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class LocationService : ILocationService
{
    private readonly NpgsqlConnection _connection;
    private readonly IDatabaseReaderFactory<SubdivisionListItemsReader> _subdivisionListItemReaderFactory;
    private readonly IDatabaseReaderFactory<CountryListItemsReader> _countryListItemReaderFactory;
    public LocationService(
        IDbConnection connection,
        IDatabaseReaderFactory<SubdivisionListItemsReader> subdivisionListItemReaderFactory,
        IDatabaseReaderFactory<CountryListItemsReader> countryListItemReaderFactory
        )
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _subdivisionListItemReaderFactory = subdivisionListItemReaderFactory;
        _countryListItemReaderFactory = countryListItemReaderFactory;
    }
    public async IAsyncEnumerable<SubdivisionListItem> SubdivisionsOfCountry(int countryId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _subdivisionListItemReaderFactory.CreateAsync(_connection);
            await foreach (var subdivision in reader.ReadAsync(countryId)) {
                yield return subdivision;
            }
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }

        }

    }
    public async IAsyncEnumerable<CountryListItem> Countries()
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _countryListItemReaderFactory.CreateAsync(_connection);
            await foreach (var country in reader.ReadAsync(new CountryListItemsReader.Request())) {
                yield return country;
            }
        }
        finally {
            await _connection.CloseAsync();
        }
    }

    private const string GOOGLE_API_ADDRESS = "https://maps.googleapis.com/maps/api/geocode/json?";
    private const string GOOGLE_API_KEY = "AIzaSyDz40b_l25vytdmQGVnOFTqHVakmaZ6QCE";
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
        var url = $"{GOOGLE_API_ADDRESS}address={address}&key={GOOGLE_API_KEY}";
        var response = await client.GetAsync(url);
        var responseLocation = new Location {
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
            if (json is not null && json.results.Count > 0) {
                var result = json.results[0];
                var country = result.address_components.FirstOrDefault(x => x.types.Contains("country"));
                if (country is not null && country.short_name == "US") {
                    var streetNumber = result.address_components.FirstOrDefault(x => x.types.Contains("street_number"))?.long_name;
                    var streetName = result.address_components.FirstOrDefault(x => x.types.Contains("route"))?.long_name;
                    if (streetNumber is not null && streetName is not null) {
                        responseLocation.Street = $"{streetNumber} {streetName}";
                    }
                }
                var postalCode = result.address_components?.FirstOrDefault(x => x.types.Contains("postal_code"));
                if (postalCode is not null) {
                    responseLocation.PostalCode = postalCode.long_name;
                }
                var neighborhood = result.address_components?.FirstOrDefault(x => x.types.Contains("neighborhood"));
                var locality = result.address_components?.FirstOrDefault(x => x.types.Contains("locality"));
                if (locality is not null) {
                    if (neighborhood is not null) {
                        responseLocation.City = neighborhood.long_name;
                    }
                    else {
                        responseLocation.City = locality.long_name;
                    }
                }
                responseLocation.Latitude = new decimal(result.geometry.location.lat);
                responseLocation.Longitude = new decimal(result.geometry.location.lng);
                return responseLocation;
            }
        }
        return location;

    }
    public sealed class AddressComponent
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }

    public sealed class Bounds
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }

    public sealed class Geometry
    {
        public Bounds bounds { get; set; }
        public Location2 location { get; set; }
        public string location_type { get; set; }
        public Viewport viewport { get; set; }
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
        public List<AddressComponent> address_components { get; set; }
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public bool partial_match { get; set; }
        public string place_id { get; set; }
        public List<string> types { get; set; }
    }

    public sealed class Root
    {
        public List<Result> results { get; set; }
        public string status { get; set; }
    }

    public sealed class Southwest
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public sealed class Viewport
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }


}
