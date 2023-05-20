using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;

namespace PoundPupLegacy.Common;

public abstract class DatabaseService
{
    public NpgsqlConnection connection;
    protected ILogger logger;
    protected DatabaseService(IDbConnection connection, ILogger logger)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        this.connection = (NpgsqlConnection)connection;
        this.logger = logger;
    }

    private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
    protected async Task<T> WithSequencedConnection<T>(Func<NpgsqlConnection, Task<T>> func)
    {
        await semaphore.WaitAsync();
        try {
            await connection.OpenAsync();
            return await func(connection);
        }
        catch (Exception e) {
            logger.LogError(e, "Error while executing database query");
            throw;
        }
        finally {
            await connection.CloseAsync();
            semaphore.Release();
        }
    }

    protected async Task<T> WithTransactedConnection<T>(Func<NpgsqlConnection, Task<T>> func)
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
        catch (Exception e) {
            logger.LogError(e, "Error while executing database query");
            throw;
        }
        finally {
            await connection.CloseAsync();
        }
    }
    protected async Task<T> WithConnection<T>(Func<NpgsqlConnection, Task<T>> func)
    {
        try {
            await connection.OpenAsync();
            return await func(connection);
        }
        catch (Exception e) {
            logger.LogError(e, "Error while executing database query");
            throw;
        }
        finally {
            await connection.CloseAsync();
        }
    }
}
