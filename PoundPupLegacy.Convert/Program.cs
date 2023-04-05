using Microsoft.Extensions.Hosting;
using MySqlConnector;
using Npgsql;

namespace PoundPupLegacy.Convert;

internal partial class Program
{
    internal const string ConnectionStringMariaDbPPL = "server=localhost;userid=root;Password=root;database=ppl";
    internal const string ConnectionStringMariaDbCPCT = "server=localhost;userid=root;Password=root;database=cpct";
    internal const string ConnectStringPostgresql = "Host=localhost;Username=niels;Password=niels;Database=ppl;Include Error Detail=True";

    static async Task Main(string[] args)
    {
        var mysqlConnectionPPL = new MySqlConnection(ConnectionStringMariaDbPPL);
        var mysqlConnectionCPCT = new MySqlConnection(ConnectionStringMariaDbCPCT);
        var postgresConnection = new NpgsqlConnection(ConnectStringPostgresql);
        await mysqlConnectionPPL.OpenAsync();
        await mysqlConnectionCPCT.OpenAsync();
        await postgresConnection.OpenAsync();

        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services => {
                services.AddCreateModelAccessors();
                var connections = new DatabaseConnections {
                    MysqlConnectionCPCT = mysqlConnectionCPCT,
                    MysqlConnectionPPL = mysqlConnectionPPL,
                    PostgressConnection = postgresConnection
                };
                services.AddSingleton<IDatabaseConnections>((sp) => connections);
                services.AddMigrators();
                services.AddTransient<MySqlToPostgresConverter>();
            })
        .Build();
        var converter = host.Services.GetRequiredService<MySqlToPostgresConverter>();
        await converter.Convert();
        await host.RunAsync();
    }

}
