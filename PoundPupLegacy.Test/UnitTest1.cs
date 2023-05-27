using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PoundPupLegacy.Common;
using PoundPupLegacy.EditModel;
using PoundPupLegacy.EditModel.UI.Services;
using PoundPupLegacy.Services;
using System.Text.RegularExpressions;
using Xunit;
using Location = PoundPupLegacy.EditModel.Location;
using LocationEditorDialog = PoundPupLegacy.EditModel.UI.Components.LocationEditorDialog;

namespace PoundPupLegacy.Test;
public class UnitTest1
{
    private const string URL_PATH = "united_states_senate_114th_congress";
    private const string REGEX = "united_states_(senate|house)_([0-9]+)(th|st|nd|rd)_congress";


    private const string REGEX2 = "(?<year>[0-9]{1,4})(-(?<month>[0-9]{1,2})(-(?<day>[0-9]{1,2}))?)?";


    [Fact]
    public void RegularExpressionIsCapableOfExtractingTypeOfChamber()
    {
        Regex regex = new Regex(REGEX);
        var match = regex.Match(URL_PATH);
        Assert.True(match.Success);
        Assert.Equal(4, match.Groups.Count);
        Assert.Equal("senate", match.Groups[1].Value);
        Assert.Equal("114", match.Groups[2].Value);

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