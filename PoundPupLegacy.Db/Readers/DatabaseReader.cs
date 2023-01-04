namespace PoundPupLegacy.Db.Readers
{
    internal interface IDatabaseReader : IAsyncDisposable
    {

    }
    internal interface IDatabaseReader<T> : IDatabaseReader
        where T : IDatabaseReader<T>
    {
        public abstract static Task<T> CreateAsync(NpgsqlConnection connection);
    }
    internal class DatabaseReader<T> : IDatabaseReader
    {
        protected NpgsqlCommand _command;
        internal DatabaseReader(NpgsqlCommand command)
        {
            _command = command;

        }
        public async ValueTask DisposeAsync()
        {
            await _command.DisposeAsync();
        }

    }
}
