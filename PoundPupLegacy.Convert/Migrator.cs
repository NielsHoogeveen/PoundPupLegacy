using PoundPupLegacy.Model;
using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db.Readers;
using System.Diagnostics;

namespace PoundPupLegacy.Convert;

internal abstract class Migrator
{
    protected MySqlConnection _mysqlConnection;
    protected NpgsqlConnection _postgresConnection;
    protected NodeIdByUrlIdReader _nodeIdReader;
    protected TermReaderByNameableId _termReaderByNameableId;
    protected SubdivisionIdReaderByName _subdivisionIdReader;
    protected SubdivisionIdReaderByIso3166Code _subdivisionIdReaderByIso3166Code;
    private Stopwatch stopwatch = new Stopwatch();

    protected Migrator(MySqlToPostgresConverter mySqlToPostgresConverter)
    {
        _mysqlConnection = mySqlToPostgresConverter.MysqlConnection;
        _postgresConnection = mySqlToPostgresConverter.PostgresConnection;
        _nodeIdReader = mySqlToPostgresConverter.NodeIdReader;
        _termReaderByNameableId = mySqlToPostgresConverter.TermByNameableIdReader;
        _subdivisionIdReader = mySqlToPostgresConverter.SubdivisionIdReader;
        _subdivisionIdReaderByIso3166Code = mySqlToPostgresConverter.SubdivisionIdReaderByIso3166Code;
    }

    public async Task Migrate()
    {
        await using var tx = await _postgresConnection.BeginTransactionAsync();
        try
        {
            Console.Write($"Migrating {Name}");
            stopwatch.Start();
            await MigrateImpl();
            Console.WriteLine($" took {stopwatch.ElapsedMilliseconds} ms");
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }

    }

    protected abstract string Name { get; }
    protected abstract Task MigrateImpl();

    protected static string LastDayOfMonth(string month, int year)
    {
        return month switch
        {
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
        if (str is null)
        {
            return null;
        }
        if (DateTime.TryParse(str, out var dt))
        {
            return new DateTimeRange(dt, dt);
        }
        else
        {
            if (str.Substring(5, 2) == "00")
            {
                var year = str.Substring(0, 4);
                var dateFrom = DateTime.Parse($"{year}-01-01");
                var dateTo = DateTime.Parse($"{year}-12-31");
                return new DateTimeRange(dateFrom, dateTo);
            }
            if (str.Substring(8, 2) == "00")
            {
                var year = str.Substring(0, 4);
                var month = str.Substring(5, 2);
                var dateFrom = DateTime.Parse($"{year}-{month}-01");
                var dateTo = DateTime.Parse($"{year}-{month}-{LastDayOfMonth(month, int.Parse(year))}");
                return new DateTimeRange(dateFrom, dateTo);
            }
            throw new NotSupportedException($"Cannot convert {str} to a date time range");

        }
    }

}
