using Npgsql;
using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.CreateModel.Inserters;
using System.Reflection;

namespace PoundPupLegacy.Db.Test
{
    public class TestDbInserters
    {

        const string ConnectStringPostgresql = "Host=localhost;Username=niels;Password=niels;Database=ppl;Include Error Detail=True";
        [Fact]
        public async void Test1()
        {
            using var connection = new NpgsqlConnection(ConnectStringPostgresql);
            connection.Open();
            var creatorAssembly = Assembly.GetAssembly(typeof(UserCreator));
            var types = creatorAssembly!.GetTypes().Where(x => x.IsAssignableTo(typeof(IDatabaseInserter)) && !x.IsInterface && !x.IsAbstract && !x.IsGenericType);
            foreach (var type in types.Where(x => x.Name != "SingleIdInserter")) {
                var m = type.GetMethod("CreateAsync", new Type[] { typeof(NpgsqlConnection) });
                var w = m!.Invoke(null, new object[] { connection }) as IDisposable;
                var w2 = (Task)w;
                await w2;
                w!.Dispose();
            }
            connection.Close();
        }
    }
}