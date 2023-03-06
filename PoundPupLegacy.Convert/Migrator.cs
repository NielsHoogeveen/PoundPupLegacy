using HtmlAgilityPack;
using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db.Readers;
using PoundPupLegacy.Model;
using System.Diagnostics;
using System.Text;

namespace PoundPupLegacy.Convert;

internal abstract class Migrator
{

    protected abstract MySqlConnection MysqlConnection { get; }
    protected readonly NpgsqlConnection _postgresConnection;
    protected readonly NodeIdReaderByUrlId _nodeIdReader;
    protected readonly TermReaderByNameableId _termReaderByNameableId;
    protected readonly SubdivisionIdReaderByName _subdivisionIdReader;
    protected readonly SubdivisionIdReaderByIso3166Code _subdivisionIdReaderByIso3166Code;
    protected readonly CreateNodeActionIdReaderByNodeTypeId _createNodeActionIdReaderByNodeTypeId;
    protected readonly DeleteNodeActionIdReaderByNodeTypeId _deleteNodeActionIdReaderByNodeTypeId;
    protected readonly EditNodeActionIdReaderByNodeTypeId _editNodeActionIdReaderByNodeTypeId;
    protected readonly EditOwnNodeActionIdReaderByNodeTypeId _editOwnNodeActionIdReaderByNodeTypeId;
    protected readonly ActionIdReaderByPath _actionReaderByPath;
    protected readonly TenantNodeIdReaderByUrlId _tenantNodeIdByUrlIdReader;
    protected readonly TenantNodeReaderByUrlId _tenantNodeByUrlIdReader;
    protected readonly FileIdReaderByTenantFileId _fileIdReaderByTenantFileId;


    private readonly Stopwatch stopwatch = new Stopwatch();

    protected Migrator(MySqlToPostgresConverter mySqlToPostgresConverter)
    {
        _postgresConnection = mySqlToPostgresConverter.PostgresConnection;
        _nodeIdReader = mySqlToPostgresConverter.NodeIdReader;
        _termReaderByNameableId = mySqlToPostgresConverter.TermByNameableIdReader;
        _subdivisionIdReader = mySqlToPostgresConverter.SubdivisionIdReader;
        _subdivisionIdReaderByIso3166Code = mySqlToPostgresConverter.SubdivisionIdReaderByIso3166Code;
        _createNodeActionIdReaderByNodeTypeId = mySqlToPostgresConverter.CreateNodeActionIdReaderByNodeTypeId;
        _deleteNodeActionIdReaderByNodeTypeId = mySqlToPostgresConverter.DeleteNodeActionIdReaderByNodeTypeId;
        _editNodeActionIdReaderByNodeTypeId = mySqlToPostgresConverter.EditNodeActionIdReaderByNodeTypeId;
        _editOwnNodeActionIdReaderByNodeTypeId = mySqlToPostgresConverter.EditOwnNodeActionIdReaderByNodeTypeId;
        _actionReaderByPath = mySqlToPostgresConverter.ActionIdReaderByPath;
        _tenantNodeIdByUrlIdReader = mySqlToPostgresConverter.TenantNodeIdByUrlIdReader;
        _tenantNodeByUrlIdReader = mySqlToPostgresConverter.TenantNodeByUrlIdReader;
        _fileIdReaderByTenantFileId = mySqlToPostgresConverter.FileIdReaderByTenantFileId;
    }

    public async Task Migrate()
    {
        await using var tx = await _postgresConnection.BeginTransactionAsync();
        try {
            Console.Write($"Migrating {Name}");
            stopwatch.Start();
            await MigrateImpl();
            Console.WriteLine($" took {stopwatch.ElapsedMilliseconds} ms");
            await tx.CommitAsync();
        }
        catch (Exception) {
            await tx.RollbackAsync();
            throw;
        }

    }

    protected abstract string Name { get; }
    protected abstract Task MigrateImpl();

    protected static string LastDayOfMonth(string month, int year)
    {
        return month switch {
            "01" => "31",
            "02" => year % 400 == 0 ? "29" : year % 100 == 0 ? "28" : year % 4 == 0 ? "29" : "28",
            "03" => "31",
            "04" => "30",
            "05" => "31",
            "06" => "30",
            "07" => "31",
            "08" => "31",
            "09" => "30",
            "10" => "31",
            "11" => "30",
            "12" => "31",
            _ => throw new Exception($"{month} is an unknown month")
        };
    }
    protected static DateTimeRange? StringToDateTimeRange(string? str)
    {
        if (str is null) {
            return null;
        }
        if (DateTime.TryParse(str, out var dt)) {
            return new DateTimeRange(dt, dt);
        }
        else {
            if (str.Substring(5, 2) == "00") {
                var year = str.Substring(0, 4);
                var dateFrom = DateTime.Parse($"{year}-01-01");
                var dateTo = DateTime.Parse($"{year}-12-31");
                return new DateTimeRange(dateFrom, dateTo);
            }
            if (str.Substring(8, 2) == "00") {
                var year = str.Substring(0, 4);
                var month = str.Substring(5, 2);
                var dateFrom = DateTime.Parse($"{year}-{month}-01");
                var dateTo = DateTime.Parse($"{year}-{month}-{LastDayOfMonth(month, int.Parse(year))}");
                return new DateTimeRange(dateFrom, dateTo);
            }
            throw new NotSupportedException($"Cannot convert {str} to a date time range");

        }
    }
    public static string TextToTeaser(string text)
    {
        var doc = Convert(text);
        var res = doc.DocumentNode.ChildNodes.Take(5).Aggregate("", (a, b) => a + b.OuterHtml.Replace(@"href=""http://poundpuplegacy.org", @"href="""));
        return res;
    }
    protected string TextToHtml(string text)
    {
        return Convert(text).DocumentNode.InnerHtml;
    }

    private static HtmlDocument Convert(string text)
    {

        if (!text.Contains("</") && !text.Contains("/>")) {
            text = FormatText(text);
        }
        else {
            text = text.Replace("\r", "").Replace("\n", "");
        }
        var doc = new HtmlDocument();
        doc.LoadHtml(text);
        var elements = MakeParagraphs(doc).ToList();
        doc.DocumentNode.RemoveAllChildren();
        foreach (var element in elements) {
            if (!(element.Name == "p" && string.IsNullOrEmpty(element.InnerHtml.Trim()))) {
                doc.DocumentNode.ChildNodes.Add(element);
            }
        }
        return doc;
    }

    private static string FormatText(string text)
    {
        var stringBuilder = new StringBuilder();
        var i = 0;
        while (i < text.Length) {
            var c = text[i];
            if (i == 0) {
                stringBuilder.Append("<p>");
            }
            if (i == text.Length - 1) {
                stringBuilder.Append(text[i]);
                stringBuilder.Append("</p>");
            }

            if (c == 'h' && (text[i..].StartsWith("http://") || text[i..].StartsWith("https://"))) {
                var endPos = text[i..].IndexOfAny(new char[] { ' ', '\t', '\n', '\r' });
                var url = endPos == -1 ?
                    text[i..text.Length] :
                    text[i..(endPos + i)];
                stringBuilder.Append($@"<a href=""{url}"">{url}</a>");
                i += endPos == -1 ? text.Length : endPos;
                continue;
            }
            else if (text[i] == '\n') {
                stringBuilder.Append("</p><p>");
            }
            else if (text[i] == '\r') {
                stringBuilder.Append("</p><p>");
                i++;
            }
            else {
                stringBuilder.Append(c);
            }
            i++;
        }
        return stringBuilder.ToString();
    }

    private static IEnumerable<HtmlNode> MakeParagraphs(HtmlDocument doc)
    {
        var element = doc.CreateElement("p");
        foreach (var elem in doc.DocumentNode.ChildNodes) {
            if (elem is HtmlTextNode textNode) {
                element.ChildNodes.Add(textNode.Clone());
            }
            else if (elem.Name == "a") {
                element.ChildNodes.Add(elem.Clone());
            }
            else if (elem.Name == "br") {
                if (element.ChildNodes.Count > 0) {
                    yield return element.Clone();
                    element = doc.CreateElement("p");
                }
            }
            else {
                if (element.ChildNodes.Count > 0) {
                    yield return element.Clone();
                    element = doc.CreateElement("p");
                }
                yield return elem.Clone();
            }
        }
        if (element.ChildNodes.Count > 0) {
            yield return element;
        }
    }


}
