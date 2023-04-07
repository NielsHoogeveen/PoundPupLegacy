using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.EditModel;
using PoundPupLegacy.Services;
using System.Reflection;
using System.Text.RegularExpressions;
using Xunit;
using Location = PoundPupLegacy.EditModel.Location;
using LocationEditorDialog = PoundPupLegacy.Components.LocationEditorDialog;

namespace PoundPupLegacy.Test;
public class UnitTest1
{
    private const string URL_PATH = "united_states_senate_114th_congress";
    private const string REGEX = "united_states_(senate|house)_([0-9]+)(th|st|nd|rd)_congress";
    const string ConnectStringPostgresql = "Host=localhost;Username=niels;Password=niels;Database=ppl;Include Error Detail=True";

    [Fact]
    public async void AddUpdatersPrepare()
    {
        using var connection = new NpgsqlConnection(ConnectStringPostgresql);
        connection.Open();
        var creatorAssembly = Assembly.GetAssembly(typeof(Program));
        var types = creatorAssembly!.GetTypes().Where(x => x.IsAssignableTo(typeof(IDatabaseUpdaterFactory)) && !x.IsInterface && !x.IsAbstract && !x.IsGenericType);
        foreach (var type in types) {
            var i = Activator.CreateInstance(type);
            var m = type.GetMethod("CreateAsync", new Type[] { typeof(NpgsqlConnection) });
            var task = (Task)m!.Invoke(i, new object[] { connection });
            await task.ConfigureAwait(false);
            var result = task.GetType().GetProperty("Result");
            var reader = (IDatabaseUpdater)result.GetValue(task);
            Assert.True(reader.HasBeenPrepared);
            Assert.NotEqual(string.Empty, reader.Sql);
            await reader.DisposeAsync();
        }
        connection.Close();
    }

    public void RegularExpressionIsCapableOfExtractingTypeOfChamber()
    {
        Regex regex = new Regex(REGEX);
        var match = regex.Match(URL_PATH);
        Assert.True(match.Success);
        Assert.Equal(4, match.Groups.Count);
        Assert.Equal("senate", match.Groups[1].Value);
        Assert.Equal("114", match.Groups[2].Value);

    }
    //Please create unit test for LocationEditorDialog
    [Fact]
    public void LocationEditorDialogTest()
    {
        //Mock the LocationService
        var locationServiceMock = new Mock<ILocationService>();
        //Mock the Countries method of the LocationService
        locationServiceMock.Setup(x => x.Countries()).Returns(new List<CountryListItem> {
            new CountryListItem {
                           Id = 1,
                           Name = "United States"
                       }
        }.ToAsyncEnumerable());
        //Mock the Subdivisions method of the LocationService
        locationServiceMock.Setup(x => x.SubdivisionsOfCountry(1)).Returns(new List<SubdivisionListItem> {
            new SubdivisionListItem {
                                      Id = 1,
                                      Name = "New York"
                                  }
        }.ToAsyncEnumerable());
        //Mock the sitedataservice
        var siteDataServiceMock = new Mock<ISiteDataService>();
        //Create a new Location object and initialize it with values
        var location = new Location {
            LocationId = 1,
            LocatableId = 1,
            Street = "123 Main Street",
            Addition = "Apt 1",
            City = "New York",
            PostalCode = "10001",
            SubdivisionId = 1,
            SubdivisionName = "New York",
            CountryId = 1,
            CountryName = "United States",
            Latitude = 40.7128M,
            Longitude = 74.0060M,
            HasBeenDeleted = false,
            Subdivisions = new List<SubdivisionListItem> {
                new SubdivisionListItem {
                                   Id = 1,
                                   Name = "New York"
                               }
            }
        };



        //Create a new TestContext, inject the location service mock and the site data service mock
        using var ctx = new TestContext();
        ctx.Services.AddSingleton(locationServiceMock.Object);
        ctx.Services.AddSingleton(siteDataServiceMock.Object);

        //Initialize the locationEditorDialog with Bunit and set location to the location object
        var cut = ctx.RenderComponent<LocationEditorDialog>(parameters => parameters.Add(p => p.Location, location));

        //Assert that the rendering of locationEditorDialog contains a text box for Street with a label
        cut.Markup.Contains(@"<label for=""location-street"">Street</label>
                            <input type=""text"" id=""location-street"" value=""123 Main Street"" />");




    }









}