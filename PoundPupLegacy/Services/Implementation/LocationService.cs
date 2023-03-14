using PoundPupLegacy.EditModel;
using System.Text;

namespace PoundPupLegacy.Services.Implementation;


public class LocationService: ILocationService
{
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



    private const string GOOGLE_API_ADDRESS = "https://maps.googleapis.com/maps/api/geocode/json?";
    private const string GOOGLE_API_KEY = "AIzaSyDz40b_l25vytdmQGVnOFTqHVakmaZ6QCE";
    public async Task ProcessLocationAsync(Location location)
    {
        HttpClient client = new HttpClient();
        var addressBuilder = new StringBuilder();
        if(location.Street is not null) {
            addressBuilder.Append(location.Street.Replace(" ", "%20"));
            addressBuilder.Append("%20");
        }
        if(location.City is not null) {
            addressBuilder.Append(location.City.Replace(" ", "%20"));
            addressBuilder.Append("%20");
        }
        if(location.PostalCode is not null) {
            addressBuilder.Append(location.PostalCode.Replace(" ", "%20"));
            addressBuilder.Append("%20");
        }
        if(location.SubdivisionName is not null) {
            addressBuilder.Append(location.SubdivisionName.Replace(" ", "%20"));
            addressBuilder.Append("%20");
        }
        addressBuilder.Append(location.CountryName.Replace(" ", "$20"));

        var address = addressBuilder.ToString();
        var url = $"{GOOGLE_API_ADDRESS}address={address}&key={GOOGLE_API_KEY}";
        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode) {
            var content = response.Content;
            var json = await content.ReadFromJsonAsync<Root>();
            if(json is not null && json.results.Count > 0) {
                var result = json.results[0];
                location.Lattitude = new decimal(result.geometry.location.lat);
                location.Longitude = new decimal(result.geometry.location.lng);
            }
        }
        
    }
}
