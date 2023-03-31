using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.EditModel;
using System.Reflection;

namespace PoundPupLegacy.Edit.Test
{
    public class TestDb
    {

        const string ConnectStringPostgresql = "Host=localhost;Username=niels;Password=niels;Database=ppl;Include Error Detail=True";

        [Fact]
        public async void AddReadersPrepare()
        {
            using var connection = new NpgsqlConnection(ConnectStringPostgresql);
            connection.Open();
            var creatorAssembly = Assembly.GetAssembly(typeof(Node));
            var types = creatorAssembly!.GetTypes().Where(x => x.IsAssignableTo(typeof(IDatabaseReaderFactory)) && !x.IsInterface && !x.IsAbstract && !x.IsGenericType);
            foreach (var type in types) {
                var i = Activator.CreateInstance(type);
                var m = type.GetMethod("CreateAsync", new Type[] { typeof(NpgsqlConnection) });
                var task = (Task)m!.Invoke(i, new object[] { connection });
                await task.ConfigureAwait(false);
                var result = task.GetType().GetProperty("Result");
                var reader = (IDatabaseReader)result.GetValue(task);
                Assert.True(reader.HasBeenPrepared);
                Assert.NotEqual(string.Empty, reader.Sql);
                await reader.DisposeAsync();
            }
            connection.Close();
        }
        [Fact]
        public async void AddUpdatersPrepare()
        {
            using var connection = new NpgsqlConnection(ConnectStringPostgresql);
            connection.Open();
            var creatorAssembly = Assembly.GetAssembly(typeof(Node));
            var types = creatorAssembly!.GetTypes().Where(x => x.IsAssignableTo(typeof(IDatabaseUpdaterFactory)) && !x.IsInterface && !x.IsAbstract && !x.IsGenericType);
            foreach (var type in types) {
                var i = Activator.CreateInstance(type);
                var m = type.GetMethod("CreateAsync", new Type[] { typeof(NpgsqlConnection) });
                var task = (Task)m!.Invoke(i, new object[] { connection });
                await task.ConfigureAwait(false);
                var result = task.GetType().GetProperty("Result");
                var reader = (IDatabaseUpdater)result.GetValue(task);
                Assert.True(reader.HasBeenPrepared);
                Assert.NotEqual(string.Empty, reader.Sql);
                await reader.DisposeAsync();
            }
            connection.Close();
        }
        [Fact]
        public async void AddDeletersPrepare()
        {
            using var connection = new NpgsqlConnection(ConnectStringPostgresql);
            connection.Open();
            var creatorAssembly = Assembly.GetAssembly(typeof(Node));
            var types = creatorAssembly!.GetTypes().Where(x => x.IsAssignableTo(typeof(IDatabaseDeleterFactory)) && !x.IsInterface && !x.IsAbstract && !x.IsGenericType);
            foreach (var type in types) {
                var i = Activator.CreateInstance(type);
                var m = type.GetMethod("CreateAsync", new Type[] { typeof(NpgsqlConnection) });
                var task = (Task)m!.Invoke(i, new object[] { connection });
                await task.ConfigureAwait(false);
                var result = task.GetType().GetProperty("Result");
                var reader = (IDatabaseDeleter)result.GetValue(task);
                Assert.True(reader.HasBeenPrepared);
                Assert.NotEqual(string.Empty, reader.Sql);
                await reader.DisposeAsync();
            }
            connection.Close();
        }
    }
}