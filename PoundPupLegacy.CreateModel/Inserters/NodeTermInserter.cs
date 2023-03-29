using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PoundPupLegacy.Db.Test")]
namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class NodeTermInserter : DatabaseInserter<NodeTerm>, IDatabaseInserter<NodeTerm>
{
    private const string NODE_ID = "node_id";
    private const string TERM_ID = "term_id";
    public static async Task<DatabaseInserter<NodeTerm>> CreateAsync(NpgsqlConnection connection)
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
        return new NodeTermInserter(command);

    }

    internal NodeTermInserter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(NodeTerm nodeTerm)
    {
        WriteValue(nodeTerm.NodeId, NODE_ID);
        WriteValue(nodeTerm.TermId, TERM_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
