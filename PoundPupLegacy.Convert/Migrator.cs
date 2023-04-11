using HtmlAgilityPack;
using MySqlConnector;
using Npgsql;
using System.Diagnostics;
using System.Text;

namespace PoundPupLegacy.Convert;
internal abstract class MigratorPPL : Migrator
{
    public MigratorPPL(
        IDatabaseConnections databaseConnections
    ) : base(databaseConnections.PostgressConnection, databaseConnections.MysqlConnectionPPL)
    {

    }
}
internal abstract class MigratorCPCT : Migrator
{
    protected readonly IDatabaseReaderFactory<NodeIdReaderByUrlId> _nodeIdReaderFactory;
    protected readonly IDatabaseReaderFactory<TenantNodeReaderByUrlId> _tenantNodeReaderByUrlIdFactory;
    public MigratorCPCT(
        IDatabaseConnections databaseConnections,
        IDatabaseReaderFactory<NodeIdReaderByUrlId> nodeIdReaderFactory,
        IDatabaseReaderFactory<TenantNodeReaderByUrlId> tenantNodeReaderByUrlIdFactory
    ) : base(databaseConnections.PostgressConnection, databaseConnections.MysqlConnectionCPCT)
    {
        _nodeIdReaderFactory = nodeIdReaderFactory;
        _tenantNodeReaderByUrlIdFactory = tenantNodeReaderByUrlIdFactory;
    }
    protected async Task<(int, int)> GetNodeId(int urlId, NodeIdReaderByUrlId nodeIdReader, TenantNodeReaderByUrlId tenantNodeReader)
    {

        var (id, ownerId) = GetUrlIdAndTenant(urlId);
        if (urlId >= 33162 && ownerId == Constants.CPCT) {
            var nodeId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                TenantId = Constants.CPCT,
                UrlId = id
            });
            var node = await tenantNodeReader.ReadAsync(new TenantNodeReaderByUrlId.Request {
                TenantId = Constants.PPL,
                UrlId = id
            });
            if (node is not null) {
                return (nodeId, node.PublicationStatusId);
            }
            else {
                return (nodeId, 0);
            }

        }
        return (await nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
            TenantId = Constants.PPL,
            UrlId = id
        }), 1);
    }
    protected (int, int) GetUrlIdAndTenant(int urlId)
    {
        return urlId switch {
            10793 => (6138, Constants.PPL),
            34880 => (35760, Constants.PPL),
            35725 => (37560, Constants.PPL),
            33644 => (38279, Constants.PPL),
            36680 => (39755, Constants.PPL),
            34973 => (40525, Constants.PPL),
            45964 => (41694, Constants.PPL),
            35124 => (45974, Constants.PPL),
            34126 => (48192, Constants.PPL),
            34082 => (52502, Constants.PPL),
            34015 => (53756, Constants.PPL),
            49152 => (60032, Constants.PPL),
            35138 => (73657, Constants.PPL),
            35146 => (73661, Constants.PPL),
            33255 => (33987, Constants.PPL),
            44216 => (29148, Constants.PPL),
            49224 => (35567, Constants.PPL),
            35233 => (44675, Constants.PPL),
            33454 => (35190, Constants.PPL),
            39431 => (55108, Constants.PPL),
            48210 => (34899, Constants.CPCT),
            48330 => (48545, Constants.CPCT),
            47699 => (48846, Constants.CPCT),
            > 33162 => (urlId, Constants.CPCT),
            _ => (urlId, Constants.PPL),
        };
    }

}

internal abstract class Migrator
{

    protected readonly NpgsqlConnection _postgresConnection;
    protected readonly MySqlConnection _mySqlConnection;
    private readonly Stopwatch stopwatch = new Stopwatch();

    protected Migrator(
        NpgsqlConnection postgresConnection,
        MySqlConnection mySqlConnection
        )
    {
        _postgresConnection = postgresConnection;
        _mySqlConnection = mySqlConnection;
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
            var dateFrom = dt.Date;
            var dateTo = dateFrom.AddDays(1).AddMilliseconds(-1);
            return new DateTimeRange(dateFrom, dateTo);
        }
        else {
            if (str.Substring(5, 2) == "00") {
                var year = int.Parse(str.Substring(0, 4));
                var dateFrom = new DateTime(year, 1, 1);
                var dateTo = dateFrom.AddYears(1).AddMilliseconds(-1);
                return new DateTimeRange(dateFrom, dateTo);
            }
            if (str.Substring(8, 2) == "00") {
                var year = int.Parse(str.Substring(0, 4));
                var month = int.Parse(str.Substring(5, 2));
                var dateFrom = new DateTime(year, month, 1);
                var dateTo = dateFrom.AddMonths(1).AddMilliseconds(-1);
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
