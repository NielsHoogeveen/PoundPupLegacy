using Npgsql;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal abstract class DatabaseService
{
    public NpgsqlConnection connection;
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
            var tx = await connection.BeginTransactionAsync();
            try {
                var result = await func(connection);
                await tx.CommitAsync();
                return result;
            }
            catch (Exception) {
                await tx.RollbackAsync();
                throw;
            }
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
