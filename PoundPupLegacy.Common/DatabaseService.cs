using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;

namespace PoundPupLegacy.Common;

public abstract class DatabaseService
{
    public NpgsqlConnection _connection;
    protected ILogger _logger;
    protected DatabaseService(IDbConnection connection, ILogger logger)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;
        _logger = logger;
    }

    private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
    protected async Task<T> WithSequencedConnection<T>(Func<NpgsqlConnection, Task<T>> func)
    {
        await semaphore.WaitAsync();
        try {
            await _connection.OpenAsync();
            return await func(_connection);
        }
        catch (Exception e) {
            _logger.LogError(e, "Error while executing database query");
            throw;
        }
        finally {
            await _connection.CloseAsync();
            semaphore.Release();
        }
    }

    protected async Task<T> WithTransactedConnection<T>(Func<NpgsqlConnection, Task<T>> func)
    {
        try {
            await _connection.OpenAsync();
            var tx = await _connection.BeginTransactionAsync();
            try {
                var result = await func(_connection);
                await tx.CommitAsync();
                return result;
            }
            catch (Exception) {
                await tx.RollbackAsync();
                throw;
            }
        }
        catch (Exception e) {
            _logger.LogError(e, "Error while executing database query");
            throw;
        }
        finally {
            await _connection.CloseAsync();
        }
    }
    protected async Task<T> WithConnection<T>(Func<NpgsqlConnection, Task<T>> func)
    {
        try {
            await _connection.OpenAsync();
            return await func(_connection);
        }
        catch (Exception e) {
            _logger.LogError(e, "Error while executing database query");
            throw;
        }
        finally {
            await _connection.CloseAsync();
        }
    }
}
