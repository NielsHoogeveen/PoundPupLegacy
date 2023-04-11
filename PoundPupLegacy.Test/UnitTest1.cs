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

    private const string REGEX2 = "(?<year>[0-9]{1,4})(-(?<month>[0-9]{1,2})(-(?<day>[0-9]{1,2}))?)?";

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
    [Fact]
    public void FuzzyDateParsesSingleYearCorrectly()
    {
        var dateString = "2023";
        var success = FuzzyDate.TryParse(dateString, out var result);
        Assert.True(success);
        Assert.NotNull(result);
        Assert.Equal(2023, result.Year);
        Assert.Null(result.Month);
        Assert.Null(result.Day);
        Assert.Equal(dateString, result.ToString());
    }

    [Fact]
    public void FuzzyDateParsesYearMonthCorrectly()
    {
        var dateString = "2023-1";
        var success = FuzzyDate.TryParse(dateString, out var result);
        Assert.True(success);
        Assert.NotNull(result);
        Assert.Equal(2023, result.Year);
        Assert.Equal(1, result.Month);
        Assert.Null(result.Day);
        Assert.Equal(dateString, result.ToString());
    }

    [Fact]
    public void FuzzyDateParsesYearMonthDayCorrectly()
    {
        var dateString = "2023-1-1";
        var success = FuzzyDate.TryParse(dateString, out var result);
        Assert.True(success);
        Assert.NotNull(result);
        Assert.Equal(2023, result.Year);
        Assert.Equal(1, result.Month);
        Assert.Equal(1, result.Day);
        Assert.Equal(dateString, result.ToString());
    }

    [Fact]
    public void FuzzyDateFailsWhenMonthIsGreaterThan12()
    {
        var dateString = "2023-13";
        var success = FuzzyDate.TryParse(dateString, out var result);
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void FuzzyDateFailsWhenYearIsGreaterThan9999()
    {
        var dateString = "10000";
        var success = FuzzyDate.TryParse(dateString, out var result);
        Assert.False(success);
        Assert.Null(result);
    }
    [Fact]
    public void FuzzyDateFailsWhenDayIsNotInMonth()
    {
        var dateString = "2023-2-29";
        var success = FuzzyDate.TryParse(dateString, out var result);
        Assert.False(success);
        Assert.Null(result);
    }
    [Fact]
    public void FuzzyDateCreatesCorrectDateRangeForExactDate()
    {
        var dateString = "2023-2-28";
        var success = FuzzyDate.TryParse(dateString, out var result);
        Assert.True(success);
        Assert.NotNull(result);
        var dateResult = result.ToDateTimeRange();
        Assert.NotNull(dateResult);
        var date = DateTime.Parse(dateString);
        var dateRange = new DateTimeRange(date, date.AddDays(1).AddMilliseconds(-1));
        Assert.Equal(dateRange, dateResult);

    }
    [Theory]
    [InlineData("2023-2", 2023, 2)]
    [InlineData("2023-12", 2023, 12)]
    public void FuzzyDateCreatesCorrectDateRangeForMonthOnlyDate(string dateString, int year, int month)
    {
        var success = FuzzyDate.TryParse(dateString, out var result);
        Assert.True(success);
        Assert.NotNull(result);
        var dateRange = result.ToDateTimeRange();
        Assert.NotNull(dateRange);
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddMilliseconds(-1);
        Assert.Equal(startDate, dateRange.Start);
        Assert.Equal(endDate, dateRange.End);

    }
    [Fact]
    public void FuzzyDateCreatesCorrectDateRangeForYearOnlyDate()
    {
        var dateString = "2023";
        var success = FuzzyDate.TryParse(dateString, out var result);
        Assert.True(success);
        Assert.NotNull(result);
        var dateRange = result.ToDateTimeRange();
        Assert.NotNull(dateRange);
        var startDate = new DateTime(2023, 1, 1);
        var endDate = startDate.AddYears(1).AddMilliseconds(-1);
        Assert.Equal(startDate, dateRange.Start);
        Assert.Equal(endDate, dateRange.End);

    }

}