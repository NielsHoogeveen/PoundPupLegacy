using Npgsql;
using System.Data;
using System.Data.Common;

namespace PoundPupLegacy.EditModel.Readers;


public class NodeCreateDocumentReader<T> : NodeEditDocumentReader
where T : class, Node
{

    private int _nodeTypeId;
    protected NodeCreateDocumentReader(NpgsqlCommand command, int nodeTypeId) : base(command)
    {
        _nodeTypeId = nodeTypeId;
    }

    protected async static Task<NpgsqlCommand> CreateCommand(NpgsqlConnection connection, string sql)
    {
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;
        command.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("node_type_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Integer);
        await command.PrepareAsync();
        return command;
    }



    public async Task<T> ReadAsync(NodeCreateDocumentRequest request)
    {
        _command.Parameters["tenant_id"].Value = request.TenantId;
        _command.Parameters["node_type_id"].Value = request.NodeTypeId;
        _command.Parameters["user_id"].Value = request.UserId;
        await using var reader = await _command.ExecuteReaderAsync();
        await reader.ReadAsync();
        var text = reader.GetString(0); ;
        var node = reader.GetFieldValue<T>(0);
        return node;
    }



}
