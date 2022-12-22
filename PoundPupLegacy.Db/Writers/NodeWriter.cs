using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;
using System.Data;
namespace PoundPupLegacy.Db.Writers;
public class NodeWriter : DatabaseWriter<Node>, IDatabaseWriter<Node>
{
    public static DatabaseWriter<Node> Create(NpgsqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = """INSERT INTO public."node" (id, user_id, created, changed, title, status, node_type_id, is_term) VALUES(@id, @user_id, @created, @changed, @title, @status, @node_type_id, @is_term)""";
        command.Parameters.Add("id", NpgsqlDbType.Integer);
        command.Parameters.Add("user_id", NpgsqlDbType.Integer);
        command.Parameters.Add("created", NpgsqlDbType.Timestamp);
        command.Parameters.Add("changed", NpgsqlDbType.Timestamp);
        command.Parameters.Add("title", NpgsqlDbType.Varchar);
        command.Parameters.Add("status", NpgsqlDbType.Integer);
        command.Parameters.Add("node_type_id", NpgsqlDbType.Integer);
        command.Parameters.Add("is_term", NpgsqlDbType.Boolean);
        command.Prepare();
        return new NodeWriter(command);
    }


    private NodeWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(Node node)
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
