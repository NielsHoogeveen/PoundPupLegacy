using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace PoundPupLegacy.Common;
public class FuzzyDateJsonConverter : JsonConverter<FuzzyDate>
{
    private FuzzyDateJsonConverter() { }
    public static FuzzyDateJsonConverter Default { get; } = new FuzzyDateJsonConverter();
    public override FuzzyDate Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) => FuzzyDate.ParseJson(reader.GetString()!);

    public override void Write(
        Utf8JsonWriter writer,
        FuzzyDate fuzzyDate,
        JsonSerializerOptions options) =>
            writer.WriteStringValue(fuzzyDate.ToJson());
}
public partial record FuzzyDate: IComparable<FuzzyDate>
{
    public FuzzyDate(int year, int? month, int? day)
    {
        if (year < 0 || year > 9999) {
            throw new ArgumentOutOfRangeException(nameof(year), year, "The year must be between 0 and 9999");
        }
        if (month < 1 || month > 12) {
            throw new ArgumentOutOfRangeException(nameof(month), month, "The month must be between 1 and 12");
        }
        if (day < 1 || day > 31) {
            throw new ArgumentOutOfRangeException(nameof(day), day, "The day must be between 1 and 31");
        }
        if (month.HasValue && day.HasValue && day > DateTime.DaysInMonth(year, month.Value)) {
            throw new ArgumentOutOfRangeException(nameof(day), day, $"The day must be between 1 and {DateTime.DaysInMonth(year, month.Value)}");
        }
        if (!month.HasValue && day.HasValue) {
            throw new ArgumentException("The day cannot be specified without a month");
        }
        Year = year;
        Month = month;
        Day = day;
    }
    public int Year { get; }
    public int? Month { get; }
    public int? Day { get; }

    [GeneratedRegex("^(?<year>[0-9]{1,5})(-(?<month>[0-9]{1,2})(-(?<day>[0-9]{1,2}))?)?", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex FuzzyDateRegex();

    const string FuzzyDateJsonRegexString = """
        ^\["(?<datefrom>[0-9]{4}-[0-9]{2}-[0-9]{2}) 00:00:00","(?<dateto>[0-9]{4}-[0-9]{2}-[0-9]{2}) 23:59:59.999"\)$
        """;

    [GeneratedRegex(FuzzyDateJsonRegexString)]
    public static partial Regex FuzzyDateJsonRegex();

    public static FuzzyDate FromDateTime(DateTime input)
    {
        return new FuzzyDate(input.Year, input.Month, input.Day);
    }
    public static bool TryFromDateTimeRange(DateTimeRange input, out FuzzyDate? result)
    {
        if (input.Start is null || input.End is null) {
            result = null;
            return false;
        }
        if (input.Start.Value.AddDays(1).AddMilliseconds(-1) == input.End.Value) {
            result = FromDateTime(input.Start.Value);
            return true;
        }
        if (input.Start.Value.AddMonths(1).AddMilliseconds(-1) == input.End) {
            result = new FuzzyDate(input.Start.Value.Year, input.Start.Value.Month, null);
            return true;
        }
        if (input.Start.Value.AddYears(1).AddMilliseconds(-1) == input.End) {
            result = new FuzzyDate(input.Start.Value.Year, null, null);
            return true;
        }
        result = null;
        return false;
    }

    public static FuzzyDate ParseJson(string input)
    {
        var regex = FuzzyDateJsonRegex();
        var m = regex.Match(input);
        if(m.Success) {
            var dateFromString = m.Groups["datefrom"];
            var dateToString = m.Groups["dateto"];
            var dateFrom = DateTime.Parse(dateFromString.ToString());
            var dateTo = DateTime.Parse(dateToString.ToString()).AddDays(1).AddMilliseconds(-1);
            if(TryFromDateTimeRange(new DateTimeRange(dateFrom, dateTo), out var fuzzyDate)) {
                return fuzzyDate!;
            }
            throw new FormatException($"Fuzzy date has incorrect format {input}");
        }
        throw new FormatException($"Fuzzy date has incorrect format {input}");
    }
    public string ToJson()
    {
        throw new Exception("This method is not implemented");
    }

    public static bool TryParse(string? input, out FuzzyDate? result)
    {
        if (input is null) {
            result = null;
            return false;
        }
        var match = FuzzyDateRegex().Match(input);
        if (!match.Success) {
            result = null;
            return false;
        }
        try {
            var year = int.Parse(match.Groups["year"].Value);
            var month = match.Groups["month"].Success ? int.Parse(match.Groups["month"].Value) : (int?)null;
            var day = match.Groups["day"].Success ? int.Parse(match.Groups["day"].Value) : (int?)null;
            result = new FuzzyDate(year, month, day);
            return true;
        }
        catch {
            result = null;
            return false;
        }
    }

    public override string ToString()
    {
        if (Month.HasValue && Day.HasValue) {
            return $"{Year}-{Month}-{Day}";
        }
        else if (Month.HasValue && !Day.HasValue) {
            return $"{Year}-{Month}";
        }
        else {
            return $"{Year}";
        }
    }

    public DateTimeRange ToDateTimeRange()
    {
        if (Month.HasValue && Day.HasValue) {
            var startDate = new DateTime(Year, Month.Value, Day.Value).Date;
            var endDate = startDate.AddDays(1).AddMilliseconds(-1);
            return new DateTimeRange(startDate, endDate);
        }
        if (Month.HasValue) {
            var startDate = new DateTime(Year, Month.Value, 1);
            var endDate = startDate.AddMonths(1).AddMilliseconds(-1);
            return new DateTimeRange(startDate, endDate);
        }
        else {
            var startDate = new DateTime(Year, 1, 1);
            var endDate = startDate.AddYears(1).AddMilliseconds(-1);
            return new DateTimeRange(startDate, endDate);
        }
    }
    public string ToDisplayString()
    {
        if (Month is null)
            return $"{Year}";
        if (Day is null)
            return $" {Year} {CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Month.Value)}";
        return $"{Year} {CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Month.Value)} {Day}";

    }

    public int CompareTo(FuzzyDate? other)
    {
        if(other == null) {
            return -1 ;
        }
        if(other.Year < this.Year) {
            return 1;
        }
        if(other.Year > this.Year) {
            return -1;
        }
        if (!other.Month.HasValue && !this.Month.HasValue) {
            return 0;
        }
        if (other.Month.HasValue && !this.Month.HasValue) {
            return -1;
        }
        if (!other.Month.HasValue && this.Month.HasValue) {
            return 1;
        }
        if (!other.Day.HasValue && !this.Day.HasValue) {
            return 0;
        }
        if (other.Day.HasValue && !this.Day.HasValue) {
            return -1;
        }
        if (!other.Day.HasValue && this.Day.HasValue) {
            return 1;
        }
        return 0;
        throw new NotImplementedException();
    }
}
