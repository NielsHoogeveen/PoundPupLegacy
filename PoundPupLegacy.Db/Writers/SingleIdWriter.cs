using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;
using System.Data;
namespace PoundPupLegacy.Db.Writers;

internal class SingleIdWriter : DatabaseWriter<int>
{
    internal static NpgsqlCommand CreateSingleIdCommand(string tableName, NpgsqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = $"""INSERT INTO public."{tableName}" (id) VALUES(@id)""";
        command.Parameters.Add("id", NpgsqlDbType.Integer);
        return command;
    }

    internal static DatabaseWriter<int> CreateSingleIdWriter(string tableName, NpgsqlConnection connection)
    {
        return new SingleIdWriter(CreateSingleIdCommand(tableName, connection));

    }

    private SingleIdWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(int id)
    {
        _command.Parameters["id"].Value = id;
        _command.ExecuteNonQuery();
    }
}
internal class SingleIdWriter<T> : DatabaseWriter<T> where T : Node
{

    internal SingleIdWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(T node)
    {
        _command.Parameters["id"].Value = node.Id;
        _command.ExecuteNonQuery();
    }
}
