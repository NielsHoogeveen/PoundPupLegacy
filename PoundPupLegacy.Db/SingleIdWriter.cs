using Npgsql;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

internal class SingleIdWriter : DatabaseWriter<int>
{

    internal SingleIdWriter(NpgsqlCommand command) : base(command)
    {
    }

    public override void Write(int id)
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

    public override void Write(T node)
    {
        _command.Parameters["id"].Value = node.Id;
        _command.ExecuteNonQuery();
    }
}
