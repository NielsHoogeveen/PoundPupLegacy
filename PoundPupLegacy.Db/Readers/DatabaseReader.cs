namespace PoundPupLegacy.Db.Readers
{
    internal interface IDatabaseReader : IDisposable
    {

    }
    internal interface IDatabaseReader<T> : IDatabaseReader
        where T : IDatabaseReader<T>
    {
        public abstract static T Create(NpgsqlConnection connection);
    }
    internal class DatabaseReader<T> : IDatabaseReader
    {
        protected NpgsqlCommand _command;
        internal DatabaseReader(NpgsqlCommand command)
        {
            _command = command;

        }
        public void Dispose()
        {
            _command.Dispose();
        }

    }
}
