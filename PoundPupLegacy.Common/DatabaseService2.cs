using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;

namespace PoundPupLegacy.Common;

public abstract class DatabaseService2(NpgsqlDataSource _dataSource, ILogger _logger)
{
    private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
    protected async Task<T> WithSequencedConnection<T>(Func<NpgsqlConnection, Task<T>> func)
    {
        await semaphore.WaitAsync();
        var connection = _dataSource.CreateConnection();
        try {
            await connection.OpenAsync();
            return await func(connection);
        }
        catch (Exception e) {
            _logger.LogError(e, "Error while executing database query");
            throw;
        }
        finally {
            if (connection.State == ConnectionState.Open) {
                await connection.CloseAsync();
            }
            semaphore.Release();
        }
    }

    protected async Task<T> WithTransactedConnection<T>(Func<NpgsqlConnection, Task<T>> func)
    {
        var connection = _dataSource.CreateConnection();
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
            _logger.LogError(e, "Error while executing database query");
            throw;
        }
        finally {
            if (connection.State == ConnectionState.Open) {
                await connection.CloseAsync();
            }
        }
    }
    protected async Task<T> WithConnection<T>(Func<NpgsqlConnection, Task<T>> func)
    {
        var connection = _dataSource.CreateConnection();
        try {
            await connection.OpenAsync();
            return await func(connection);
        }
        catch (Exception e) {
            _logger.LogError(e, "Error while executing database query");
            throw;
        }
        finally {
            if (connection.State == ConnectionState.Open) {
                await connection.CloseAsync();
            }
        }
    }
}
