﻿namespace PoundPupLegacy.Db.Readers
{
    public interface IDatabaseReader : IAsyncDisposable
    {

    }
    public interface IDatabaseReader<T> : IDatabaseReader
        where T : IDatabaseReader<T>
    {
        public abstract static Task<T> CreateAsync(NpgsqlConnection connection);
    }
    public class DatabaseReader<T> : IDatabaseReader
    {
        protected NpgsqlCommand _command;
        internal DatabaseReader(NpgsqlCommand command)
        {
            _command = command;

        }
        public virtual async ValueTask DisposeAsync()
        {
            await _command.DisposeAsync();
        }

    }
}
