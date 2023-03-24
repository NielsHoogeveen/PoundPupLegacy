using System.Data;

namespace PoundPupLegacy.Db.Readers;

public sealed class EditNodeActionIdReaderByNodeTypeId : DatabaseReader, IDatabaseReader<EditNodeActionIdReaderByNodeTypeId>
{
    public static async Task<EditNodeActionIdReaderByNodeTypeId> CreateAsync(NpgsqlConnection connection)
    {
        var sql = """
            SELECT id FROM edit_node_action WHERE node_type_id = @node_type_id
            """;

        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;

        command.Parameters.Add("node_type_id", NpgsqlDbType.Integer);
        await command.PrepareAsync();

        return new EditNodeActionIdReaderByNodeTypeId(command);

    }

    internal EditNodeActionIdReaderByNodeTypeId(NpgsqlCommand command) : base(command) { }

    public async Task<int> ReadAsync(int nodeTypeId)
    {
        _command.Parameters["node_type_id"].Value = nodeTypeId;

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows) {
            await reader.ReadAsync();
            var id = reader.GetInt32("id");
            await reader.CloseAsync();
            return id;
        }
        await reader.CloseAsync();
        throw new Exception($"edit node action cannot be found for node type  {nodeTypeId}");
    }
}
