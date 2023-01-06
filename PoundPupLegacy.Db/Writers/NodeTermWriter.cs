using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PoundPupLegacy.Db.Test")]
namespace PoundPupLegacy.Db.Writers;

internal class NodeTermWriter : DatabaseWriter<NodeTerm>, IDatabaseWriter<NodeTerm>
{
    private const string NODE_ID = "node_id";
    private const string TERM_ID = "term_id";
    public static async Task<DatabaseWriter<NodeTerm>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "node_term",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = NODE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = TERM_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new NodeTermWriter(command);

    }

    internal NodeTermWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(NodeTerm nodeTerm)
    {
        WriteValue(nodeTerm.NodeId, NODE_ID);
        WriteValue(nodeTerm.TermId, TERM_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
