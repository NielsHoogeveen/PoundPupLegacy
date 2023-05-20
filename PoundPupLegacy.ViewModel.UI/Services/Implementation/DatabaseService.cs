using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;

internal abstract class DatabaseService
{
    private NpgsqlConnection connection;
    protected ILogger logger;
    public DatabaseService(IDbConnection connection, ILogger logger)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        this.connection = (NpgsqlConnection)connection;
        this.logger = logger;
    }

    protected async Task<T> WithConnection<T>(Func<NpgsqlConnection, Task<T>> func)
    {
        try {
            await connection.OpenAsync();
            return await func(connection);
        }
        catch(Exception e) {
            logger.LogError(e, "Error while executing database query");
            throw;
        }
        finally {
            await connection.CloseAsync();
        }
    }
}
