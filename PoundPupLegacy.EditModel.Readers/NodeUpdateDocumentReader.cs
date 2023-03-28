using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.EditModel.Readers;

public abstract class NodeUpdateDocumentReaderFactory<T> : NodeEditDocumentReaderFactory<T>
where T : class, IDatabaseReader
{
    protected async Task<NpgsqlCommand> CreateCommand(NpgsqlConnection connection, string sql)
    {
        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;
        command.Parameters.Add("url_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("node_type_id", NpgsqlTypes.NpgsqlDbType.Integer);
        await command.PrepareAsync();
        return command;
    }
}

public class NodeUpdateDocumentReader<T> : NodeEditDocumentReader, ISingleItemDatabaseReader<NodeEditDocumentReader.NodeUpdateDocumentRequest, T>
where T : class, Node
{

    private int _nodeTypeId;
    protected NodeUpdateDocumentReader(NpgsqlCommand command, int nodeTypeId) : base(command)
    {
        _nodeTypeId = nodeTypeId;
    }


    public async Task<T> ReadAsync(NodeUpdateDocumentRequest request)
    {
        _command.Parameters["url_id"].Value = request.UrlId;
        _command.Parameters["user_id"].Value = request.UserId;
        _command.Parameters["tenant_id"].Value = request.TenantId;
        _command.Parameters["node_type_id"].Value = _nodeTypeId;
        await using var reader = await _command.ExecuteReaderAsync();
        await reader.ReadAsync();
        var text = reader.GetString(0); ;
        var node = reader.GetFieldValue<T>(0);
        return node;
    }
}
