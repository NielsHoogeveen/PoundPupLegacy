using Npgsql;
using PoundPupLegacy.EditModel;
using System.Data;
using System.Text;

namespace PoundPupLegacy.Services.Implementation;

public class LocationService : ILocationService
{
    private readonly NpgsqlConnection _connection;
    public LocationService(NpgsqlConnection connection)
    {
        _connection = connection;
    }
    public async IAsyncEnumerable<SubdivisionListItem> SubdivisionsOfCountry(int countryId)
    {
        try {
            await _connection.OpenAsync();
            using (var command = _connection.CreateCommand()) {
                var sql = $"""
            select
                s.id,
                s.name
                from subdivision s
                join bottom_level_subdivision bls on bls.id = s.id
                where s.country_id = @country_id
                order by s.name
            """;
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 300;
                command.CommandText = sql;
                command.Parameters.Add("country_id", NpgsqlTypes.NpgsqlDbType.Integer);
                await command.PrepareAsync();
                command.Parameters["country_id"].Value = countryId;
                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync()) {
                    var id = reader.GetInt32(0);
                    var name = reader.GetString(1);
                    yield return new SubdivisionListItem {
                        Id = id,
                        Name = name
                    };
                }
            }
        }
        finally {
            await _connection.CloseAsync();
        }

    }
    public async IAsyncEnumerable<CountryListItem> Countries()
    {
        try {
            await _connection.OpenAsync();
            using var readCommand = _connection.CreateCommand();
            var sql = $"""
                select
                    c.id,
                    t.name
                    from country c
                    join term t on t.nameable_id = c.id
                    join tenant_node tn on tn.node_id = t.vocabulary_id
                    where tn.tenant_id = 1 and tn.url_id = 4126
            
                """;
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;
            await readCommand.PrepareAsync();
            await using var reader = await readCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync()) {
                var id = reader.GetInt32(0);
                var name = reader.GetString(1);
                yield return new CountryListItem {
                    Id = id,
                    Name = name
                };
            }
            await reader.CloseAsync();
        }
        finally {
            await _connection.CloseAsync();
        }
    }

    private const string GOOGLE_API_ADDRESS = "https://maps.googleapis.com/maps/api/geocode/json?";
    private const string GOOGLE_API_KEY = "AIzaSyDz40b_l25vytdmQGVnOFTqHVakmaZ6QCE";
    public async Task<Location> ValidateLocationAsync(Location location)
    {
        HttpClient client = new HttpClient();
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
    public class AddressComponent
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }

    public class Bounds
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }

    public class Geometry
    {
        public Bounds bounds { get; set; }
        public Location2 location { get; set; }
        public string location_type { get; set; }
        public Viewport viewport { get; set; }
    }

    public class Location2
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Northeast
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Result
    {
        public List<AddressComponent> address_components { get; set; }
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public bool partial_match { get; set; }
        public string place_id { get; set; }
        public List<string> types { get; set; }
    }

    public class Root
    {
        public List<Result> results { get; set; }
        public string status { get; set; }
    }

    public class Southwest
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Viewport
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }


}
