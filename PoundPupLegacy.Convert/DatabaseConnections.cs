using MySqlConnector;
using Npgsql;

namespace PoundPupLegacy.Convert;

public interface IDatabaseConnections
{
    NpgsqlConnection PostgressConnection { get; }
    MySqlConnection MysqlConnectionPPL { get; }
    MySqlConnection MysqlConnectionCPCT { get; }
}

internal class DatabaseConnections : IDatabaseConnections
{
    public required NpgsqlConnection PostgressConnection { get; init; }
    public required MySqlConnection MysqlConnectionPPL { get; init; }
    public required MySqlConnection MysqlConnectionCPCT { get; init; }
}
