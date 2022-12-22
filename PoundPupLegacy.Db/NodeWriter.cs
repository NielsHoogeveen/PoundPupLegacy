using Npgsql;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;
public class NodeWriter : DatabaseWriter<Node>
{
    public NodeWriter(NpgsqlCommand command) : base(command)
    {
    }

    public override void Write(Node node)
    {
        _command.Parameters["id"].Value = node.Id;
        _command.Parameters["user_id"].Value = node.UserId;
        _command.Parameters["created"].Value = node.Created;
        _command.Parameters["changed"].Value = node.Changed;
        _command.Parameters["title"].Value = node.Title;
        _command.Parameters["status"].Value = node.Status;
        _command.Parameters["node_type_id"].Value = node.NodeTypeId;
        _command.Parameters["is_term"].Value = node.IsTerm;
        _command.ExecuteNonQuery();
    }
}
