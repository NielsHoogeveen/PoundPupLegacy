using Npgsql;
using PoundPupLegacy.Db.Writers;
using System.Reflection;

namespace PoundPupLegacy.Db.Test
{
    public class TestDbWriters
    {

        const string ConnectStringPostgresql = "Host=localhost;Username=postgres;Password=niels;Database=ppl;Include Error Detail=True";
        [Fact]
        public void Test1()
        {
            using var connection = new NpgsqlConnection(ConnectStringPostgresql);
            connection.Open();
            var writerAssembly = Assembly.GetAssembly(typeof(UserCreator));
            var types = writerAssembly!.GetTypes().Where(x => x.IsAssignableTo(typeof(IDatabaseWriter)) && !x.IsInterface && !x.IsAbstract);
            foreach (var type in types) {
                var m = type.GetMethod("Create", new Type[] { typeof(NpgsqlConnection) });
                var w = m!.Invoke(null, new object[] { connection }) as IDisposable;
                w!.Dispose();
            }
            connection.Close();
        }
    }
}